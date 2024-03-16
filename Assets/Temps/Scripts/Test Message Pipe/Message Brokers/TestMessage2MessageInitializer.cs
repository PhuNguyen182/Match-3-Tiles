using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer.Unity;
using VContainer;

namespace TestMessagePipe
{
    public struct TestMessage2
    {
        public string Message;
    }

    public class TestMessage2MessageInitializer : BaseMessageRegister
    {
        public IPublisher<TestMessage2> Publisher { get; private set; }
        public ISubscriber<TestMessage2> Subscriber { get; private set; }

        public override void SetContainerBuilder(IContainerBuilder container, MessagePipeOptions options)
        {
            base.SetContainerBuilder(container, options);
            container.RegisterMessageBroker<TestMessage2>(messagePipeOptions);
            container.RegisterEntryPoint<MessageBrokerTestMessage2>(Lifetime.Singleton);
            container.RegisterBuildCallback(container =>
            {
                Publisher = container.Resolve<IPublisher<TestMessage2>>();
                Subscriber = container.Resolve<ISubscriber<TestMessage2>>();
            });
        }
    }

    public class MessageBrokerTestMessage2 : IStartable
    {
        public IPublisher<TestMessage2> Publisher { get; }
        public ISubscriber<TestMessage2> Subscriber { get; }

        public MessageBrokerTestMessage2(IPublisher<TestMessage2> publisher, ISubscriber<TestMessage2> subscriber)
        {
            Publisher = publisher;
            Subscriber = subscriber;
        }

        public void Start() { }
    }
}