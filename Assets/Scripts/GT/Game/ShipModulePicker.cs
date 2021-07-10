using GT.Data.Game;
using UnityEngine;

namespace GT.Game
{
    public class ShipModulePicker : MonoBehaviour
    {
        [SerializeField] private Transform _transform; 
        // todo: Ask pick on a press some button 
        private void LateUpdate()
        {
            if (DataHandler.CheckModulesPosition(new Vector2(_transform.position.x, _transform.position.y)))
            {
                Debug.Log("Detect");
            }
        }
    }
}