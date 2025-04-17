using BlitzyUI;
using Game.Modules.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class UILose : BlitzyUI.Screen
    {
        [SerializeField] private Text _txtCoin;
        [SerializeField] private Button _btnRevive;
        [SerializeField] private Button _btnRestart;

        public override void OnFocus()
        {
        }

        public override void OnFocusLost()
        {
        }

        public override void OnPop()
        {
            PopFinished();
            EventManager.UnsubscribeFrom<EventUpdateCoin>(OnUpdateCoin);
        }

        public override void OnPush(Data data)
        {
            PushFinished();
            EventManager.Raise(new EventLoseGame() { });
            UpdateCoin();
            EventManager.SubscribeTo<EventUpdateCoin>(OnUpdateCoin);
        }

        public override void OnSetup()
        {
            GetComponent<Canvas>().overrideSorting = false;
            _btnRevive.onClick.AddListener(() => BtnRevive());
            _btnRestart.onClick.AddListener(() => BtnReplay());
        }

        public void UpdateCoin()
        {
            _txtCoin.text = Manager.Instance.UserData.playerCoin.ToString();
        }

        private void OnUpdateCoin(ref EventUpdateCoin eve)
        {
            UpdateCoin();
        }

        public void BtnRevive()
        {

        }

        public void BtnReplay()
        {
            UIManager.Instance.QueuePop(null);
            LevelManager.Instance.BoosterRestart();
        }
    }
}
