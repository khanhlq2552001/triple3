using System.Collections.Generic;
using BlitzyUI;
using Game.Modules.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class UIBuyBooster : BlitzyUI.Screen
    {
        public BoosterType type;
        public Button btnBuy;
        public Image imgBooster;


        [SerializeField] private List<Sprite> _spritesBooster = new List<Sprite>();
        [SerializeField] private int _price;
        [SerializeField] private TextMeshProUGUI _title;

        public override void OnFocus()
        {
        }

        public override void OnFocusLost()
        {
        }

        public override void OnPop()
        {
            PopFinished();
        }

        public override void OnPush(Data data)
        {
            PushFinished();
            EventManager.Raise(new EventPauseGame() { });
        }

        public override void OnSetup()
        {
            btnBuy.onClick.RemoveAllListeners();
            btnBuy.onClick.AddListener(BtnBuy);
        }

        public void SetUpBoosterBuy()
        {
            switch (type)
            {
                case BoosterType.Back:
                    imgBooster.sprite = _spritesBooster[0];
                    _title.text = "NO UNDO";
                    break;
                case BoosterType.Hint:
                    imgBooster.sprite = _spritesBooster[1];
                    _title.text = "NO HINT";
                    break;
                case BoosterType.Replay:
                    imgBooster.sprite = _spritesBooster[2];
                    _title.text = "NO REPLAY";
                    break;
            }
        }

        public void BtnBuy()
        {
            if (Manager.Instance.UserData.playerCoin < _price) return;

            Manager.Instance.UserData.playerCoin -= _price;
            switch (type)
            {
                case BoosterType.Back:
                    Manager.Instance.UserData.quantityBoosterBack++;
                    break;
                case BoosterType.Hint:
                    Manager.Instance.UserData.quantityBoosterHint++;
                    break;
                case BoosterType.Replay:
                    Manager.Instance.UserData.quantityBoosterRestart++;
                    break;
            }
            Close();
            EventManager.Raise(new EventUpdateCoin() { });
            EventManager.Raise(new EventUpdateBooster() { });
        }

        public void Close()
        {
            UIManager.Instance.QueuePop();
            EventManager.Raise(new EventContinuesGame() { });
        }
    }
}
