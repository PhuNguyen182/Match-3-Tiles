using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;
using R3;

namespace TestMessagePipe
{
    public class MessageManager : MonoBehaviour
    {
        private ExampleClassA _publisher;
        private ExampleClassB _subscriber;
        private MessageBrokerRegister _messageBroker;

        private IDisposable _disposable;

        private void Awake()
        {
            var builder = Disposable.CreateBuilder();
            _messageBroker = new();

            _publisher = new();
            _publisher.AddTo(ref builder);

            _subscriber = new();
            _subscriber.AddTo(ref builder);

            _disposable = builder.Build();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _publisher.PublishMessage();
            }
        }

        public void SetContainer(IContainerBuilder container, MessagePipeOptions options)
        {
            
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}
