using System.Collections;
using UnityEngine;

public class Request : OfficeInteractable
{
    [SerializeField] private OfficeUIContext _beforeInteractCtx;
    [SerializeField] private OfficeUIContext _afterInteractCtx;
    [SerializeField] private OfficeUIContext _afterStage1ClearCtx;
    [SerializeField] private OfficeUIContext _afterStage2ClearCtx;

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
    [SerializeField] GameObject sign;
    [SerializeField] GameObject stamp;

    [Header("Audio")]
    [SerializeField] AudioClip signSound;

    void Awake()
    {
        actions = new() { { OfficeState.BeforeInteracts, () => OnClickRequest(_beforeInteractCtx) }, 
                          { OfficeState.AfterInteracts, EnterStage1},
                          { OfficeState.ReadyStage2, () => OnClickRequest(_afterStage1ClearCtx) },
                          { OfficeState.ReadyStage3, () => OnClickRequest(_afterStage2ClearCtx) }};
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

    private void OnClickStageStart(SceneData stage)
    {
        if (_isPlaying) return; // 중복 클릭 방지
        _co = StartCoroutine(RequestDirection(stage));
    }

    public void OnClickRequest(OfficeUIContext ctx)
    {
        OfficeUIController.Instance.ShowUI(ctx);
    }

    public void EnterStage1()
    {
        if (_isPlaying) return;
        ResultManager.instance.SetStage(1);
        OfficeUIController.Instance.HideUI();
        OnClickStageStart(SceneData.Stage1);
    }

    public void EnterStage2()
    {
        if (_isPlaying) return;
        ResultManager.instance.SetStage(2);
        OfficeUIController.Instance.HideUI();
        OnClickStageStart(SceneData.Stage2);
    }
    

    IEnumerator RequestDirection(SceneData nextStage)
    {
        _isPlaying = true;
        _requestClicked = false;

        // 1. 관련 UI 띄우기
        OfficeUIController.Instance.ShowUI(_afterInteractCtx);

        // 2. 2초 대기
        yield return new WaitForSeconds(1.5f);

        // 3. 대사 바꾸기
         normalText.SetActive(false);

        if (stamp.activeSelf == false) signText.SetActive(true); // 도장이 찍혀있으면(리플레이 시) 텍스트 off

        // 4. 의뢰서 클릭 전까지 대기
        yield return new WaitUntil(() => _requestClicked);

        // 5. 사인, 스탬프
        if (sign.activeSelf == false)
        {
            sign.SetActive(true);
            SFXPlayer.Instance.PlaySFX(signSound);
        }
        
        // + 사인 소리 

        // 6. 퀘스트 완료 이벤트 호출
        QuestEventManager.TriggerEvent(QuestEventId.freshman);
        
        // 7. 씬 넘어 가기 전 잠시 대기
        yield return new WaitForSeconds(3f);

        // 8. 씬 이동
        GameManager.Instance.MoveScene(nextStage);
    }

    public void RequestSign()
    {
        _requestClicked = true;
    }

    
    public void CheckCondition()
    {
        _clickedCount++;

        if (_clickedCount >= _needCount)
        {
            OfficeStateMachine.SetState(OfficeState.AfterInteracts);
        }
    }
}