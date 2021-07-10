using UnityEngine;

namespace GT.Game.Connectors
{
    public class ModuleConnector : MonoBehaviour
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => transform.localPosition = _position = value;
        }

        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
    }
}