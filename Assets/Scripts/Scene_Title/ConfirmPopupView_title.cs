using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopupView_title : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Audios")]
    [SerializeField] private AudioClip y_ButtonClickClip; // Y 버튼 클릭 소리
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
                SFXPlayer.Instance.PlaySFX(y_ButtonClickClip);
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
                SFXPlayer.Instance.PlaySFX(x_ButtonClickClip);
                Close();
                action?.Invoke();
            });
        }

        gameObject.SetActive(true);
    }

    public void Close()
    {
        _onYes = null;
        _onNo = null;
        gameObject.SetActive(false);  
    }
}
