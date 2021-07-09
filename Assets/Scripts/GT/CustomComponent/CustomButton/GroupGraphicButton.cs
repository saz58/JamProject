using UnityEngine;
using UnityEngine.UI;

namespace GT.CustomComponent.CustomButton
{
    public class GroupGraphicButton : Button
    {
        [Header("Group target graphics")]
        [SerializeField] private Graphic[] graphicsGroup = default;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            if (graphicsGroup.Length <= 0)
                Debug.LogWarning(
                    "If you don't use serialized list of Graphics you can use Unity built-in UI.Button component.");
#endif
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            Color tintColor;
            switch (state)
            {
                case SelectionState.Normal:
                    tintColor = colors.normalColor;
                    break;
                case SelectionState.Highlighted:
                    tintColor = colors.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    tintColor = colors.pressedColor;
                    break;
                case SelectionState.Disabled:
                    tintColor = colors.disabledColor;
                    break;
                case SelectionState.Selected:
                    tintColor = colors.selectedColor;
                    break;
                default:
                    tintColor = Color.black;
                    break;
            }


            if (transition.Equals(Transition.ColorTint))
                ColorTween(tintColor * colors.colorMultiplier);

            void ColorTween(Color targetColor)
            {
                foreach (var g in graphicsGroup)
                    g.CrossFadeColor(targetColor, !instant ? colors.fadeDuration : 0f, true, true);
            }
        }
    }
}