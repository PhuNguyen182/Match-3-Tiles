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
            builder.UseEntryPoints(entries =>
            {
                // All entry point classes must derive from IStartable or ITickable
                entries.Add<GamePresenter>();
                entries.Add<GameTester>();
            });

            builder.RegisterInstance("Test string");

            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.Register<GoodbyeWorld>(Lifetime.Singleton);
            builder.RegisterComponent(helloScreen);

        }
    }
}
