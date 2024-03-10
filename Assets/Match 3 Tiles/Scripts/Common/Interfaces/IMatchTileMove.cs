using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Match3Tiles.Scripts.Common.Interfaces
{
    public interface IMatchTileMove
    {
        public UniTask MoveTo(Vector3 position);
    }
}
