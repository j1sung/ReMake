using UnityEngine;

public enum UIState { Room, Bag, Settings }

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject roomUI;     // 방 HUD 패널
    [SerializeField] GameObject roomImage;  // 방 이미지
    [SerializeField] GameObject bagUI;      // 가방 패널
    [SerializeField] GameObject popupUI;    // 팝업 패널

    public UIState State { get; private set; } = UIState.Room;

    void Start() => SetState(UIState.Room);

    public void OnClickOpenBag()   => SetState(UIState.Bag);
    public void OnClickCloseBag()  => SetState(UIState.Room);

    // 오브젝트 클릭 시 호출
    public void OnClickObje()
    {
        if (State != UIState.Room) return;
        popupUI.SetActive(true);
        popupUI.transform.SetAsLastSibling(); // 항상 팝업을 맨 위로 배치
        
    }

    public void OnClickClosePopup()
    {
        popupUI.SetActive(false);
    }



    void SetState(UIState next)
    {
        State = next;

        bool isRoom = next == UIState.Room;
        bool isBag  = next == UIState.Bag;

        roomUI.SetActive(isRoom);
        roomImage.SetActive(isRoom);
        bagUI.SetActive(next == UIState.Bag);

        if (!isRoom) popupUI.SetActive(false); // 방이 아니면 팝업 자동 닫기
    }
}