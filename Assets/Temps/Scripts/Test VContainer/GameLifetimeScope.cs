using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Temps.Scripts.TestVContainer
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private HelloScreen helloScreen;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GamePresenter>();
            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.Register<GoodbyeWorld>(Lifetime.Singleton);
            builder.RegisterComponent(helloScreen);
        }
    }
}
