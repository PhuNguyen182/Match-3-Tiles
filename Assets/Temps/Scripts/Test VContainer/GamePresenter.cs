using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using VContainer;
using VContainer.Unity;

namespace Temps.Scripts.TestVContainer
{
    public class GameTester : IStartable
    {
        public GameTester(string test)
        {
            Debug.Log(test);
        }

        public void Start()
        {
            
        }

        [Inject]
        public void SetValue(int num)
        {
            Debug.Log(num);
        }
    }

    public class GamePresenter : IStartable
    {
        public string _s, _s2;
        private readonly HelloScreen _helloScreen;
        private readonly HelloWorldService _service;
        private readonly GoodbyeWorld _goodbye;

        public GamePresenter(HelloWorldService service, GoodbyeWorld goodbye, HelloScreen helloScreen)
        {
            _service = service;
            _goodbye = goodbye;
            _helloScreen = helloScreen;
        }

        public void Start()
        {
            _helloScreen.HelloButton.onClick.AddListener(() =>
            {
                _service.Hello();
                _goodbye.Goodbye();
                Debug.Log(_s);
            });
        }


        [Inject]
        public void SetString(string s)
        {
            _s = s;
            Debug.Log(_s);
        }

        // When Inject same format methods, they will be executed both
        //[Inject]
        //public void SetString2(string s)
        //{
        //    _s2 = s;
        //    Debug.Log(_s2);
        //}
    }
}
