using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    [CreateAssetMenu(fileName = "DataLevel", menuName = "Data/LevelData")]
    public class DataLevel : ScriptableObject
    {
        public List<Data> listData;
    }
}
