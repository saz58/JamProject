using GT.Data;
using UnityEngine;

namespace GT.Test
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log(CustomPlayerPrefs.GetInt("test"));
            CustomPlayerPrefs.SetInt("test", 1);
            Debug.Log(CustomPlayerPrefs.GetInt("test"));
            CustomPlayerPrefs.Flush();
        }

        [EditorButton]
        private void TestA()
        {
            
        }
    }
}