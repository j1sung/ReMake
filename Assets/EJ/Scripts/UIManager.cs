using UnityEngine;
using UnityEngine.UI;

public enum UIState { Room, Bag, Settings }

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject roomUI;     // 방 HUD 패널
    [SerializeField] GameObject roomImage;  // 방 이미지
    [SerializeField] GameObject bagUI;      // 가방 패널
    [SerializeField] GameObject popupUI;    // 팝업 패널
    [SerializeField] private Text ObjeNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image ObjeImage;

    private ClickObje currentObje; // 현재 선택중인 오브제 정보
    
    public UIState State { get; private set; } = UIState.Room;

    void Start() => SetState(UIState.Room);

    public void OnClickOpenBag()   => SetState(UIState.Bag);
    public void OnClickCloseBag()  => SetState(UIState.Room);

    // 오브젝트 클릭 시 호출
    public void OnClickObje(ClickObje obje)
    {
        currentObje = obje;

        if (State != UIState.Room) return;
        popupUI.SetActive(true);
        popupUI.transform.SetAsLastSibling(); // 항상 팝업을 맨 위로 배치
        ObjeNameText.text = obje.data.objeName;
        descriptionText.text = obje.data.objeDescription;
        ObjeImage.sprite = obje.data.icon;
    }

    public void OnClickClosePopup()
    {
        popupUI.SetActive(false);
        // 팝업이 닫힐 때 오브제 습득 처리
        if (currentObje != null)
        {
            currentObje.Acquire();
            currentObje = null;
        }
    }



    void SetState(UIState next)
    {
        State = next;

        bool isRoom = next == UIState.Room;
        bool isBag  = next == UIState.Bag;

        roomUI.SetActive(isRoom);
        roomImage.SetActive(isRoom);
        bagUI.SetActive(next == UIState.Bag);

        if (!isRoom)
        {
            popupUI.SetActive(false); // 방이 아니면 팝업 자동 닫기
            roomImage.SetActive(false); // 방 이미지 닫기
        }
    }
}