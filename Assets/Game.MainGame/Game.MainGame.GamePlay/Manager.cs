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
            Game.Launch();
        }

        // Start is called before the first frame update
        private void Start()
        {
            UserData = Game.Data.Load<UserData>();
            SceneManager.LoadSceneAsync(Name_Scene_Main);
        }

    }
}
