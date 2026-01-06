using System.Collections;
using UnityEngine;

public class Request : OfficeInteractable
{
    [SerializeField] private OfficeUIContext _beforeInteractCtx;
    [SerializeField] private OfficeUIContext _afterInteractCtx;

    [Header("Interacts Condition")]
    private int _clickedCount;
    [SerializeField] private int _needCount = 2;

    [Header("Coroutine")]
    private bool _isPlaying;
    private Coroutine _co;
    private bool _requestClicked; 

    [Header("UI")]
    [SerializeField] GameObject normalText;
    [SerializeField] GameObject signText;
    [SerializeField] GameObject signImage;

    void Awake()
    {
        actions = new() { { OfficeState.BeforeInteracts, OnClickRequestBeforeInteracts }, 
                          { OfficeState.AfterInteracts, OnClickRequestAfterInteracts} };
    }

    private void OnDisable()
    {
        if (_co != null)
        {
            StopCoroutine(_co);
            _co = null;
        }
        _isPlaying = false;
    }

    private void OnClickRequestBeforeInteracts()
    {
        OfficeUIController.Instance.ShowUI(_beforeInteractCtx);
    }

    private void OnClickRequestAfterInteracts()
    {
        if (_isPlaying) return; // 중복 클릭 방지
        _co = StartCoroutine(RequestDirection());
    }

    public void CheckCondition()
    {
        _clickedCount++;

        if (_clickedCount >= _needCount)
        {
            OfficeStateMachine.SetState(OfficeState.AfterInteracts);
        }
    }

    IEnumerator RequestDirection()
    {
        _isPlaying = true;
        _requestClicked = false;

        // 1. 관련 UI 띄우기
        OfficeUIController.Instance.ShowUI(_afterInteractCtx);

        // 2. 2초 대기
        yield return new WaitForSeconds(1.5f);

        // 3. 대사 바꾸기
         normalText.SetActive(false);
         signText.SetActive(true);
         
        // 4. 의뢰서 클릭 전까지 대기
        yield return new WaitUntil(() => _requestClicked);

        // 5. 사인
        signImage.SetActive(true);
        // + 사인 소리 

        // 6. 씬 넘어 가기 전 잠시 대기
        yield return new WaitForSeconds(3f);

        // 7. 씬 이동
        GameManager.Instance.MoveScene(SceneData.Room);
    }

    public void RequestSign()
    {
        _requestClicked = true;
    }
}
