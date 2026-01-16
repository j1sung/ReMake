/// 오브제를 클릭했을때 발생하는 코드
/// 마우스로 클릭했을때 해당 오브제와 관련된 팝업창을 띄운다.
/// 만약 획득이 확정되면 1. 인벤토리에 추가하고, 2. 해당 오브제를 씬에서 비활성화한다.

using UnityEngine;
using UnityEngine.EventSystems;

public enum ObjeInteractResult
{
    None,
    CanInteract, // 상호작용
    CycleImage // 이미지 순환
}

public class ClickObje : MonoBehaviour
{
    [SerializeField] RoomUIManager ui;
    [SerializeField] InventoryManager inventory;
    public ObjeData data;
    public bool IsInteracted;

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 팝업 표시
        if (ui != null)
        {
            SFXPlayer.Instance.PlaySFX(data.objeSound);
            IsInteracted = data.canInteract;
            ui.OnClickObje(this);
        }
    }
    // 오브제를 획득했을때 호출
    public void Acquire()
    {
        if (IsInteracted == true) return;
        if (inventory != null) inventory.AddButton(data); // 인벤토리에 추가
        gameObject.SetActive(false); // 
    }

    public ObjeInteractResult Interact()
    {
        IsInteracted = data.canInteract;

        // 숨겨진 정보가 있고, 아직 사용 안 했을 때
        if (IsInteracted == true)
        {
            return ObjeInteractResult.CanInteract;
        }

        // 비밀이 없는데 두번째 사진이 있으면 이미지 순환만 허용
        if (data.secondiconImage != null)
            return ObjeInteractResult.CycleImage;

        return ObjeInteractResult.None;
    }
}