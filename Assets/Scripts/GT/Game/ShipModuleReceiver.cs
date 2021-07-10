using UnityEngine;

namespace GT.Game
{
    public class ShipModuleReceiver : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = default;
        private void OnMouseUp()
        {
            Debug.Log("click");
        }
    }
}