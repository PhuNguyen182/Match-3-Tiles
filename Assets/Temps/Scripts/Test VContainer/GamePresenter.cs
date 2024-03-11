using UnityEngine;
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
    }

    public class GamePresenter : IStartable
    {
        private string _s, _s2;
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
