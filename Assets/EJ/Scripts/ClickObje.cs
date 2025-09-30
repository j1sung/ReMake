/// 오브제를 클릭했을때 발생하는 코드
/// 마우스로 클릭했을때 해당 오브제와 관련된 팝업창을 띄운다.
/// 만약 획득이 확정되면 1. 인벤토리에 추가하고, 2. 해당 오브제를 씬에서 비활성화한다.

using UnityEngine;
using UnityEngine.EventSystems;

public class ClickObje : MonoBehaviour
{
    [SerializeField] UIManager ui;
    [SerializeField] InventoryManager inventory;
    public ObjeData data;

    public void OnMouseDown()
    {
        // 팝업 표시
        if (ui != null) ui.OnClickObje(this);
    }

    // 오브제를 획득했을때 호출
    public void Acquire()
    {
        if (inventory != null) inventory.AddObje(data); // 인벤토리에 추가
        gameObject.SetActive(false); // 
    }
}