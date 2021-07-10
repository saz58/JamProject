using CustomExtension;
using GT.Data.Game;
using UnityEngine;

namespace GT.Game
{
    public class ShipPickableModule : MonoBehaviour
    {
        [SerializeField] private Transform _transform = default;
        [SerializeField] private SpriteRenderer spriteRenderer =default; 

        public Vector2 Position => new Vector2(_transform.position.x, _transform.position.y);

        private ModuleData _data;
        public ModuleData Data => _data;
        public Sprite Icon => spriteRenderer.sprite;

        private void Awake()
        {
            _data = DataHandler.AddInGameShipModule(this);
        }

        public void Pick()
        {
            StartCoroutine(_transform.Scale(Vector3.zero, 0.5F));
            StartCoroutine(_transform.RotateDuringTime(Vector3.forward, 0.5F, 4, () => { Destroy(gameObject); }));
        }
    }
}