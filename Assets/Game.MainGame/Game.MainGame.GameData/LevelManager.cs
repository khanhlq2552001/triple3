using System.Collections;
using System.Collections.Generic;
using BlitzyUI;
using DG.Tweening;
using Game.Modules.Events;
using Lean.Pool;
using UnityEngine;

namespace Game.MainGame
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [SerializeField] private ItemGrid _objTile1;
        [SerializeField] private Data _data;
        [SerializeField] private DataLevel _dataLevels;
        [SerializeField] private float _spacing = 1.1f;
        [SerializeField] private List<Transform>  _tranParent;
        [SerializeField] private List<ItemGrid> _items;

        public DataSpriteThem dataThemes;
        public SlotUnder slotUnder;
        public GameController controller;

        private int _idOrder = 0;
        private int _countItem = 0;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
      //      GenerateData();
        }

        public List<ItemGrid> GetItems()
        {
            return _items;
        }

        public DataLevel GetDatas()
        {
            return _dataLevels;
        }

        public void SetLevel(int Level)
        {
            int id = (Level - 1) % (_dataLevels.listData.Count);

            _data = _dataLevels.listData[id];
        }

        public void GenerateData()
        {
            dataThemes.spritesThemes[0].listSpriteTheme.Shuffle();
            controller.State = StateController.pause;
            _items.Clear();
            _idOrder = 0;
            _countItem = 0;
            slotUnder.gameObject.SetActive(true);
            UIGamePlay ui = UIManager.Instance.GetScreen<UIGamePlay>(GameManager.ScreenId_UIGamePlay);
            ui.StartCountDown(180);

          for(int i=0; i< _data.grids.Count; i++)
            {
                int idx = i;
                _countItem += _data.grids[i].idItems.Count;
                SpawnGrid(_data.grids[idx], idx);
            }
        }

        public void SpawnGrid(Grid grid, int id)
        {
            // Tính toán kích thước tổng thể của lưới
            float totalWidth = (grid.width - 1) * _spacing;
            float totalHeight = (grid.height - 1) * _spacing;
            int count = (grid.width * grid.height);
            int idCount= 0;
            int idGrid = 0;
            Vector2 startPosition = Vector2.zero;
            List<ItemGrid> itemsidGrid = new List<ItemGrid>();

        // Tính toán vị trí bắt đầu để lưới được đặt ở giữa màn hình

        startPosition = new Vector2(-totalWidth * 0.5f, totalHeight * 0.5f + 0.5f + 0.5f * id);

            for (int i = 0; i < count; i++)
            {
                int x = i % grid.width;
                int y = i / grid.width;
                Vector2 position = startPosition + new Vector2(x * _spacing, -y * _spacing);

                ItemGrid o = new ItemGrid();

                // neu nhu co phan tu can xoa
                if(grid.idsRemoves.Count > 0)
                {
                    if (i != grid.idsRemoves[idCount])
                    {
                        o = LeanPool.Spawn(_objTile1, position, Quaternion.identity, _tranParent[id]);

                        o.IDGrid = id;
                        o.name = "tileGrid_" + id;
                        o.ResetAtribute();
                        itemsidGrid.Add(o);
                        _items.Add(o);
                        o.SetId(grid.idItems[idGrid], _data.idTheme);
                        o.SetOrderInLayer(_idOrder * 2);
                        o.OrderBG = _idOrder * 2;
                        o.PosStart = position;
                        _idOrder++;
                        idGrid++;
                    }
                    else
                    {
                        if (idCount < (grid.idsRemoves.Count - 1)) idCount++;
                    }
                }
                else
                {
                        o = LeanPool.Spawn(_objTile1, position, Quaternion.identity, _tranParent[id]);

                        o.IDGrid = id;
                        o.name = "tileGrid_" + id;
                        o.ResetAtribute();
                        itemsidGrid.Add(o);
                        _items.Add(o);
                        o.SetId(grid.idItems[idGrid], _data.idTheme);
                        o.SetOrderInLayer(_idOrder * 2);
                        o.OrderBG = _idOrder * 2;
                        o.PosStart = position;
                        _idOrder++;
                        idGrid++;
                } // khong co phan tu nao can xoa
            }

            if(id > 0)
            {
                StartCoroutine(EffectStart(id, itemsidGrid));
            }
        }

        IEnumerator EffectStart(int idGrid, List<ItemGrid> listItem)
        {
            yield return new WaitForSeconds(idGrid * 0.15f);
            _tranParent[idGrid].transform.position = new Vector3(_tranParent[idGrid].transform.position.x, _tranParent[idGrid].transform.position.y - 6f, 0f);
            _tranParent[idGrid].DOMove(Vector3.zero, 0.25f).SetEase(Ease.InOutSine).OnComplete(() => {
                for(int i=0; i< listItem.Count; i++)
                {
                    listItem[i].CheckOverlapCollider();

                    if(idGrid == (_data.grids.Count - 1))
                    {
                        controller.State = StateController.normal;
                    }
                }
                });
        }

        public void RestartLevel()
        {
            BoosterRestart();
        }

        public void Restart()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].gameObject.active)
                {
                    LeanPool.Despawn(_items[i]);
                }
            }
            _items.Clear();
            slotUnder.ClearData();
            GenerateData();
        }

        public void CountDownCountItem()
        {
            _countItem--;

            if(_countItem == 0)
            {
                StartCoroutine(DelayWinCoroutine());
            }
        }
        IEnumerator DelayWinCoroutine()
        {
            controller.State = StateController.pause;
            yield return new WaitForSeconds(1f);
            UIManager.Instance.QueuePush(GameManager.ScreenID_UIWin, null, "UIWin", null);
        }

        public void BoosterBack()
        {
            if (Manager.Instance.UserData.quantityBoosterBack > 0)
            {
                slotUnder.BoosterBack();
            }
            else
            {
                UIManager.Instance.QueuePush(GameManager.ScreenUi_BuyBooster, null, "UIBuyBooster", null);
                UIBuyBooster ui = UIManager.Instance.GetScreen<UIBuyBooster>(GameManager.ScreenUi_BuyBooster);
                ui.type = BoosterType.Back;
                ui.SetUpBoosterBuy();
            }
        }

        public void BoosterRestart()
        {
            if (Manager.Instance.UserData.quantityBoosterRestart > 0)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].gameObject.active)
                    {
                        LeanPool.Despawn(_items[i]);
                    }
                }
                _items.Clear();
                slotUnder.ClearData();
                GenerateData();
                Manager.Instance.UserData.quantityBoosterRestart--;
                EventManager.Raise(new EventUpdateBooster() { });
            }
            else
            {
                UIManager.Instance.QueuePush(GameManager.ScreenUi_BuyBooster, null, "UIBuyBooster", null);
                UIBuyBooster ui = UIManager.Instance.GetScreen<UIBuyBooster>(GameManager.ScreenUi_BuyBooster);
                ui.type = BoosterType.Replay;
                ui.SetUpBoosterBuy();
            }
        }

        public void ClearData()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].gameObject.active)
                {
                    LeanPool.Despawn(_items[i]);
                }
            }

            _items.Clear();
        }

        public void BoosterLight()
        {
            if (Manager.Instance.UserData.quantityBoosterHint > 0)
            {
                slotUnder.BoosterLight();
                Manager.Instance.UserData.quantityBoosterHint--;
                EventManager.Raise(new EventUpdateBooster() { });
            }
            else
            {
                UIManager.Instance.QueuePush(GameManager.ScreenUi_BuyBooster, null, "UIBuyBooster", null);
                UIBuyBooster ui = UIManager.Instance.GetScreen<UIBuyBooster>(GameManager.ScreenUi_BuyBooster);
                ui.type = BoosterType.Hint;
                ui.SetUpBoosterBuy();
            }
        }
    }
}

[System.Serializable]
public class Data
{
    public int idTheme;
    public List<Grid> grids;
}

[System.Serializable]
public class Grid
{
    public int width;
    public int height;
    public List<int> idsRemoves;
    public List<int> idItems;
}

