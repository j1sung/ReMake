using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIState { Room, Bag, Settings }

public class UIManager : MonoBehaviour
{
    [SerializeField] private ColorOverlayEffect overlayEffect;

    [SerializeField] GameObject bg;
    [SerializeField] Sprite spaceBg;
    [SerializeField] Sprite puzzleBg;

    [Header("Panels")]
    [SerializeField] GameObject roomUI;     // 방 HUD 패널
    [SerializeField] GameObject ObjePopupUI;    // 팝업 패널
    [SerializeField] GameObject roomAll;  // 방 전체 오브젝트
    [SerializeField] GameObject bagUI;      // 가방 패널
    [SerializeField] GameObject submitPopupUI; // 제출 패널

    
    [SerializeField] private Text ObjeNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image ObjeImage;

    [SerializeField] private InvenGridManager InvenGridManager;

    private ClickObje currentObje; // 현재 선택중인 오브제 정보

    private bool isConfirmStep = false; // 제출할지 한번 확인 변수

    [Header("Audios")]
    public AudioClip buttonClickClip;  // 버튼 클릭 소리
    public AudioClip x_ButtonClickClip;  // X 버튼 클릭 소리
    public AudioClip openBag;
    public AudioClip closeBag;
    
    public UIState State { get; private set; } = UIState.Room;

    void Start() => SetState(UIState.Room);

    // ======공간 이동 Room/Bag=======
    // Bag 열기
    public void OnClickOpenBag()
    {   
        SFXPlayer.Instance.PlaySFX(openBag);
        SetState(UIState.Bag);
    }

    // Bag 닫기
    public void OnClickCloseBag()
    {   
        SFXPlayer.Instance.PlaySFX(closeBag);
        SetState(UIState.Room);
    }
    // ======팝업 On/Off=======
    // 오브젝트 팝업 열기
    public void OnClickObje(ClickObje obje)
    {
        currentObje = obje;

        ObjePopupUI.SetActive(true);
        ObjePopupUI.transform.SetAsLastSibling(); // 항상 팝업을 맨 위로 배치
        ObjeNameText.text = obje.data.objeName;
        descriptionText.text = obje.data.objeDescription;
        ObjeImage.sprite = obje.data.puzzleImage;
    }

    public void OnClickInteractiveObje()
    {
        if (currentObje.IsInteracted == false) return;
        ObjeNameText.text = currentObje.data.objeName;
        descriptionText.text = currentObje.data.secretDescription;
        ObjeImage.sprite = currentObje.data.secretImage;
        // 패널 창에서 상호작용 했다면, false로 변경.
        currentObje.IsInteracted = false;
    }

    // 오브젝트 팝업 닫기
    public void OnClickClosePopup()
    {   
        ObjePopupUI.SetActive(false);
        // 팝업이 닫힐 때 오브제 습득 처리
        if (currentObje != null)
        {
            currentObje.Acquire();
            currentObje = null;
        }
    }

// ======Bag 제출 팝업 On/Off======
    // 제출 팝업 열기
    public void OnClickSubmit()
    {   
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        submitPopupUI.SetActive(true);
    }
    
    // 제출하기
    public void OnClickSubmitYes()
    {
        // 첫번째 Yes
        if (!isConfirmStep)
        {
            SFXPlayer.Instance.PlaySFX(buttonClickClip);
            TMP_Text popupText = submitPopupUI.transform.Find("Pop_up_image/Text").GetComponent<TMP_Text>();
            popupText.text = "선택하지 않은 기억들은\n시간의 흐름에 따라 희미해집니다.";
            isConfirmStep = true;
            return;
        }

        // 두번째 Yes -> 제출하기
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        InvenGridManager.SubmitItems();
        GameManager.Instance.MoveScene(SceneData.Result);
    }

    // 제출 팝업 닫기
    public void OnClickSubmitNo()
    {   
        SFXPlayer.Instance.PlaySFX(x_ButtonClickClip);
        isConfirmStep = false;
        TMP_Text popupText = submitPopupUI.transform.Find("Pop_up_image/Text").GetComponent<TMP_Text>();
        popupText.text = "포장을 끝내고\n방을 떠날까요?";
        submitPopupUI.SetActive(false);
    }

    // ======Room/Bag 공간 설정======
    void SetState(UIState next)
    {
        State = next;

        bool isRoom = next == UIState.Room;
        bool isBag  = next == UIState.Bag;

        roomUI.SetActive(isRoom); // 방 버튼 누르면 열림
        roomAll.SetActive(isRoom);
        bagUI.SetActive(isBag); // 가방 버튼 누르면 열림

        overlayEffect.enabled = isRoom; // 카메라 후처리 On/Off

        bg.GetComponent<SpriteRenderer>().sprite = isRoom ? spaceBg : puzzleBg;


        if (!isRoom) ObjePopupUI.SetActive(false); // 방이 아니면 팝업 자동 닫기
    }
}