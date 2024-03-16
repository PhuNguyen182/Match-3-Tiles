using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using R3;
using Random = UnityEngine.Random;

namespace TestReactive
{
    public class TestTImer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;


        private ReactiveProperty<int> reactive = new ReactiveProperty<int>();

        private void Awake()
        {
            BindInt(reactive);
            reactive.Value = 0;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                reactive.Value = Random.Range(0, 100);
            }
        }


        private void BindInt(ReactiveProperty<int> reactiveValue)
        {
            reactiveValue.Pairwise().Subscribe(x => Debug.Log($"{x.Previous} {x.Current}"));
        }

        private Observable<int> CreateIntTween(int from, int to, float duration, Ease ease, Action<int> action)
        {
            return Observable.Create<int>(o =>
            {
                Tween tween = DOVirtual.Int(from, to, duration, o.OnNext).SetEase(ease);
                tween.OnComplete(() =>
                {
                    o.OnNext(to);
                    o.OnCompleted();
                });

                return Disposable.Create(() => tween.Kill());
            });
        }
    }
}
