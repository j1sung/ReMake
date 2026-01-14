using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TypingEffect typing;
    [SerializeField] private GameObject speechBubble;

    CallScript currentScript;
    int index;
    bool canClick;

    void Update()
    {
        if (!canClick) return;
        if (Input.GetMouseButtonDown(0))
            Next();
    }

    public void Play(CallScript script)
    {
        currentScript = script;
        index = 0;
        canClick = false;
        speechBubble.SetActive(true);
        PlayCurrent();
    }

    void PlayCurrent()
    {
        canClick = false;
        typing.Type(currentScript.lines[index], EnableClick);
    }

    void EnableClick()
    {
        canClick = true;
    }

    void Next()
    {
        index++;

        if (index < currentScript.lines.Length)
            PlayCurrent();
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        speechBubble.SetActive(false);
        OfficeStateMachine.SetState(currentScript.nextStateAfterCall);
    }
}