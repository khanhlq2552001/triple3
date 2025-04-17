using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class PageView : MonoBehaviour
    {
        public ScrollPage[] scrollPages = new ScrollPage[2];
        public int idCurrentPage;
        [HideInInspector] public int idPageCount;
        public Transform[] transPages = new Transform[3];
        public AnimationCurve moveCurve;
        public float timeAnimator;

        private bool _isScroll = true;

        [SerializeField] private Text _txtPage;

        private void Awake()
        {
            for (int i = 0; i < scrollPages.Length; i++)
                scrollPages[i].pageView = this;
        }

        public void SetPage()
        {
            int countPage = Manager.Instance.UserData.levelLevel / 12;
            idCurrentPage = 0;
            idPageCount = countPage;

            scrollPages[0].transform.position = transPages[0].position;
            scrollPages[1].transform.position = transPages[1].position;
            scrollPages[0].countPage = countPage;
            scrollPages[0].SetData();
        }

        public void SetText()
        {
            if(_txtPage != null)
            {
                _txtPage.text = "Page " + (idPageCount + 1);
            }
        }

        public void NextPage()
        {
            if (!_isScroll) return;

            idPageCount++;

            SetText();

            if (idCurrentPage == 0)
            {
                _isScroll = false;
                idCurrentPage = 1;
                scrollPages[1].countPage = idPageCount;
                scrollPages[1].SetData();
                scrollPages[1].transform.position = transPages[1].position;

                scrollPages[1].transform.DOMove(transPages[0].position, timeAnimator)
                .SetEase(Ease.Linear);

                scrollPages[0].transform.DOMove(transPages[2].position, timeAnimator)
                .SetEase(Ease.Linear).OnComplete(() => {

                    _isScroll = true;
                });
                return;
            }

            if (idCurrentPage == 1)
            {
                _isScroll = false;
                idCurrentPage = 0;
                scrollPages[0].countPage = idPageCount;
                scrollPages[0].SetData();
                scrollPages[0].transform.position = transPages[1].position;

                scrollPages[0].transform.DOMove(transPages[0].position, timeAnimator)
                .SetEase(Ease.Linear);

                scrollPages[1].transform.DOMove(transPages[2].position, timeAnimator)
                .SetEase(Ease.Linear).OnComplete(() => {
                    _isScroll = true;
                });
                return;
            }
        }

        public void UndoPage()
        {
            if (!_isScroll) return;



            idPageCount--;
            if(idPageCount < 0)
            {
                idPageCount = 0;
                return;
            }
            SetText();

            if (idCurrentPage == 0)
            {
                _isScroll = false;
                idCurrentPage = 1;
                scrollPages[1].countPage = idPageCount;
                scrollPages[1].SetData();
                scrollPages[1].transform.position = transPages[2].position;
                scrollPages[1].transform.DOMove(transPages[0].position, timeAnimator)
                .SetEase(Ease.Linear);
                scrollPages[0].transform.DOMove(transPages[1].position, timeAnimator)
                .SetEase(Ease.Linear).OnComplete(() => {
                    _isScroll = true;
                });
                return;
            }

            if (idCurrentPage == 1)
            {
                _isScroll = false;
                idCurrentPage = 0;
                scrollPages[0].countPage = idPageCount;
                scrollPages[0].SetData();
                scrollPages[0].transform.position = transPages[2].position;

                scrollPages[0].transform.DOMove(transPages[0].position, timeAnimator)
                .SetEase(Ease.Linear);
                scrollPages[1].transform.DOMove(transPages[1].position, timeAnimator)
                .SetEase(Ease.Linear).OnComplete(() => {
                    _isScroll = true;
                });
                return;
            }
        }



        [Header("Swipe Settings")]
        [SerializeField] private Collider2D swipeZone; // Vùng swipe dùng Collider2D
        [SerializeField] private float swipeThreshold = 0.1f; // Độ dài tối thiểu để tính là swipe

        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

                // Nếu không nằm trong vùng chỉ định thì bỏ qua
                if (!IsInSwipeZone(worldTouchPos)) return;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startTouchPosition = worldTouchPos;
                        break;

                    case TouchPhase.Ended:
                        _endTouchPosition = worldTouchPos;
                        DetectSwipeDirection();
                        break;
                }
            }
        }

        private bool IsInSwipeZone(Vector2 worldPosition)
        {
            if (swipeZone == null) return true; // Nếu không set vùng thì cho phép toàn màn hình
            return swipeZone.OverlapPoint(worldPosition);
        }

        private void DetectSwipeDirection()
        {
            Vector2 delta = _endTouchPosition - _startTouchPosition;

            // Vuốt ngang
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) && Mathf.Abs(delta.x) > swipeThreshold)
            {
                if (delta.x > 0)
                {
                    OnSwipeRight();
                }
                else
                {
                    OnSwipeLeft();
                }
            }
        }

        private void OnSwipeLeft()
        {
            NextPage();
            // TODO: Thêm logic xử lý vuốt trái tại đây
        }

        private void OnSwipeRight()
        {
            UndoPage();
            // TODO: Thêm logic xử lý vuốt phải tại đây
        }
    }
}
