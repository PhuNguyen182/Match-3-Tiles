using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestHexagonGrid
{
    [Serializable]
    public enum HexagonType
    {
        FlatTop,
        PointedTop
    }

    public static class CoordinateConverter
    {
        public static Vector3 FromCubeToHexagon(Vector3Int position, float size, HexagonType hexagonType)
        {
            bool shouldOffset;
            float x, y, offset;
            float witdh, height;
            float horizontalDistance, verticalDistance;

            Vector3 hexPosition = Vector3.zero;

            if (hexagonType == HexagonType.PointedTop)
            {
                shouldOffset = position.y % 2 == 0;
                witdh = Mathf.Sqrt(3f) * size;
                height = size * 2f;

                horizontalDistance = witdh;
                verticalDistance = height * 0.75f;
                offset = shouldOffset ? witdh / 2f : 0f;

                x = (horizontalDistance * position.x) + offset;
                y = verticalDistance * position.y;

                hexPosition = new Vector3(x, -y);
            }

            else if(hexagonType == HexagonType.FlatTop)
            {
                shouldOffset = position.x % 2 == 0;
                witdh = size * 2f;
                height = Mathf.Sqrt(3f) * size;

                horizontalDistance = witdh * 0.75f;
                verticalDistance = height;
                offset = shouldOffset ? height / 2f : 0f;

                x = horizontalDistance * position.x;
                y = (verticalDistance * position.y) - offset;

                hexPosition = new Vector3(x, -y);
            }

            return hexPosition;
        }
    }
}
