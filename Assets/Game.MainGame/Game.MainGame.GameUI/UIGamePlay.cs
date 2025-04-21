using System.Collections;
using BlitzyUI;
using Cysharp.Threading.Tasks;
using Game.Modules.Events;
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
   //     [SerializeField] private Animator _animDHC;
        [SerializeField] private Text _txtLevel;

        private Coroutine _timeCoroutine;

        public override void OnFocus()
        {
        }

        public override void OnFocusLost()
        {
        }

        public override void OnPop()
        {
            PopFinished();
            EventManager.UnsubscribeFrom<EventUpdateLevelChoose>(OnUpdateLevelChoose);
            EventManager.UnsubscribeFrom<EventPauseGame>(OnPauseTime);
            EventManager.UnsubscribeFrom<EventContinuesGame>(OnContinuesGame);
            EventManager.UnsubscribeFrom<EventLoseGame>(OnStopTimeLose);
            EventManager.UnsubscribeFrom<EventWinGame>(OnStopTimeWin);
            EventManager.UnsubscribeFrom<EventUpdateCoin>(OnUpdateCoin);
        }

        public override void OnPush(Data data)
        {
            PushFinished();
            _btnBoosterLight.interactable = true;
            _txtLevel.text = "Level " + Manager.Instance.UserData.levelChoose;
            UpdateCoin();
            EventManager.SubscribeTo<EventUpdateLevelChoose>(OnUpdateLevelChoose);
            EventManager.SubscribeTo<EventPauseGame>(OnPauseTime);
            EventManager.SubscribeTo<EventContinuesGame>(OnContinuesGame);
            EventManager.SubscribeTo<EventLoseGame>(OnStopTimeLose);
            EventManager.SubscribeTo<EventWinGame>(OnStopTimeWin);
            EventManager.SubscribeTo<EventUpdateCoin>(OnUpdateCoin);
        }

        private void OnUpdateLevelChoose(ref EventUpdateLevelChoose eve)
        {
            _txtLevel.text = "Level " + Manager.Instance.UserData.levelChoose;
        }

        private void OnPauseTime(ref EventPauseGame eve)
        {
            PauseTimeCountDown();
        }

        private void OnContinuesGame(ref EventContinuesGame eve)
        {
            ContinuesCountDown();
        }

        private void OnStopTimeLose(ref EventLoseGame eve)
        {
            StopCountDown();
        }

        private void OnStopTimeWin(ref EventWinGame eve)
        {
            StopCountDown();
        }

        public void ExitUI()
        {
            OnPop();
            UIManager.Instance.ForceRemoveScreen(GameManager.ScreenId_UIGamePlay);
        }

        public override void OnSetup()
        {
            GetComponent<Canvas>().overrideSorting = false;
            _btnBoosterBack.onClick.AddListener(() => BtnBoosterBack());
            _btnBoosterLight.onClick.AddListener(() => BtnBoosterLight());
            _btnBoosterRestart.onClick.AddListener(() => BtnBoosterRestart());
            _btnPause.onClick.AddListener(() => BtnPause());
        }

        public void UpdateCoin()
        {
            _txtCoin.text = Manager.Instance.UserData.playerCoin.ToString();
        }
        private void OnUpdateCoin(ref EventUpdateCoin eve)
        {
            UpdateCoin();
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

        private void BtnPause()
        {
            UIManager.Instance.QueuePush(GameManager.ScreenUi_Setting, null, "UiSetting", null);

            UISetting ui = UIManager.Instance.GetScreen<UISetting>(GameManager.ScreenUi_Setting);
            ui.ShowPause();
        }

        private float _timeCurrent = 0;
        private float _startTime = 0;
        public Image imgProgress;

        public void StartCountDown(int time)
        {
            imgProgress.fillAmount = 1;
            StopCountDown();
          //  _animDHC.speed = speed;
            _timeCurrent = time;
            _startTime = time;

         //   _animDHC.SetBool("startTime", true);
          //  _animDHC.Play("startTime", 0, 0f);
            _timeCoroutine = StartCoroutine(CountTimeCoroutine());
        }

        public void ContinuesCountDown()
        {
          //  _animDHC.speed = _currentSpeed;
            _timeCoroutine = StartCoroutine(CountTimeCoroutine());
        }

        public void PauseTimeCountDown()
        {
         //   _animDHC.speed = 0;
            if (_timeCoroutine != null)
            {
                StopCoroutine(_timeCoroutine);
                _timeCoroutine = null;
            }
        }

        public void StopCountDown()
        {
        //    _animDHC.SetBool("startTime", false);

            if(_timeCoroutine != null)
            {
                StopCoroutine(_timeCoroutine);
                _timeCoroutine = null;
            }
        }

        IEnumerator CountTimeCoroutine()
        {
            while(_timeCurrent > 0)
            {
                yield return new WaitForSeconds(1f);
                _timeCurrent--;
                float value = (float)_timeCurrent / _startTime;
                imgProgress.fillAmount = value;
            }
            LevelManager.Instance.controller.State = StateController.pause;
            yield return new WaitForSeconds(0.5f);
            UIManager.Instance.QueuePush(GameManager.ScreenID_UILose, null, "UILose", null);
        }
    }
}
