using BlitzyUI;
using Game.Modules.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class UIWin : BlitzyUI.Screen
    {
        [SerializeField] private Text _txtCoin;
        [SerializeField] private Text _txtCoinReward;
        [SerializeField] private Button _btnRewardAds;
        [SerializeField] private Button _btnReward;
        [SerializeField] private Button _btnHome;

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
            UpdateCoin();
            EventManager.SubscribeTo<EventUpdateCoin>(OnUpdateCoin);

            int level = Manager.Instance.UserData.levelLevel;
            int levelChoose = Manager.Instance.UserData.levelChoose;

            if (levelChoose == level)
            {
                levelChoose++;
                GameManager.Instance.SetLevel(levelChoose);
                GameManager.Instance.SetLevelChoose(levelChoose);
            }
            else
            {
                levelChoose++;
                GameManager.Instance.SetLevelChoose(levelChoose);
            }

            EventManager.Raise(new EventWinGame() { });
        }

        public override void OnSetup()
        {
            GetComponent<Canvas>().overrideSorting = false;
            _btnRewardAds.onClick.AddListener(() => BtnRewardAds());
            _btnReward.onClick.AddListener(() => BtnReward());
            _btnHome.onClick.AddListener(() => BtnHome());
        }

        public void UpdateCoin()
        {
            _txtCoin.text = Manager.Instance.UserData.playerCoin.ToString();
        }
        private void OnUpdateCoin(ref EventUpdateCoin eve)
        {
            UpdateCoin();
        }

        public void BtnRewardAds()
        {
            int level = Manager.Instance.UserData.levelLevel;
            int levelChoose = Manager.Instance.UserData.levelChoose;

            Manager.Instance.UserData.playerCoin += 250;
            UIManager.Instance.QueuePop();
            LevelManager.Instance.SetLevel(Manager.Instance.UserData.levelChoose);
            LevelManager.Instance.GenerateData();
            EventManager.Raise(new EventUpdateCoin() { });

            EventManager.Raise(new EventUpdateLevelChoose {

            });


        }

        public void BtnReward()
        {
            int level = Manager.Instance.UserData.levelLevel;
            int levelChoose = Manager.Instance.UserData.levelChoose;

            UIManager.Instance.QueuePop();
            LevelManager.Instance.SetLevel(Manager.Instance.UserData.levelChoose);
            LevelManager.Instance.GenerateData();
            EventManager.Raise(new EventUpdateLevelChoose {

            });
        }

        public void BtnHome()
        {
            UIManager.Instance.QueuePop();

            UIGamePlay uiGamePlay = UIManager.Instance.GetScreen<UIGamePlay>(GameManager.ScreenId_UIGamePlay);
            uiGamePlay.ExitUI();
            UIManager.Instance.QueuePush(GameManager.ScreenID_Home, null, "UIHome", null);
        }
    }
}
