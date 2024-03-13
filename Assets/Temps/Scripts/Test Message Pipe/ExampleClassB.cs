using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using R3;

namespace TestMessagePipe
{
    public class ExampleClassB : IDisposable
    {
        private readonly string _testValue;
        private readonly ISubscriber<TestMessage> _subscriber;

        private IDisposable _disposable;

        public ExampleClassB(ISubscriber<TestMessage> subscriber)
        {
            var bag = Disposable.CreateBuilder();

            _testValue = "Class B";
            _subscriber = subscriber;
            
            _subscriber.Subscribe(TestSubscriber).AddTo(ref bag);
            _disposable = bag.Build();
        }

        private void TestSubscriber(TestMessage message)
        {
            Debug.Log($"Subscriber: {message.Number} {message.Message}");
        }

        public void Dispose()
        {
            Debug.Log($"{_testValue} dispose!");
            _disposable.Dispose();
        }
    }
}
