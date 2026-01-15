using UnityEngine;
using System.Collections;

public class Telephone : OfficeInteractable
{
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] DialogueController dialogue;
    [SerializeField] AudioClip phoneRing;
    [SerializeField] AudioClip callWait;

    [SerializeField] private CallScript _tutorial;
    [SerializeField] private CallScript _stage1Clear;

    [SerializeField] private OfficeUIContext _beforeInteractCtx;
    [SerializeField] private OfficeUIContext _afterStage1Ctx;

    private bool _isCalling;

    void Awake()
    {
        actions = new(){{OfficeState.BeforeCall, () => OnReceiveCall(_tutorial)},
                        {OfficeState.BeforeInteracts, () => OnMissedCall(_beforeInteractCtx, OfficeState.BeforeInteracts)},
                        {OfficeState.AfterInteracts, () => OnMissedCall(_beforeInteractCtx, OfficeState.AfterInteracts)},
                        {OfficeState.Stage1Clear, () => OnReceiveCall(_stage1Clear)},
                        {OfficeState.ReadyStage2, () => OnMissedCall(_afterStage1Ctx, OfficeState.ReadyStage2)},
                        {OfficeState.ReadyStage3, () => OnMissedCall(_afterStage1Ctx, OfficeState.ReadyStage3)}};
    }

    private void OnReceiveCall(CallScript script)
    {
        // if (phoneRing.isPlaying) phoneRing.Stop();
        OfficeStateMachine.SetState(OfficeState.Calling);
        SpeechBubble.SetActive(true);
        dialogue.Play(script);
    }

    private void OnMissedCall(OfficeUIContext ctx, OfficeState returnState)
    {
        if (_isCalling) return;
        _isCalling = true;
        OfficeStateMachine.SetState(OfficeState.Calling);
        StartCoroutine(CallSequence(ctx, returnState));
    }

    private IEnumerator CallSequence(OfficeUIContext ctx, OfficeState returnState)
    {
        SFXPlayer.Instance.PlaySFX(callWait);

        yield return new WaitForSeconds(callWait.length);
        Debug.Log($"[CallSequence] wait = {callWait?.length}");

        OfficeUIController.Instance.ShowUI(ctx);

        OfficeStateMachine.SetState(returnState);
        _isCalling = false;
    }
}
