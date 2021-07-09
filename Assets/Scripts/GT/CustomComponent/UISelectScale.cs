using CustomExtension;
using UnityEngine;

namespace GT.CustomComponent
{
    public class UISelectScale : MonoBehaviour
    {
        [SerializeField] private RectTransform[] rects = default;
        [SerializeField] [Range(0.0f, 1.0f)] private float scale = 0.98f;

        public void ResetScale()
        {
            foreach (var rect in rects) rect.localScale = Vector3.one;
        }

        public void Scale(bool isPressed)
        {
            foreach (var rect in rects)
                rect.localScale = isPressed ? new Vector3(scale, scale, scale) : Vector3.one;
        }

        public void ScaleEffect()
        {
            foreach (var rect in rects)
            {
                StartCoroutine(rect.Scale(new Vector2(0.7f, 0.7f), 0.24f, completed:
                    () =>
                    {
                        StartCoroutine(rect.Scale(new Vector2(1.07f, 1.07f), 0.24f, completed:
                            () => { StartCoroutine(rect.Scale(new Vector2(1f, 1f), 0.12f)); }));
                    }));
            }
        }
    }
}