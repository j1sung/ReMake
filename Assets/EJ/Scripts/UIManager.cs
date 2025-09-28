using UnityEngine;
using UnityEngine.UI;

public enum UIState { Room, Bag, Settings }

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject roomUI;     // 방 HUD 패널
    [SerializeField] GameObject ObjePopupUI;    // 팝업 패널
    [SerializeField] GameObject roomAll;  // 방 전체 오브젝트
    [SerializeField] GameObject bagUI;      // 가방 패널
    [SerializeField] GameObject submitPopupUI; // 제출 패널

    
    [SerializeField] private Text ObjeNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image ObjeImage;
    
    

    public UIState State { get; private set; } = UIState.Room;

    void Start() => SetState(UIState.Room);

    // ======공간 이동 Room/Bag=======
    // Bag 열기
    public void OnClickOpenBag()   => SetState(UIState.Bag);

    // Bag 닫기
    public void OnClickCloseBag()  => SetState(UIState.Room);

    // ======팝업 On/Off=======
    // 오브젝트 팝업 열기
    public void OnClickObje(ObjeData data)
    {
        if (State != UIState.Room) return;
        ObjePopupUI.SetActive(true);
        ObjePopupUI.transform.SetAsLastSibling(); // 항상 팝업을 맨 위로 배치
        ObjeNameText.text = data.objeName;
        descriptionText.text = data.objeDescription;
        ObjeImage.sprite = data.icon;
    }

    // 오브젝트 팝업 닫기
    public void OnClickClosePopup()
    {
        ObjePopupUI.SetActive(false);
    }

    // ======Bag 제출 팝업 On/Off======
    // 제출 팝업 열기
    public void OnClickSubmit()
    {
        submitPopupUI.SetActive(true);
    }
    /*
    // 제출하기
    public void OnClickSubmitYes()
    {
        submitPopupUI.SetActive(false);
    }*/

    // 제출 팝업 닫기
    public void OnClickSubmitNo()
    {
        submitPopupUI.SetActive(false);
    }

    // ======Room/Bag 공간 설정======
    void SetState(UIState next)
    {
        State = next;

        bool isRoom = next == UIState.Room;
        bool isBag  = next == UIState.Bag;

        roomUI.SetActive(isRoom);
        roomAll.SetActive(isRoom);
        bagUI.SetActive(isBag);

        if (!isRoom) ObjePopupUI.SetActive(false); // 방이 아니면 팝업 자동 닫기
    }
}