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
        [SerializeField] private MessageManager messageManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(container =>
            {
                GlobalMessagePipe.SetProvider(container.AsServiceProvider());
            });

            var options = builder.RegisterMessagePipe();
            messageManager.SetContainer(builder, options);
        }
    }
}
