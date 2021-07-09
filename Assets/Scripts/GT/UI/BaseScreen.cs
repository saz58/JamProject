using GT.Asset;
using UnityEngine;

namespace GT.UI
{
    public abstract class BaseScreen : MonoBehaviour, IResourceReferenceHolder
    {
        public abstract void Open(System.Action onOpen);

        public abstract void Close(System.Action onClose);

        
        public System.Action DestroyResource { get; set; }

        private void OnDestroy()
        {
            DestroyResource?.Invoke();
        }
    }
}