using System;
using BlitzyUI;
using UnityEngine;

namespace Game.MainGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static readonly BlitzyUI.Screen.Id ScreenId_UIGamePlay = new BlitzyUI.Screen.Id("UIGamePlay");
        public static readonly BlitzyUI.Screen.Id ScreenID_UIWin = new BlitzyUI.Screen.Id("UIWin");
        public static readonly BlitzyUI.Screen.Id ScreenID_UILose = new BlitzyUI.Screen.Id("UILose");
        public static readonly BlitzyUI.Screen.Id ScreenID_Home = new BlitzyUI.Screen.Id("UIHome");
        public static readonly BlitzyUI.Screen.Id ScreenUi_Setting = new BlitzyUI.Screen.Id("UiSetting");

        public GameObject fxSmoke;
        public Action onActionUpdate;
        public GameObject trail;
        public Action onUpdateLevel;
        public Action onUpdateLevelChoose;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            if(Instance == null)
            {
                Instance = this;
           //     SetUpData();
            }
        }

        private void Start()
        {
            SetLevelChoose(Manager.Instance.UserData.levelChoose);
            UIManager.Instance.QueuePush(ScreenID_Home, null, "UIHome", null);
        }

        private void Update()
        {
            onActionUpdate?.Invoke();
        }

        public void SetLevel(int Level)
        {
            Manager.Instance.UserData.levelLevel = Level;
            onActionUpdate?.Invoke();
        }

        public void SetLevelChoose(int level)
        {
            Manager.Instance.UserData.levelChoose = level;
            onUpdateLevelChoose?.Invoke();
            LevelManager.Instance.SetLevel(level);
        }
    }
}
