using UnityEngine;

public class Slide_Manual : MonoBehaviour
{
    [Header("Manual Page")]
    [SerializeField] GameObject[] pages;
    [SerializeField] AudioClip nextClip;
    private int _currentPage;

    void OnEnable()
    {
        if (pages == null || pages.Length == 0) return;

        _currentPage = 0;
        ShowPage(_currentPage);
    }

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == index);
    }

    public void NextPage()
    {
        SFXPlayer.Instance.PlaySFX(nextClip);
        if (_currentPage == pages.Length - 1)
        {
            _currentPage = 0;
        }
        else
            _currentPage++;

        ShowPage(_currentPage);
    }

    public void PrevPage()
    {
        SFXPlayer.Instance.PlaySFX(nextClip);
        if (_currentPage == 0)
        {
            _currentPage = pages.Length - 1;
        }
        else
            _currentPage--;

        ShowPage(_currentPage);
    }
}
