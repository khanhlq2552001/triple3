using BlitzyUI;
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
        }

        public override void OnPush(Data data)
        {
            PushFinished();
        }

        public override void OnSetup()
        {
            GetComponent<Canvas>().overrideSorting = false;
            _btnRewardAds.onClick.AddListener(() => BtnRewardAds());
            _btnReward.onClick.AddListener(() => BtnReward());
            _btnHome.onClick.AddListener(() => BtnHome());
        }

        public void BtnRewardAds()
        {

        }

        public void BtnReward()
        {
            int level = PlayerPrefs.GetInt("level");
            int levelChoose = PlayerPrefs.GetInt("levelChoose");

            if(levelChoose == level)
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

            UIManager.Instance.QueuePop();
            LevelManager.Instance.SetLevel(PlayerPrefs.GetInt("levelChoose"));
            LevelManager.Instance.GenerateData();
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
