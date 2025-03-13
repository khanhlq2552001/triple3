using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace Game.MainGame
{
    public enum StateItem
    {
        DontMove,
        CanMove,
        Queue
    }

    public class ItemGrid : MonoBehaviour
    {
        [SerializeField] private StateItem _state;
        [SerializeField] private int _id;
        [SerializeField] private int _idGrid;
        [SerializeField] private List<ItemGrid> _listGridTop;
        [SerializeField] private List<ItemGrid> _listGridDown;
        [SerializeField] private SpriteRenderer _sprItem;
        [SerializeField] private SpriteRenderer _sprBG;
        [SerializeField] private Color _colorShadow;

        private int _orderSprBG;
        private Vector3 _posStart;
        private GameObject _trail;
        private Rigidbody2D _rb;

        public int idSlotUnder;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public StateItem State
        {
            get => _state;

            set {
                _state = value;

                switch (_state)
                {
                    case StateItem.DontMove:
                        SetColorShadow();
                        break;
                    case StateItem.CanMove:
                        SetColorLight();
                        break;
                }
            }
        }

        public int OrderBG
        {
            get => _orderSprBG;

            set => _orderSprBG = value;
        }

        public Vector3 PosStart
        {
            get => _posStart;

            set => _posStart = value;
        }

        public int ID
        {
            get => _id;

            set => _id = value;
        }

        public int IDGrid
        {
            get => _idGrid;

            set => _idGrid = value;
        }

        public void SetId(int id, int idTheme)
        {
            ID = id;
            _sprItem.sprite = LevelManager.Instance.dataThemes.spritesThemes[idTheme].listSpriteTheme[id];
        }

        public void AddGridTop(ItemGrid item)
        {
            _listGridTop.Add(item);

            State = StateItem.DontMove;
        }

        public void AddGridDown(ItemGrid item)
        {
            _listGridDown.Add(item);
        }

        public void RemoveGridTop(ItemGrid item)
        {
            _listGridTop.Remove(item);

            if(_listGridTop.Count == 0)
            {
                State = StateItem.CanMove;
            }
        }

        public void RemoveGridDown(ItemGrid item)
        {
            _listGridDown.Remove(item);
        }

        public void ResetAtribute()
        {
            State = StateItem.CanMove;
            _rb.bodyType = RigidbodyType2D.Static;
            _listGridDown.Clear();
            _listGridTop.Clear();
        }

        public void SetColorShadow()
        {
            _sprBG.DOColor(_colorShadow, 0.1f);
            _sprItem.DOColor(_colorShadow, 0.1f);
        }

        public void SetColorLight()
        {
            _sprBG.DOColor(Color.white, 0.1f);
            _sprItem.DOColor(Color.white, 0.1f);
        }

        public void SetOrderInLayer(int orderBG)
        {
            _sprBG.sortingOrder = orderBG;
            _sprItem.sortingOrder = orderBG + 1;
        }

        public void CheckMovingQueue()
        {
            for(int i=0; i< _listGridDown.Count; i++)
            {
                _listGridDown[i].RemoveGridTop(this);
                _listGridDown[i].CheckOverLapTop();
            }

            for(int i=0; i< _listGridTop.Count; i++)
            {
                _listGridTop[i].RemoveGridDown(this);
            }
        }

        public void CheckOverlapCollider()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
            foreach (Collider2D col in hitColliders)
            {
                if (col.gameObject.name == ("tileGrid_" + (IDGrid -1)))
                {
                    ItemGrid itemGrid = col.gameObject.GetComponent<ItemGrid>();
                    itemGrid.AddGridTop(this);
                    AddGridDown(itemGrid);
                }
            }
        }

        public void CheckOverLapTop()
        {
            _listGridTop.Clear();
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
            foreach (Collider2D col in hitColliders)
            {
                ItemGrid item = col.GetComponent<ItemGrid>();

                if (item != null)
                {
                    if(item.IDGrid > IDGrid)
                    {
                        AddGridTop(item);
                    }
                }
            }
        }

        public void EffectLose()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;

            float randomAngle = UnityEngine.Random.Range(0f, 360f);

            Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

            // Chọn lực ngẫu nhiên từ 10 đến 20
            float randomForce = UnityEngine.Random.Range(5f, 10f);

            _rb.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);

            float randomTorque = UnityEngine.Random.Range(-10f, 10f); // Xoay theo hướng ngẫu nhiên
            _rb.AddTorque(randomTorque, ForceMode2D.Impulse);
        }

        public void EndItemGrid(float time)
        {
            StartCoroutine(DelayEndCoroutine(time));
        }

        IEnumerator DelayEndCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            transform.DOScale(new Vector3(1.1f, 1.1f, 0), 0.1f).OnComplete(() => {
                transform.DOScale(new Vector3(1f, 1f, 0), 0.1f).OnComplete(() => {
                    LeanPool.Spawn(GameManager.Instance.fxSmoke, transform.position, Quaternion.identity);
                    LevelManager.Instance.CountDownCountItem();
                    LeanPool.Despawn(gameObject);
                });
            });

        }

        public void ScaleItem()
        {
            SetOrderInLayer(2000);
            transform.DOScale(new Vector3(1.1f, 1.1f, 0), 0.2f);
        }

        public void ScaleNormalItem(bool isTrue)
        {
            if (isTrue)
            {
                SetOrderInLayer(_orderSprBG);
            }

            transform.DOScale(new Vector3(1f, 1f, 0), 0.2f);
        }

        public void MovingTarget(Transform target, float time, bool isQueue, Action action = null)
        {
            if (!isQueue)
            {
                transform.DOMove(target.position, time).SetEase(Ease.InOutCubic).OnComplete(() => {
                    action?.Invoke();
                });
            }
            else
            {
                Vector3 startPos = transform.position;
                Vector3 endPos = target.position;

                // Tính điểm giữa
                Vector3 midPoint = (startPos + endPos) / 2;

                // Vector hướng từ start đến end
                Vector3 direction = (endPos - startPos).normalized;

                // Tạo vector vuông góc trong 2D (quay 90 độ)
                Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);

                // Chọn một góc ngẫu nhiên từ -10 đến 10 độ
                float randomAngle = UnityEngine.Random.Range(-10f, 10f);
                float curveAmount = Vector3.Distance(startPos, endPos) * Mathf.Tan(Mathf.Deg2Rad * randomAngle);

                // Dịch chuyển điểm giữa theo vector vuông góc
                Vector3 controlPoint = midPoint + perpendicular * curveAmount;

                // Mảng các điểm để DoPath sử dụng
                Vector3[] pathPoints = { startPos, controlPoint, endPos };

                _trail = LeanPool.Spawn(GameManager.Instance.trail, transform.position, Quaternion.identity);
                GameManager.Instance.onActionUpdate += UpdateTrail;

                // Di chuyển đối tượng theo đường cong
                transform.DOPath(pathPoints, time, PathType.CatmullRom, PathMode.Ignore)
                         .SetEase(Ease.InOutCubic).OnComplete(() => {
                             action?.Invoke();
                             if (isQueue)
                             {
                                 transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutCubic);
                                 GameManager.Instance.onActionUpdate -= UpdateTrail;
                                 LeanPool.Despawn(_trail);
                             }
                         });
            }
        }

        public void UpdateTrail()
        {
            _trail.transform.position = transform.position;
        }

        public void MovingStart(float time)
        {
            transform.DOMove(PosStart, time).SetEase(Ease.InOutSine).OnComplete(() => {
                State = StateItem.CanMove;
                CheckOverlapCollider();
                SetOrderInLayer(_orderSprBG);
            });
        }
    }
}
