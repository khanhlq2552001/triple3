using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MainGame
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}
