#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace CustomExtension
{
    public static class CustomGizmos
    {
        private static Color32 textColor;
        private static Color32 bgColor;
        public static void DrawString(string text, Vector3 worldPos)
        {
            textColor = Color.green;
            bgColor = Color.blue;
            Handles.BeginGUI();

            GUI.color = Color.green;
            GUI.backgroundColor = Color.blue;

            var view = SceneView.currentDrawingSceneView;
            if (view != null && view.camera != null)
            {
                Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
                if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width ||
                    screenPos.z < 0)
                {
                    GUI.color = textColor;
                    Handles.EndGUI();
                    return;
                }

                Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
                var r = new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y);
                GUI.Box(r, text, EditorStyles.numberField);
                GUI.Label(r, text);
                GUI.color = textColor;
                GUI.backgroundColor = bgColor;
            }

            Handles.EndGUI();
        }
    }
}
#endif
