using VContainer;
using VContainer.Unity;

namespace Temps.Scripts.TestVContainer
{
    public class GamePresenter : IStartable
    {
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
    }
}
