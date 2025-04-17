using BlitzyUI;
using Game.Modules.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class UIHome : BlitzyUI.Screen
    {
        [SerializeField] private Text _txtLevel;
        [SerializeField] private Text _txtCoin;
        [SerializeField] private Transform _tranParent;
        [SerializeField] private Button _btnPlay;
        [SerializeField] private ItemLevel _itemLevel;

        private ItemLevel _itemChoose;

        public PageView pageView;

        public override void OnFocus()
        {
        }

        public override void OnFocusLost()
        {
        }


        public override void OnPop()
        {
            PopFinished();
            GameManager.Instance.onUpdateLevelChoose -= UpdateTextLevelChoose;
            EventManager.UnsubscribeFrom<EventUpdateCoin>(OnUpdateCoin);
        }

        public override void OnPush(Data data)
        {
            PushFinished();
            //  CreateScroll();
            pageView.SetPage();
            UpdateTextLevelChoose();
            UpdateCoin();

            LevelManager.Instance.slotUnder.gameObject.SetActive(false);
            GameManager.Instance.onUpdateLevelChoose += UpdateTextLevelChoose;
            EventManager.SubscribeTo<EventUpdateCoin>(OnUpdateCoin);
        }

        private void OnUpdateCoin(ref EventUpdateCoin eve)
        {
            UpdateCoin();
        }

        public override void OnSetup()
        {
            _btnPlay.onClick.AddListener(() => BtnPlay());
        }

        public void SetItemChoose(ItemLevel choose)
        {
            _itemChoose = choose;
        }

        public void UpdateCoin()
        {
            _txtCoin.text = Manager.Instance.UserData.playerCoin.ToString();
        }

        public void BtnPlay()
        {
            UIManager.Instance.QueuePop(null);

            UIGamePlay ui = UIManager.Instance.GetScreen<UIGamePlay>(GameManager.ScreenId_UIGamePlay);
            if(ui != null)
            {
                ui.gameObject.SetActive(true);
            }

            UIManager.Instance.QueuePush(GameManager.ScreenId_UIGamePlay, null, "UIGamePlay", null);
            LevelManager.Instance.GenerateData();
        }

        public void UpdateTextLevelChoose()
        {
            _txtLevel.text = "Level " + Manager.Instance.UserData.levelChoose;
        }

        public void SetItemChoose(ItemLevel item,int levelC)
        {
            GameManager.Instance.SetLevelChoose(levelC);
            int level = Manager.Instance.UserData.levelLevel;

            if(_itemChoose.level < level)
            {
                _itemChoose.SetType(1, _itemChoose.level);
            }
            if(_itemChoose.level == level)
            {
                _itemChoose.SetType(3, _itemChoose.level);
            }

            _itemChoose = item;
        }

        public void BtnMenu()
        {
            UIManager.Instance.QueuePush(GameManager.ScreenUi_Setting, null, "UiSetting", null);

            UISetting ui = UIManager.Instance.GetScreen<UISetting>(GameManager.ScreenUi_Setting);
            ui.ShowMenu();
        }

    }
}
