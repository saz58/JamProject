using GT.Data.Game;
using UnityEngine;

namespace GT.Game
{
    public class ModulePicker : MonoBehaviour
    {
        public Transform Transform; 
        private void LateUpdate()
        {
            DataHandler.CheckModulesPosition(new Vector2(Transform.position.x, Transform.position.y));
        }
    }
}