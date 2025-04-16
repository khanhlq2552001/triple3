using UnityEngine;

namespace Game.MainGame
{
    public class UISetting : BlitzyUI.Screen
    {


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
        }

        public override void OnSetup()
        {
        }
    }
}
