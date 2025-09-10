using System.Collections.Generic;
using Items.Data;
using UnityEngine;

namespace Utils
{
    public static class Util
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        public static bool IsSameItem(ItemData a, ItemData b)
        {
            return a.ItemSo == b.ItemSo && a.LevelSo == b.LevelSo;
        }
    }
}
