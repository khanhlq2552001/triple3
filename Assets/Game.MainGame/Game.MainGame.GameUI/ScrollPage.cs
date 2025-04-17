using System.Collections.Generic;
using BlitzyUI;
using UnityEngine;

namespace Game.MainGame
{
    public class ScrollPage : MonoBehaviour
    {
        public List< ItemLevel> itemLevels;
        public int idPage;
        public int countPage;
        public PageView pageView;

        public void SetData()
        {
            int levelMax = LevelManager.Instance.GetDatas().listData.Count;
            int level = Manager.Instance.UserData.levelLevel;
            int levelChoose = Manager.Instance.UserData.levelChoose;

            for (int i = 0; i < 12; i++)
            {


                int idx = i + 1 + countPage * 12;
                itemLevels[i].level = idx;

                if (idx < level)
                {
                    itemLevels[i].SetType(1, idx);
                }
                else if (idx == level)
                {
                    itemLevels[i].SetType(3, idx);
                }
                else
                {
                    itemLevels[i].SetType(0, idx);
                }

                if (idx == levelChoose)
                {
                    itemLevels[i].SetType(2, idx);
                    UIHome ui = UIManager.Instance.GetScreen<UIHome>(GameManager.ScreenID_Home);
                    ui.SetItemChoose(itemLevels[i]);
                }
            }
        }
    }
}
