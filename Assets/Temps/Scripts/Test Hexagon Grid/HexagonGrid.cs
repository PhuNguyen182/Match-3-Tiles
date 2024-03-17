using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestHexagonGrid
{
    public class HexagonGrid : MonoBehaviour
    {
        [SerializeField] private float size = 1;
        [SerializeField] private HexagonType hexagonType;
        [SerializeField] private GameObject gridCell;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2Int offset;

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    Vector3Int pos = new Vector3Int(i + offset.x, j + offset.y);
                    Vector3 hexPos = CoordinateConverter.FromCubeToHexagon(pos, size, hexagonType);
                    Instantiate(gridCell, hexPos, Quaternion.identity);
                }
            }
        }
    }
}
