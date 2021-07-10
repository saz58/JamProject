using UnityEngine;

public class ModuleConnector : MonoBehaviour
{
    public Vector2 Position { get; set; }

    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }
}
