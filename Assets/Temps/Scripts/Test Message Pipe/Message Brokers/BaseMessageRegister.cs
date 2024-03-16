using MessagePipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace TestMessagePipe
{
    public class BaseMessageRegister : MonoBehaviour, IMessageRegister
    {
        protected IContainerBuilder container;
        protected MessagePipeOptions messagePipeOptions;

        public virtual void SetContainerBuilder(IContainerBuilder container, MessagePipeOptions options)
        {
            this.container = container;
            messagePipeOptions = options;
        }
    }
}
