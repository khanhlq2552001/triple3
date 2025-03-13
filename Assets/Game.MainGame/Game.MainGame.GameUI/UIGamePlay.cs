using UnityEngine;
using UnityEngine.UI;

namespace Game.MainGame
{
    public class UIGamePlay : BlitzyUI.Screen
    {
        [SerializeField] private Button _btnBoosterBack;
        [SerializeField] private Button _btnBoosterLight;
        [SerializeField] private Button _btnBoosterRestart;
        [SerializeField] private Sprite _sprOn;
        [SerializeField] private Sprite _sprOff;
        [SerializeField] private Button _btnPause;
        [SerializeField] private Text _txtCoin;
        [SerializeField] private Animator _animDHC;

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
            _btnBoosterLight.interactable = true;
        }

        public void ExitUI()
        {
            OnPop();
            gameObject.SetActive(false);
        }

        public override void OnSetup()
        {
            GetComponent<Canvas>().overrideSorting = false;
            _btnBoosterBack.onClick.AddListener(()=> BtnBoosterBack());
            _btnBoosterLight.onClick.AddListener(() => BtnBoosterLight());
            _btnBoosterRestart.onClick.AddListener(() => BtnBoosterRestart());
        }

        public void SetActiveBoosterLight(bool isActive)
        {
            _btnBoosterLight.interactable = isActive;

            if (isActive)
            {
                _btnBoosterLight.GetComponent<Image>().sprite = _sprOn;
            }
            else
            {
                _btnBoosterLight.GetComponent<Image>().sprite = _sprOff;
            }
        }

        private void BtnBoosterBack()
        {
            LevelManager.Instance.BoosterBack();
        }

        private void BtnBoosterLight()
        {
            LevelManager.Instance.BoosterLight();
        }

        private void BtnBoosterRestart()
        {
            LevelManager.Instance.BoosterRestart();
        }
    }
}
