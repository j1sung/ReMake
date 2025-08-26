using UnityEngine;
using UnityEngine.EventSystems;

public class ClickObje : MonoBehaviour
{
    [SerializeField] UIManager ui;

    void OnMouseDown()
    {
        if (ui != null) ui.OnClickObje();
    }
}