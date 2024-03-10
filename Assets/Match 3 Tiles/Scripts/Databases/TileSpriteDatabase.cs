using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Match3Tiles.Scripts.Databases
{
    [CreateAssetMenu(fileName = "Tile Sprite Database", menuName = "Scriptable Objects/Databases/Tile Sprite Database")]
    public class TileSpriteDatabase : ScriptableObject
    {
        [SerializeField] private Sprite[] tileSprites;

        public Sprite GetSpriteByIndex(int index)
        {
            return tileSprites[index];
        }
    }
}
