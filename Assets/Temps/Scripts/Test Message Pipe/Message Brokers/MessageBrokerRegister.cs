using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;

namespace TestMessagePipe
{
    public class MessageBrokerRegister
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BuiltinContainerBuilder _messageBuilder;

        public MessageBrokerRegister()
        {
            _messageBuilder = new();
            _messageBuilder.AddMessagePipe();

            AddMessageBrokers();
            _serviceProvider = _messageBuilder.BuildServiceProvider();
            GlobalMessagePipe.SetProvider(_serviceProvider);
        }

        private void AddMessageBrokers()
        {
            _messageBuilder.AddMessageBroker<TestMessage>();
            _messageBuilder.AddMessageBroker<TestMessage2>();
            _messageBuilder.AddMessageBroker<TestMessage3>();
        }
    }
}
