using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.MainGame
{
    public class Manager : Singleton<Manager>
    {
        public static readonly string Name_Scene_Main = "GamePlay";

        public UserData UserData
        {
            get; private set;
        }

        private void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            Game.Launch();
        }

        // Start is called before the first frame update
        private void Start()
        {
            UserData = Game.Data.Load<UserData>();
            SceneManager.LoadSceneAsync(Name_Scene_Main);
        }

        public void CheckFirstData()
        {
            if (!UserData.isFirst)
            {
                UserData.isFirst = true;
                UserData.levelChoose = 0;
                UserData.levelLevel = 1;
                UserData.soundOn = true;
                UserData.musicOn = true;
                UserData.vibrateOn = true;
            }
        }
    }
}
