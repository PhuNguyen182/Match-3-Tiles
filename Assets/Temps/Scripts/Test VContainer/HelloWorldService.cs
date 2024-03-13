using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Temps.Scripts.TestVContainer
{
    public class HelloWorldService
    {
        public string Halo;

        public void Hello()
        {
            Debug.Log("Hello World!");
        }
    }

    public class GoodbyeWorld
    {
        public void Goodbye()
        {
            Debug.Log("Goodbye World!");
        }
    }

    public class TestInjectClass
    {
        public TestInjectClass(IObjectResolver resolver)
        {
            resolver.Inject(123456);
        }
    }
}
