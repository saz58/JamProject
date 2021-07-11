using CustomExtension;
using GT.Data.Game;
using Pool;
using UnityEngine;

namespace GT.Game
{
    public class PickableModule : MonoBehaviour, IPoolObject
    {
        [SerializeField] private Transform _transform = default;
        [SerializeField] private SpriteRenderer spriteRenderer = default;
        public Vector2 Position => new Vector2(_transform.position.x, _transform.position.y);

        private ModuleData _data;

        public ModuleData Data
        {
            get => _data;
            set => _data = value;
        }

        public Sprite Icon => spriteRenderer.sprite;

        public void AnimateDisplay(Vector2 targetPos)
        {
            var t = Random.Range(0.1F, 0.4F);
            StartCoroutine(_transform.EaseMove(targetPos, t,
                ease: EasingFunction.Ease.EaseOutCubic));
            StartCoroutine(_transform.RotateDuringTime(Vector3.forward, t, 4));
            StartCoroutine(_transform.Scale(Vector3.one, t));
        }

        public void OnGetWithPool()
        {
            gameObject.SetActive(true);
            _transform.localScale = Vector3.zero;
            _data = DataHandler.AddInGameModule(this);
        }

        public void OnReturnToPool()
        {
            StopAllCoroutines();
        }

        public void SetSpriteByType()
        {
            // todo: implement.
        }

        public void Pick()
        {
            StartCoroutine(_transform.Scale(Vector3.zero, 0.5F));
            StartCoroutine(_transform.RotateDuringTime(Vector3.forward, 0.5F, 4,
                () => { PoolManager.Return(nameof(PickableModule), this); }));
        }
    }
}