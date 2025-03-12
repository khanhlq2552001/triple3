using System;
using BlitzyUI;
using UnityEngine;

namespace Game.MainGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static readonly BlitzyUI.Screen.Id ScreenId_UIGamePlay = new BlitzyUI.Screen.Id("UIGamePlay");

        public GameObject fxSmoke;
        public Action onActionUpdate;
        public GameObject trail;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            UIManager.Instance.QueuePush(ScreenId_UIGamePlay, null, "UIGamePlay", null);
        }

        private void Update()
        {
            onActionUpdate?.Invoke();
        }
    }
}
