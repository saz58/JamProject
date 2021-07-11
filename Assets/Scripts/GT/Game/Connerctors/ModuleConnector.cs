using GT.Data.Game;
using GT.Game.Swarms;
using UnityEngine;
using UnityEngine.EventSystems;
using Pool;

namespace GT.Game.Connectors
{
    public class ModuleConnector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPoolObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer = default;
        private Color32 _spriteColor;
        private SwarmConstructor constructor = default;
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

        private void Awake()
        {
            _spriteColor = spriteRenderer.color;
            constructor = transform.parent.GetComponent<SwarmConstructor>();
        }

        public void Highlight(bool toggle)
        {
            spriteRenderer.enabled = toggle;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (DataHandler.SelectedData == null) return;
            constructor.AddModule(DataHandler.SelectedData, Position);

            DataHandler.ApplySelectedData();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g, spriteRenderer.color.b, 255);
        }
        
        public void OnGetWithPool()
        {
        }

        public void OnReturnToPool()
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            spriteRenderer.color = _spriteColor;
        }
    }
}