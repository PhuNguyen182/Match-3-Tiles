using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using Cysharp.Threading.Tasks;
using R3;

namespace TestMessagePipe
{
    public class ExampleClassB : IDisposable
    {
        private readonly string _testValue;
        private readonly ISubscriber<TestMessage> _subscriber;
        private readonly ISubscriber<TestMessage2> _subscriber2;
        private readonly IAsyncSubscriber<TestMessage> asyncSubscriber;

        private IDisposable _disposable;

        public ExampleClassB()
        {
            DisposableBuilder builder = Disposable.CreateBuilder();

            _testValue = "Class B";
            _subscriber = GlobalMessagePipe.GetSubscriber<TestMessage>();
            _subscriber2 = GlobalMessagePipe.GetSubscriber<TestMessage2>();

            _subscriber.Subscribe(x => TestSubscriber(x)).AddTo(ref builder);
            _subscriber2.Subscribe(x => TestSubscriber2(x)).AddTo(ref builder);

            asyncSubscriber?.Subscribe((value, cancellationToken) => TestAsync(value, cancellationToken)).AddTo(ref builder);
            _disposable = builder.Build();
        }

        private async UniTask TestAsync(TestMessage message, CancellationToken cancellationToken)
        {
            await UniTask.CompletedTask;
        }

        private void TestSubscriber(TestMessage message)
        {
            Debug.Log($"Subscriber: {message.Number} {message.Message}");
        }

        private void TestSubscriber2(TestMessage2 message)
        {
            Debug.Log($"Subscriber: {message.Message}");
        }

        public void Dispose()
        {
            Debug.Log($"{_testValue} dispose!");
            _disposable.Dispose();
        }
    }
}
