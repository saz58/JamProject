using TMPro;
using UnityEditor;
using UnityEngine;

namespace GT.CustomComponent.CustomButton
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CaptionButton))]
    public class CaptionButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
#endif

    public class CaptionButton : GroupGraphicButton
    {
        [SerializeField] private TextMeshProUGUI caption = default;

        public string Caption
        {
            get => caption.text;
            set => caption.text = value;
        }
    }
}