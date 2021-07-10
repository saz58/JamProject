using GT.Data.Game;
using UnityEngine;

namespace GT.Game
{
    public class ModulePicker : MonoBehaviour
    {
        [SerializeField] private Transform _transform; 
        // todo: Ask pick on a press some button 
        private void LateUpdate()
        {
            if (DataHandler.CheckModulesPosition(new Vector2(_transform.position.x, _transform.position.z)))
            {
                Debug.Log("Detect");
            }
        }
    }
}