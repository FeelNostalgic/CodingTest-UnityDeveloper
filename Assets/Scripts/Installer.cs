using CodingTest.Controllers;
using CodingTest.Serializer;
using CodingTest.Utility;
using UnityEngine;

namespace Installer
{
    public class Installer : MonoBehaviour
    {
        private void Awake()
        {
            ServiceContainer.AddInstance(new SaveController(new UnityJsonSerializer()));
        }
    }
}