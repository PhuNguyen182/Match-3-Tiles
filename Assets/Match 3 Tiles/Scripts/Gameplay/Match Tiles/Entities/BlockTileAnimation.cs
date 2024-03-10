using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Entities
{
    public class BlockTileAnimation : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveDuration = 0.25f;
        [SerializeField] private Ease moveEase = Ease.OutQuad;

        private Tweener _moveTween;
        private CancellationToken _token;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        public async UniTask MoveTo(Vector3 position)
        {
            // Important!!! All mathods with .ToUniTask().Forget() here can be awaitable.
            // Await them multiple times can cause stack overflow, rendering to crash game
            _moveTween ??= CreateMoveTween(position);
            _moveTween.ChangeStartValue(transform.position).ToUniTask().Forget();
            _moveTween.ChangeEndValue(position).ToUniTask().Forget();

            _moveTween.Rewind();
            _moveTween.Play().ToUniTask().Forget();

            // To be safe, wait for exactly the amount of time of the tween or tweener
            await UniTask.Delay(TimeSpan.FromSeconds(_moveTween.Duration()), cancellationToken: _token);
            if (_token.IsCancellationRequested)
                return;
        }

        private Tweener CreateMoveTween(Vector3 toPosition)
        {
            return transform.DOMove(toPosition, moveDuration).SetEase(moveEase).SetAutoKill(false);
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
        }
    }
}
