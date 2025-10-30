using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    private GameObject album;
    private GameObject albumBook;
    public GameObject credits;

    [Header("Audios")]
    [SerializeField] private AudioClip buttonClickClip;  // 버튼 클릭 소리
    [SerializeField] private AudioClip x_ButtonClickClip;  // X 버튼 클릭 소리

    private void Start()
    {
        var albumCanvas = ResultManager.instance.albumObject;
        album = albumCanvas.transform.Find("Album")?.gameObject;
        albumBook = album.transform.Find("AlbumBook")?.gameObject;
    }

    public void StartButton()
    {   
        SFXPlayer.Instance.PlaySFX(buttonClickClip);
        GameManager.Instance.MoveScene(SceneData.Office);
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
