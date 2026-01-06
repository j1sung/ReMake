using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopupView_title : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Audios")]
    [SerializeField] private AudioClip x_ButtonClickClip;  // X 버튼 클릭 소리

    private Action _onYes;
    private Action _onNo;

    public void Open(Action onYes, Action onNo = null)
    {
        _onYes = onYes;
        _onNo = onNo;

        // YES 버튼이 존재하면 연결
        if (yesButton != null) 
        {
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() =>
            {
                var action = _onYes;
                Close();
                action?.Invoke();
            });
        }

        // NO 버튼이 존재하면 연결
        if (noButton != null) 
        {
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() =>
            {
                var action = _onNo;
                Close();
                action?.Invoke();
            });
        }

        gameObject.SetActive(true);
    }

    public void Close()
    {
        SFXPlayer.Instance.PlaySFX(x_ButtonClickClip);
        _onYes = null;
        _onNo = null;
        gameObject.SetActive(false);  
    }
}
