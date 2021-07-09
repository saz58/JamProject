using UnityEngine;
using UnityEngine.UI;

namespace GT.CustomComponent.ProgressBar
{
    /*
     todo: make custom component.
     - Side from pivot based. left/right.
     - From min.
     - From max.
     
     - create new vertical.
     */
    public class UIHorizontalScaleProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform progress = default;
        [SerializeField] [Range(0, 1)] private float xPivot = default;
        [SerializeField] private Image image = default;
        private Vector2 _size;

        public Image Image => image;

        private void Awake()
        {
            _size = progress.localScale;
            progress.pivot = new Vector2(xPivot, 0.5f);
        }

        public void UpdateProgress(float value)
        {
            var localScale = progress.localScale;
            localScale = new Vector2(_size.x - _size.x * value, localScale.y);
            progress.localScale = localScale;
        }
    }
}