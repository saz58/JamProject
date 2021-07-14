using CustomExtension;
using GT.Data.Game;
using GT.Game.Modules;
using Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GT.Game
{
    public class PickableModule : MonoBehaviour, IPoolObject
    {
        [SerializeField] private Sprite attack;
        [SerializeField] private Sprite shield;
        [SerializeField] private Sprite speed;
        [SerializeField] private Transform _transform = default;
        [SerializeField] private SpriteRenderer spriteRenderer = default;
        public Vector2 Position => new Vector2(_transform.position.x, _transform.position.y);

        private ModuleData _data;

        public ModuleData Data
        {
            get => _data;
            set => _data = value;
        }
        public void AnimateDisplay(Vector2 targetPos)
        {
            _transform.localScale = Vector3.zero;
            
            var t = Random.Range(0.1F, 0.4F);
            StartCoroutine(_transform.EaseMove(targetPos, t,
                ease: EasingFunction.Ease.EaseOutCubic));
            StartCoroutine(_transform.RotateDuringTime(Vector3.forward, t, 4));
            StartCoroutine(_transform.Scale(Vector3.one, t, ease: EasingFunction.Ease.EaseOutCubic));
        }

        public void OnGetWithPool()
        {
            gameObject.SetActive(true);
            _data = DataHandler.AddInGameModule(this);
            SetSprite();
            void SetSprite()
            {
                switch (_data.Type)
                {
                    case ModuleType.Attack:
                        spriteRenderer.sprite = attack;
                        break;
                    case ModuleType.Shield:
                        spriteRenderer.sprite = shield;
                        break;
                    case ModuleType.Speed:
                        spriteRenderer.sprite = speed;
                        break;
                }
            }
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