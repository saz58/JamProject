using UnityEngine;
using UnityEngine.EventSystems;

namespace GT.Game.Connectors
{
    public class ModuleConnector : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = default;
        
        
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

        public void Highlight(bool toggle)
        {
            spriteRenderer.enabled = toggle;
        }

        private void OnMouseUp()
        {
            Debug.Log("AAAAA");
        }
    }
}