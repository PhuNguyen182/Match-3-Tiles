using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3Tiles.Scripts.Utils
{
    public static class CollectionUtils
    {
        public static void Shuffle<T>(this List<T> list)
        {
            if (list.Count < 2)
                return;

            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        public static List<T> GetShuffle<T>(this List<T> list)
        {
            List<T> copiedList = new(list);
            copiedList.Shuffle();
            
            return copiedList;
        }

        public static int GetMappedIndex<T>(this List<T> list, int checkIndex, List<T> shuffled)
        {
            return (checkIndex >= 0 && checkIndex < list.Count) ? shuffled.IndexOf(list[checkIndex]) : -1;
        }
    }
}
