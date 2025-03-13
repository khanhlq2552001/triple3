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
                SetUpData();
            }
        }

        private void Start()
        {
            SetLevelChoose(PlayerPrefs.GetInt("levelChoose"));
            UIManager.Instance.QueuePush(ScreenID_Home, null, "UIHome", null);
        }

        private void Update()
        {
            onActionUpdate?.Invoke();
        }

        public void SetLevel(int Level)
        {
            PlayerPrefs.SetInt("level", Level);
            onActionUpdate?.Invoke();
        }

        public void SetLevelChoose(int level)
        {
            PlayerPrefs.SetInt("levelChoose", level);
            onUpdateLevelChoose?.Invoke();
            LevelManager.Instance.SetLevel(level);
        }

        private void SetUpData()
        {
            if (!PlayerPrefs.HasKey("isFirst"))
            {
                PlayerPrefs.SetInt("isFirst", 1);
                PlayerPrefs.SetInt("coin", 0);
                PlayerPrefs.SetInt("level", 1);
                PlayerPrefs.SetInt("levelChoose", 0);
            }
        }
    }
}
