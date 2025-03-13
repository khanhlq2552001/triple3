using BlitzyUI;
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
        }

        public override void OnPush(Data data)
        {
            PushFinished();
            CreateScroll();
            UpdateTextLevelChoose();

            LevelManager.Instance.slotUnder.gameObject.SetActive(false);
            GameManager.Instance.onUpdateLevelChoose += UpdateTextLevelChoose;
        }

        public override void OnSetup()
        {
            _btnPlay.onClick.AddListener(() => BtnPlay());
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
            _txtLevel.text = "Level " + PlayerPrefs.GetInt("levelChoose");
        }

        public void SetItemChoose(ItemLevel item,int levelC)
        {
            GameManager.Instance.SetLevelChoose(levelC);
            int level = PlayerPrefs.GetInt("level");

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

        public void CreateScroll()
        {
            for(int i= _tranParent.childCount -1; i >= 0; i--)
            {
                Destroy(_tranParent.GetChild(i).gameObject);
            }

            int levelMax = LevelManager.Instance.GetDatas().listData.Count;
            int level = PlayerPrefs.GetInt("level");
            int levelChoose = PlayerPrefs.GetInt("levelChoose");

            for(int i=0; i< 12; i++)
            {
                int idx = i;
                ItemLevel item = Instantiate(_itemLevel, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_tranParent);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;

                if(i < level)
                {
                    item.SetType(1, idx);
                }
                else if (i == level)
                {
                    item.SetType(3, idx);
                }
                else
                {
                    item.SetType(0, idx);
                }

                if(i == levelChoose)
                {
                    item.SetType(2, idx);
                    _itemChoose = item;
                }
            }
        }
    }
}
