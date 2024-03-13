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

        public ExampleClassA(IPublisher<TestMessage> publisher)
        {
            _testValue = "Class A";
            _publisher = publisher;
        }

        public void PublishMessage()
        {
            _publisher.Publish(new TestMessage
            {
                Message = "This is first message",
                Number = 1000
            });
        }

        public void Dispose()
        {
            Debug.Log($"{_testValue} dispose!");
        }
    } 
}
