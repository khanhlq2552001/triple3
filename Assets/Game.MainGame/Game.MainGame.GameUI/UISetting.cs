using BlitzyUI;
using Game.Modules.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class UISetting : BlitzyUI.Screen
    {
        public Sprite[] sprStatusSound;
        public Sprite[] sprStatusMusic;
        public Sprite[] sprStatusVibra;
        public Button btnSound;
        public Button btnMusic;
        public Button btnVibra;

      //  public Button btnShop;
     //   public Button btnRateUs;
     //   public Button btnRestore;
     //   public Button btnRemoveAds;
        public Button btnLevel;
        public Button btnReplay;

        public TextMeshProUGUI txtTitle;

        private enum ModeSetting
        {
            menu,
            pause
        }

        private ModeSetting _setting = ModeSetting.menu;

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
            btnSound.GetComponent<SetupButton>().CheckSFX(ButtonType.sound, sprStatusSound);
            btnMusic.GetComponent<SetupButton>().CheckSFX(ButtonType.music, sprStatusMusic);
            btnVibra.GetComponent<SetupButton>().CheckSFX(ButtonType.vibra, sprStatusVibra);
        }

        public void SetUpBtn(int id)
        {

            switch (id)
            {
                case 0:
                    if (Manager.Instance.UserData.soundOn)
                    {
                        Manager.Instance.UserData.soundOn = false;
                    }
                    else
                    {
                        Manager.Instance.UserData.soundOn = true;
                    }
                    btnSound.GetComponent<SetupButton>().CheckSFX(ButtonType.sound, sprStatusSound);
                    break;
                case 1 :
                    if (Manager.Instance.UserData.musicOn)
                    {
                        Manager.Instance.UserData.musicOn = false;
                    }
                    else
                    {
                        Manager.Instance.UserData.musicOn = true;
                    }
                    btnMusic.GetComponent<SetupButton>().CheckSFX(ButtonType.music, sprStatusMusic);
                    break;
                case 2:
                    if (Manager.Instance.UserData.vibrateOn)
                    {
                        Manager.Instance.UserData.vibrateOn = false;
                    }
                    else
                    {
                        Manager.Instance.UserData.vibrateOn = true;
                    }
                    btnVibra.GetComponent<SetupButton>().CheckSFX(ButtonType.vibra, sprStatusVibra);
                    break;
                case 3:
                    Close();
                    LevelManager.Instance.Restart();
                    break;
                case 4:
                    // Level
                    UIManager.Instance.QueuePop();

                    LevelManager.Instance.ClearData();
                    UIGamePlay uiGamePlay = UIManager.Instance.GetScreen<UIGamePlay>(GameManager.ScreenId_UIGamePlay);
                    uiGamePlay.ExitUI();
                    UIManager.Instance.QueuePush(GameManager.ScreenID_Home, null, "UIHome", null);
                    break;
            }
            AudioController.instance.CheckMusic();
            AudioController.instance.CheckSound();
        }

        public void Close()
        {
            UIManager.Instance.QueuePop();
            if(_setting == ModeSetting.pause)
            {
                GameManager.Instance.controller.ResumGame();
                EventManager.Raise(new EventContinuesGame { });
            }
        }

        public void ShowPause()
        {
            //btnRateUs.SetActive(false);
            //btnRestore.SetActive(false);
            //btnShop.SetActive(false);

            //btnRemoveAds.SetActive(true);
            btnLevel.SetActive(true);
            btnReplay.SetActive(true);
            txtTitle.text = "PAUSE";
            _setting = ModeSetting.pause;
            GameManager.Instance.controller.PauseGame();
            EventManager.Raise(new EventPauseGame { });
        }

        public void ShowMenu()
        {
            //btnRateUs.SetActive(true);
            //btnRestore.SetActive(true);
            //btnShop.SetActive(true);

            //btnRemoveAds.SetActive(false);
            btnLevel.SetActive(false);
            btnReplay.SetActive(false);
            _setting = ModeSetting.menu;
            txtTitle.text = "MENU";
        }
    }
}
