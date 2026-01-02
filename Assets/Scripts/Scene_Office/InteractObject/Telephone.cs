using UnityEngine;

public class Telephone : OfficeInteractable
{
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] DialogueController dialogue;
    // [SerializeField] AudioClip phoneRing;

    [SerializeField] private OfficeUIContext _beforeInteractCtx;

    void Awake()
    {
        actions = new(){{OfficeState.BeforeCall, OnReceiveCall}, 
                        {OfficeState.BeforeInteracts, OnMissedCall}};
    }

    private void OnReceiveCall()
    {
        // if (phoneRing.isPlaying) phoneRing.Stop();
        SpeechBubble.SetActive(true);
        dialogue.PlayCurrent();
    }

    private void OnMissedCall()
    {
        // SFXPlayer.Instance.PlaySFX(phoneRing);
        OfficeUIController.Instance.ShowUI(_beforeInteractCtx);
    }
}
