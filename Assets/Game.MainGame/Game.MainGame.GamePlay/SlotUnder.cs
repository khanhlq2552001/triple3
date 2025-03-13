using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlitzyUI;
using DG.Tweening;
using UnityEngine;

namespace Game.MainGame
{
    public class SlotUnder : MonoBehaviour
    {
        [SerializeField] private Transform[] _tranSlots = new Transform[7];
        [SerializeField] private ItemGrid[] _itemGrids = new ItemGrid[7];
        [SerializeField] private ParticleSystem _partiSmoke;

        private int _id;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetItemUnder(int id, ItemGrid item)
        {
            for (int i= _itemGrids.Length -1; i >=0 ; i--)
            {
                if (_itemGrids[i] != null)
                {
                    if (_itemGrids[i].ID == id)
                    {
                        _id = i +1;
                        MovingRight(i + 1, item);
                        item.State = StateItem.Queue;
                        item.MovingTarget(_tranSlots[i + 1], 0.3f,true, item.CheckMovingQueue);
                        CheckEat();
                        CheckBoosterLight();
                        CheckWarning();
                        CheckLose();
                        return;
                    }
                }
            }

            for(int i=0; i< _itemGrids.Length; i++)
            {
                if (_itemGrids[i] == null)
                {
                    _itemGrids[i] = item;
                    item.State = StateItem.Queue;
                    item.SetOrderInLayer(i * 2 + 2000);
                    item.MovingTarget(_tranSlots[i], 0.3f, true, item.CheckMovingQueue);
                    CheckBoosterLight();
                    CheckWarning();
                    CheckLose();
                    return;
                }
            }
        }

        public void ClearData()
        {
            _animator.SetBool("isLose", false);
            _animator.SetBool("warning", false);
            for (int i=0; i< _itemGrids.Length; i++)
            {
                _itemGrids[i] = null;
            }
        }

        private void MovingRight(int id, ItemGrid item)
        {
            for(int i= (_itemGrids.Length -1); i >= id ; i--)
            {
                if (_itemGrids[i] != null)
                {
                    int idx =i;
                    _itemGrids[i].transform.DOMove(_tranSlots[i + 1].position, 0.3f).SetEase(Ease.InOutSine);
                    _itemGrids[i + 1] = _itemGrids[i];
                    _itemGrids[i + 1].SetOrderInLayer((i +1) * 2 + 2000);
                }
                if( i == id)
                {
                    _itemGrids[i] = item;
                    item.SetOrderInLayer(i * 2 + 2000);
                    return;
                }
            }
        }

        private void CheckEat()
        {
            int idCheck = _itemGrids[_id].ID;

            if (_id < 2) return;

            if(_itemGrids[_id - 1] == null || _itemGrids[_id - 2] == null)
            {
                CheckLose();
                return;
            }

            if (_itemGrids[_id - 1].ID == idCheck && _itemGrids[_id -2].ID == idCheck)
            {
                _itemGrids[_id - 1].EndItemGrid(0.3f);
                _itemGrids[_id].EndItemGrid(0.3f);
                _itemGrids[_id - 2].EndItemGrid(0.3f);
                _itemGrids[_id - 1] = null;
                _itemGrids[_id] = null;
                _itemGrids[_id - 2] = null;
                GomHang(_id);
            }

            CheckLose();
        }

        private void CheckLose()
        {
            if (_itemGrids[_itemGrids.Length - 1] != null)
            {
                _animator.SetBool("isLose", true);
                _partiSmoke.Play();
                List<ItemGrid> listItem = LevelManager.Instance.GetItems();
                for(int i=0; i< listItem.Count; i++)
                {
                    if (listItem[i].gameObject.active)
                    {
                        listItem[i].State = StateItem.Queue;
                        listItem[i].SetColorLight();
                        listItem[i].EffectLose();
                        StartCoroutine(DelayLoseCoroutine());
                    }
                }
            }
        }

        IEnumerator DelayLoseCoroutine()
        {
            LevelManager.Instance.controller.State = StateController.pause;
            yield return new WaitForSeconds(1.5f);
            UIManager.Instance.QueuePush(GameManager.ScreenID_UILose, null, "UILose", null);
        }

        private void GomHang(int idx)
        {
            for(int i= (idx +1); i < _itemGrids.Length; i++)
            {
                if (_itemGrids[i] != null)
                {
                    int id = i;
                    StartCoroutine(DelayGomHangCoroutine(_itemGrids[id], id - 3));
                    _itemGrids[i - 3] = _itemGrids[i];
                    _itemGrids[i - 3].SetOrderInLayer((i - 3) * 2 + 2000);
                    _itemGrids[i] = null;
                }
                else
                {
                    return;
                }
            }
        }

        IEnumerator DelayGomHangCoroutine(ItemGrid item,int id)
        {
            yield return new WaitForSeconds(0.4f);
            item.MovingTarget(_tranSlots[id], 0.3f, false);
        }

        public void BoosterBack()
        {
            for(int i= _itemGrids.Length -1; i >= 0; i--)
            {
                if (_itemGrids[i] != null)
                {
                    _itemGrids[i].MovingStart(0.3f);
                    _itemGrids[i] = null;
                    CheckBoosterLight();
                    return;
                }
            }
        }

        public void CheckWarning()
        {
            if (_itemGrids[_itemGrids.Length - 2] != null)
            {
                _animator.SetBool("warning", true);
            }
            else
            {
                _animator.SetBool("warning", false);
            }
        }

        public void CheckBoosterLight()
        {
            UIGamePlay ui =  UIManager.Instance.GetScreen<UIGamePlay>(GameManager.ScreenId_UIGamePlay);

            if (_itemGrids[_itemGrids.Length -2] != null)
            {
                bool allUnique = _itemGrids.Where(item => item != null)
                      .Select(item => item.ID) // Lấy ID
                      .Distinct() // Loại bỏ trùng lặp
                      .Count() == _itemGrids.Count(item => item != null);
                if (allUnique)
                {
                    ui.SetActiveBoosterLight(false);
                }
                else
                {
                    ui.SetActiveBoosterLight(true);
                }
            }
            else
            {
                ui.SetActiveBoosterLight(true);
            }
        }

        public void BoosterLight()
        {
                int? firstDuplicateId = _itemGrids.Where(item => item != null)
                    .GroupBy(item => item.ID)
                    .Where(group => group.Count() >= 2)
                    .Select(group => (int?)group.Key) // Chuyển sang nullable int
                    .FirstOrDefault(); // Trả về null nếu không tìm thấy
                List<ItemGrid> listItem = LevelManager.Instance.GetItems();
                if (firstDuplicateId != null)
                {
                    for(int i=0; i< listItem.Count; i++)
                    {
                        if ((listItem[i].State == StateItem.DontMove || listItem[i].State == StateItem.CanMove) && listItem[i].ID == firstDuplicateId)
                        {
                            listItem[i].CheckMovingQueue();
                            listItem[i].SetColorLight();
                            SetItemUnder(listItem[i].ID, listItem[i]);
                            return;
                        }
                    }
                }
                else
                {
                    int firstId = 0;
                    bool th2= false;
                    for(int i=0; i< _itemGrids.Length; i++)
                    {
                        if (_itemGrids[i]!= null)
                        {
                            firstId = _itemGrids[i].ID;
                            th2 = true;
                            break;
                        }
                    }

                    if (th2)
                    {
                        int count = 0;
                        for (int i = 0; i < listItem.Count; i++)
                        {
                            if ((listItem[i].State == StateItem.DontMove || listItem[i].State == StateItem.CanMove) && listItem[i].ID == firstId)
                            {
                                listItem[i].CheckMovingQueue();
                                listItem[i].SetColorLight();
                                SetItemUnder(listItem[i].ID, listItem[i]);
                                count++;

                                if (count == 2)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

                int idChoose = 0;
                int count1 = 0;
                for (int i = listItem.Count -1; i >= 0; i--)
                {
                    if ((listItem[i].State == StateItem.DontMove || listItem[i].State == StateItem.CanMove))
                    {
                        idChoose = listItem[i].ID;
                    break;
                    }
                }

                for (int i = listItem.Count -1; i >= 0 ; i--)
                {
                    if ((listItem[i].State == StateItem.DontMove || listItem[i].State == StateItem.CanMove) && listItem[i].ID == idChoose)
                    {
                        listItem[i].CheckMovingQueue();
                        listItem[i].SetColorLight();
                        SetItemUnder(listItem[i].ID, listItem[i]);
                        count1++;

                        if (count1 == 3)
                        {
                            return;
                        }
                    }
                }
        }
    }
}
