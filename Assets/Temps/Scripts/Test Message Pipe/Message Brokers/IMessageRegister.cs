using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;

namespace TestMessagePipe
{
    public interface IMessageRegister
    {
        public void SetContainerBuilder(IContainerBuilder container, MessagePipeOptions options);
    }
}
