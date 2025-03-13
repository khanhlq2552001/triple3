using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    [CreateAssetMenu(fileName = "ThemeDatas", menuName = "Theme/ThemeData")]
    public class DataSpriteThem : ScriptableObject
    {
        public List<Sprites> spritesThemes;
    }

    [System.Serializable]
    public class Sprites
    {
        public List<Sprite> listSpriteTheme;
    }
}
