using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer.Unity;
using VContainer;

namespace TestMessagePipe
{
    public struct TestMessage3
    {
        public int NumberValue;
    }

    public class TestMessage3MessageInitializer : BaseMessageRegister
    {
        public IPublisher<TestMessage3> Publisher { get; private set; }
        public ISubscriber<TestMessage3> Subscriber { get; private set; }

        public override void SetContainerBuilder(IContainerBuilder container, MessagePipeOptions options)
        {
            base.SetContainerBuilder(container, options);
            container.RegisterMessageBroker<TestMessage3>(messagePipeOptions);
            container.RegisterEntryPoint<MessageBrokerTestMessage3>(Lifetime.Singleton);
            container.RegisterBuildCallback(container =>
            {
                Publisher = container.Resolve<IPublisher<TestMessage3>>();
                Subscriber = container.Resolve<ISubscriber<TestMessage3>>();
            });
        }
    }

    public class MessageBrokerTestMessage3 : IStartable
    {
        public IPublisher<TestMessage3> Publisher { get; }
        public ISubscriber<TestMessage3> Subscriber { get; }

        public MessageBrokerTestMessage3(IPublisher<TestMessage3> publisher, ISubscriber<TestMessage3> subscriber)
        {
            Publisher = publisher;
            Subscriber = subscriber;
        }

        public void Start() { }
    }
}