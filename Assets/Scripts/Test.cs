using System;
using GT.Asset;
using GT.Game.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace TestV
{
    public class Test : MonoBehaviour, IResourceReferenceHolder
    {
        [SerializeField] private Button a;
        [SerializeField] private Button b;
        [SerializeField] private Button c;

        public Action DestroyResource { get; set; }


        [EditorButton]
        private void TestA()
        {
            RandomPickableModuleSpawner.SpawnGroupModulesByRadius(Vector2.zero);

        }
    }
}