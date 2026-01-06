using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class IntroPlayer : MonoBehaviour
{   
    [SerializeField] DialogueScript script;

    [Header("UI")]
    [SerializeField] private GameObject seniorPanel;
    [SerializeField] private TMP_Text seniorText;
    [SerializeField] private GameObject kPanel;
    [SerializeField] private TMP_Text kText;

    private int _index;

    void OnEnable()
    {
        if (OfficeStateMachine.currentState != OfficeState.Intro) return;
        _index = 0;
        showText(_index);
    }

    void showText(int index)
    {
        if (script.lines[index].speaker == Speaker.Senior)
        {   
            seniorPanel.SetActive(true);
            seniorText.text = script.lines[index].text;
        }

        else
        {
            kPanel.SetActive(true);
            kText.text = script.lines[index].text;
        }
    }
}
