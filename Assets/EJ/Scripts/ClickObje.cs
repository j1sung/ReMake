using UnityEngine;
using UnityEngine.EventSystems;

public class ClickObje : MonoBehaviour
{
    [SerializeField] UIManager ui;
    [SerializeField] InventoryManager inventory;
    public ObjeData data;

    void OnMouseDown()
    {
        // 1. 팝업 표시
        if (ui != null) ui.OnClickObje(data);

        // 2. 인벤토리에 추가
        if (inventory != null) inventory.AddButton(data);

        // 3. 습득 처리
        gameObject.SetActive(false);
    }
}