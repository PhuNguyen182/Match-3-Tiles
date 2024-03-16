using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;

namespace TestMessagePipe 
{
    public struct TestMessage
    {
        public int Number;
        public string Message;
    }

    public class ExampleClassA : IDisposable
    {
        private readonly string _testValue;
        private readonly IPublisher<TestMessage> _publisher;
        private readonly IPublisher<TestMessage2> _publisher2;

        public ExampleClassA()
        {
            _testValue = "Class A";
            _publisher = GlobalMessagePipe.GetPublisher<TestMessage>();
            _publisher2 = GlobalMessagePipe.GetPublisher<TestMessage2>();
        }

        public void PublishMessage()
        {
            _publisher.Publish(new TestMessage
            {
                Message = "This is first message",
                Number = 1000
            });

            _publisher2.Publish(new TestMessage2
            {
                Message = "This is second message"
            });
        }

        public void Dispose()
        {
            Debug.Log($"{_testValue} dispose!");
        }
    } 
}
