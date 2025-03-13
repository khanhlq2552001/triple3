using BlitzyUI;
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
        }

        public override void OnPush(Data data)
        {
            PushFinished();
        }

        public override void OnSetup()
        {
            GetComponent<Canvas>().overrideSorting = false;
            _btnRevive.onClick.AddListener(() => BtnRevive());
            _btnRestart.onClick.AddListener(() => BtnReplay());
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
