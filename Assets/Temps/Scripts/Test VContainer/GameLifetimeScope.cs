using VContainer;
using VContainer.Unity;
using UnityEngine;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Entities;

namespace Temps.Scripts.TestVContainer
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private MatchTileBlock prefab;
        [SerializeField] private HelloScreen helloScreen;
        

        protected override void Configure(IContainerBuilder builder)
        {
            builder.UseEntryPoints(entries =>
            {
                entries.Add<GamePresenter>();
                entries.Add<GameTester>();
            });
            
            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.Register<GoodbyeWorld>(Lifetime.Singleton);
            builder.RegisterInstance("000");
            builder.RegisterInstance(123);
            builder.RegisterComponent(helloScreen);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var x = this.Container.Resolve<HelloWorldService>();
                Debug.Log(x.Halo);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                var x = new TestInjectClass(this.Container);
            }
        }
    }
}
