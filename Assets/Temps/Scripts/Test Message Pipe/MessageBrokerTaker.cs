using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;
using VContainer.Unity;
using R3;

namespace TestMessagePipe
{
    public class MessageBrokerTaker : MonoBehaviour
    {
        private IDisposable _disposable;

        private ExampleClassA _objectA;
        private ExampleClassB _objectB;

        private IPublisher<TestMessage> _publisher;
        private ISubscriber<TestMessage> _subscriber;
        private IContainerBuilder _container;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _objectA.PublishMessage();
            }
        }

        public void SetBuilder(IContainerBuilder container)
        {
            _container = container;
            var option = _container.RegisterMessagePipe();

            _container.RegisterMessageBroker<TestMessage>(option);
            _container.RegisterEntryPoint<MessageBrokerDemo>(Lifetime.Singleton);
            _container.RegisterBuildCallback(container =>
            {
                _publisher = container.Resolve<IPublisher<TestMessage>>();
                _subscriber = container.Resolve<ISubscriber<TestMessage>>();
                InitMessage();
            });
        }

        public void InitMessage()
        {
            DisposableBuilder disposableBuilder = Disposable.CreateBuilder();

            _objectA = new(_publisher);
            _objectB = new(_subscriber);

            _objectA.AddTo(ref disposableBuilder);
            _objectB.AddTo(ref disposableBuilder);

            _disposable = disposableBuilder.Build();
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}
