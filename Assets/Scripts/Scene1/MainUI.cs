using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    private GameObject album;
    private GameObject albumBook;
    public GameObject credits;

    private void Start()
    {
        var albumCanvas = ResultManager.instance.albumObject;
        album = albumCanvas.transform.Find("Album")?.gameObject;
        albumBook = album.transform.Find("AlbumBook")?.gameObject;
    }

    public void StartButton()
    {
        GameManager.Instance.MoveScene(SceneData.Office);
    }
    public void AlbumButton()
    {
        album.SetActive(true);
        albumBook.SetActive(true);
    }

    public void CreditsOn()
    {
        credits.SetActive(true);
    }

    public void CreditsOff()
    {
        credits.SetActive(false);
    }
}
