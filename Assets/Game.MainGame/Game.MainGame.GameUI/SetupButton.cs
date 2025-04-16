using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public enum ButtonType
    {
        sound,
        music,
        vibra
    }

    public class SetupButton : MonoBehaviour
    {
        private Image _imgButton;

        private void Awake()
        {
            _imgButton = GetComponent<Image>();
        }

        public void CheckSFX(ButtonType type, Sprite[] sprites)
        {
            switch (type)
            {
                case ButtonType.sound:
                    if (Manager.Instance.UserData.soundOn)
                    {
                        _imgButton.sprite = sprites[0];
                    }
                    else
                        _imgButton.sprite = sprites[1];
                    break;
                case ButtonType.music:
                    if (Manager.Instance.UserData.musicOn)
                    {
                        _imgButton.sprite = sprites[0];
                    }
                    else
                        _imgButton.sprite = sprites[1];
                    break;
                case ButtonType.vibra:
                    if (Manager.Instance.UserData.vibrateOn)
                    {
                        _imgButton.sprite = sprites[0];
                    }
                    else
                        _imgButton.sprite = sprites[1];
                    break;
            }
        }
    }
}
