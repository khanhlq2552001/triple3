using BlitzyUI;
using UnityEngine;

namespace Game.MainGame
{
    public class GameManager : MonoBehaviour
    {
        public static readonly BlitzyUI.Screen.Id ScreenId_UIGamePlay = new BlitzyUI.Screen.Id("UIGamePlay");

        private void Start()
        {
            UIManager.Instance.QueuePush(ScreenId_UIGamePlay, null, "UIGamePlay", null);
        }
    }
}
