using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject album;
    public GameObject albumBook;
    public GameObject credits;

    public void StartButton()
    {
        GameManager.instance.GoOffice();
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
