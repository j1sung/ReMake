using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Image continueButtonImage;
    [SerializeField] private Color enabledColor = Color.white;
    [SerializeField] private Color disabledColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    [SerializeField] private Button continueButton;
    private GameObject album;
    private GameObject albumBook;
    [SerializeField] private GameObject credits;

    [Header("Audios")]
    [SerializeField] private AudioClip buttonClickClip;  // 버튼 클릭 소리
    [SerializeField] private AudioClip x_ButtonClickClip;  // X 버튼 클릭 소리

    private void Start()
    {
        var albumCanvas = ResultManager.instance.resultCanvas;
        album = albumCanvas.transform.Find("Album")?.gameObject;
        albumBook = album.transform.Find("AlbumBook")?.gameObject;
        RefreshContinueButton();
    }

    private void RefreshContinueButton()
    {
        bool hasSave = DataManager.Instance.HasGameSaveFile();
        continueButton.interactable = hasSave;
        continueButtonImage.color = hasSave? enabledColor : disabledColor;
    }

    public void StartButton()
    {   
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        GameManager.Instance.MoveScene(SceneData.Office);
    }

    public void OnClickContinue()
    {
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        DataManager.Instance.LoadGame();
    }

    public void AlbumButton()
    {   
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        album.SetActive(true);
        albumBook.SetActive(true);
    }

    public void CreditsOn()
    {   
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        credits.SetActive(true);
    }

    public void CreditsOff()
    {   
        SFXPlayer.Instance.PlaySFX(x_ButtonClickClip);
        credits.SetActive(false);
    }
}
