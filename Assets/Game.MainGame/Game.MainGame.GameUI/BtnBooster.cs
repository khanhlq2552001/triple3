using Game.Modules.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public enum BoosterType
    {
        Back,
        Hint,
        Replay
    }

    public class BtnBooster : MonoBehaviour
    {
        public BoosterType boosterType;

        private TextMeshProUGUI _txtCount;

        private void Awake()
        {
            _txtCount = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            CheckCount();
            EventManager.SubscribeTo<EventUpdateBooster>(OnUpdateBooster);
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeFrom<EventUpdateBooster>(OnUpdateBooster);
        }

        private void OnUpdateBooster(ref EventUpdateBooster eve)
        {
            CheckCount();
        }

        public void CheckCount()
        {
            switch (boosterType)
            {
                case BoosterType.Back:
                    if (Manager.Instance.UserData.quantityBoosterBack == 0)
                        _txtCount.text = "+";
                    else _txtCount.text = Manager.Instance.UserData.quantityBoosterBack.ToString();
                    break;
                case BoosterType.Hint:
                    if (Manager.Instance.UserData.quantityBoosterHint == 0)
                        _txtCount.text = "+";
                    else _txtCount.text = Manager.Instance.UserData.quantityBoosterHint.ToString();
                    break;
                case BoosterType.Replay:
                    if (Manager.Instance.UserData.quantityBoosterRestart == 0)
                        _txtCount.text = "+";
                    else _txtCount.text = Manager.Instance.UserData.quantityBoosterRestart.ToString();
                    break;
            }
        }
    }
}
