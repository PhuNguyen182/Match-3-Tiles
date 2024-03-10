using Match3Tiles.Scripts.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles
{
    public class GameInput : MonoBehaviour
    {
        [SerializeField] private LayerMask tileMask;

        private Camera _mainCamera;
        private Collider2D _tileCollider;

        public bool IsLocked { get; set; }
        public Action<IMatchTile> OnGetMatchBlock;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                ProcessInput(position);
            }
        }

        private void ProcessInput(Vector3 position)
        {
            _tileCollider = Physics2D.OverlapPoint(position, tileMask);
            if(_tileCollider != null)
            {
                if(_tileCollider.TryGetComponent(out IMatchTile tileBlock))
                {
                    OnGetMatchBlock?.Invoke(tileBlock);
                }
            }
        }
    }
}
