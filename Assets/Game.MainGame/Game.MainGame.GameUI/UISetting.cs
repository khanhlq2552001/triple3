using BlitzyUI;
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
            }
        }

        public void Close()
        {
            UIManager.Instance.QueuePop();
        }
    }
}
