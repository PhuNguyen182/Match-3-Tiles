using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace TestMessagePipe
{
    public class MessagePipeLifeTime : LifetimeScope
    {
        [SerializeField] private MessageBrokerTaker taker;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(container =>
            {
                GlobalMessagePipe.SetProvider(container.AsServiceProvider());
            });

            taker.SetBuilder(builder);
        }
    }

    public class MessageBrokerDemo : IStartable
    {
        public IPublisher<TestMessage> Publisher { get; }
        public ISubscriber<TestMessage> Subscriber { get; }

        public MessageBrokerDemo(IPublisher<TestMessage> publisher, ISubscriber<TestMessage> subscriber)
        {
            Publisher = publisher;
            Subscriber = subscriber;
        }

        public void Start()
        {

        }
    }
}
