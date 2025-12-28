using UnityEngine;

public class Telephone : OfficeInteractable
{
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] DialogueController dialogue;
    // [SerializeField] AudioSource phoneRing;

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
        Debug.Log("부재중 전화");
    }
}
