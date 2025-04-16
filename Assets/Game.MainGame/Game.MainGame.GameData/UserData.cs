using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    [Serializable]
    public class UserData : SavePlayerPrefs
    {
        public string lastTimePlayGame = string.Empty;
        public bool isFirst = false;
        public int incomePerMin;
        public int playerCash;
        public int playerDiamond;
        public int currentMap;
        public bool hasUnlockMap;
        public bool soundOn;
        public bool musicOn;
        public bool vibrateOn;
        public bool removeAds;
        public bool greatdeal;
        public bool premiumPackBought;
        public bool hasUseFreeTrial;
        public bool isResetQuest;
        public bool firstOpenApp;//flagAppOpenADS
        public bool firstOpenAppShop;//flag show premiumpack
        public bool firstLoading;//flagFirstLoading
        public bool firstMoveCamOnOpenApp;
        public bool isOpenTutorailToilet;
        public bool isOpenTutorailThermometer;
        public DataTrackingFirebase dataTrackingFirebase;

        public int levelChoose;
        public int levelLevel;

        public bool isX2MoneyIap;
        public float timeX2Iap;
        public int countResetX2Money;


        public bool isCollectCashIap;
        public float timeCollectCash;
        public int countResetCollectCash;

        public bool isVehiIap;
        public float timeVehi;



    }



    [Serializable]
    public class DataTrackingFirebase
    {
        public string timeStartReward = string.Empty;
        public string timeStartIntern = string.Empty;
        public string timeStartAppOpen = string.Empty;
        public string timeCanShowOpenADS = DateTime.Now.ToString();
        public List<int> trackingTimeIncrementalProgressGame = new();

        public int currentDataMapIndex = 0;
        public List<DataSession> dataMaps =
    new List<DataSession> { new DataSession(), new DataSession(), new DataSession(), new DataSession(), new DataSession(), new DataSession() };

        public int currentDataSession = 0;
        public List<DataSession> dataSessionS =
   new List<DataSession> { new DataSession(), new DataSession(), new DataSession(), new DataSession(), new DataSession()};
    }

    [Serializable]
    public class DataSession
    {
        public float minutes;
        public bool completed;
        public bool status;

        public DataSession(float minutes = 0, bool completed = false, bool status = false)
        {
            this.minutes = minutes;
            this.completed = completed;
            this.status = status;
        }
    }
}
