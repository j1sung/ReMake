using UnityEngine;

public class Manual : OfficeInteractable
{
    [SerializeField] private AudioClip nextPageSFX; // 넘기는 소리

    [Header("Manual Page")]
    [SerializeField] GameObject[] pages;
    private int _currentPage = 0;

    [Header("Button")]
    [SerializeField] private GameObject _nextBtn;
    [SerializeField] private GameObject _prevBtn;

    [SerializeField] private OfficeUIContext _beforeInteractCtx;
    [SerializeField] private OfficeUIContext _AfterStage1Clear;
    [SerializeField] GameObject coverText;

    [SerializeField] private Request _request; 
    private bool isClicked = false;

    void Awake()
    {
        actions = new(){{OfficeState.BeforeInteracts, OnClickBefore},
                        {OfficeState.AfterInteracts, () => OnClickManual(_AfterStage1Clear)},
                        {OfficeState.ReadyStage2, () => OnClickManual(_AfterStage1Clear)},
                        {OfficeState.ReadyStage3, () => OnClickManual(_AfterStage1Clear)}};
    }

    private void OnClickBefore()
    {
        OfficeUIController.Instance.ShowUI(_beforeInteractCtx);
        _currentPage = 0;
        ShowPage(_currentPage);

        if (!isClicked)
        {   
            isClicked = true;
            _request.CheckCondition();
        }
    }

    private void OnClickManual(OfficeUIContext ctx)
    {
        OfficeUIController.Instance.ShowUI(ctx);
        _currentPage = 0;
        ShowPage(_currentPage);
    }

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == index);

        _prevBtn.SetActive(index > 0);
        _nextBtn.SetActive(index < pages.Length - 1 && index > 0);
    }

    public void NextPage()
    {
        PlayInteractSFX(); // 사운드 재생(오버라이드)

        if (_currentPage >= pages.Length - 1) return;
        _currentPage++;

        if (_currentPage == 1)
        {
            HideCoverText();   // 텍스트 숨김 예외 처리
        }
        ShowPage(_currentPage);
    }

    public void PrevPage()
    {
        PlayInteractSFX(); // 사운드 재생(오버라이드)

        if (_currentPage <= 0) return;
        _currentPage--;
        ShowPage(_currentPage);
    }

    private void HideCoverText()
    {
        if (coverText != null)
            coverText.SetActive(false);
    }

    protected override void PlayInteractSFX()
    {
        if (nextPageSFX == null) return;
        SFXPlayer.Instance?.PlaySFX(nextPageSFX);
    }
}
