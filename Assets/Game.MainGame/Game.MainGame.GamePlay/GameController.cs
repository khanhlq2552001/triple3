using UnityEngine;

namespace Game.MainGame
{
    public enum StateController
    {
        normal,
        pause
    }

    public class GameController : MonoBehaviour
    {
        private ItemGrid _itemChoose;
        private StateController _stateController;

        private void Update()
        {
            if(_stateController == StateController.pause)
            {
                return;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    // Lấy tất cả các Collider2D tại vị trí chạm
                    Collider2D[] hitColliders = Physics2D.OverlapPointAll(touchPosition);

                    CheckClick(hitColliders);
                }
                else if(touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    // Lấy tất cả các Collider2D tại vị trí chạm
                    Collider2D[] hitColliders = Physics2D.OverlapPointAll(touchPosition);

                    CheckClickEnd(hitColliders);
                    _itemChoose = null;
                }
            }
        }

        public StateController State
        {
            get => _stateController;

            set => _stateController = value;
        }

        private void CheckClick(Collider2D[] hitColliders)
        {
            foreach (Collider2D col in hitColliders)
            {
                ItemGrid item = col.GetComponent<ItemGrid>();

                if (item != null)
                {
                    if(item.State == StateItem.CanMove)
                    {
                        item.ScaleItem();
                        _itemChoose = item;
                    }
                }
            }
        }

        private void CheckClickEnd(Collider2D[] hitColliders)
        {
            foreach (Collider2D col in hitColliders)
            {
                ItemGrid item = col.GetComponent<ItemGrid>();

                if (item != null)
                {
                    if (item == _itemChoose)
                    {
                        LevelManager.Instance.slotUnder.SetItemUnder(item.ID, item);
                        return;
                    }
                }
            }

            if(_itemChoose != null)
            {
                _itemChoose.ScaleNormalItem(true);
            }
        }
    }
}
