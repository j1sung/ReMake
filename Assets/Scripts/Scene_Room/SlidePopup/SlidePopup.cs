using UnityEngine;
using System.Collections;

public enum PopupContent
{
    None,
    Manual,
    Inspect,
    Collection
}

public class SlidePopup : MonoBehaviour
{
    [Header("Slide")]
    [SerializeField] RectTransform panel;
    [SerializeField] GameObject blocker;
    [SerializeField] Vector2 openPos;
    [SerializeField] Vector2 closedPos;
    [SerializeField] float duration = 0.5f;

    [Header("Contents")]
    [SerializeField] GameObject manualPanel;
    [SerializeField] GameObject inspectPanel;
    [SerializeField] GameObject collectionPanel;

    PopupContent currentContent = PopupContent.None;
    bool isInspectSessionActive;

    bool isOpen;
    bool isAnimating;
    Coroutine co;

    void Awake()
    {
        panel.anchoredPosition = closedPos;
        SetBlocker(false);
        SetAllContents(false);
        SetContent(PopupContent.Manual);

        isOpen = false;
        isAnimating = false;
        isInspectSessionActive = false;
    }

    // 버튼 토글
    public void Toggle(PopupContent target)
    {
        if (isAnimating) return;

        if (target == PopupContent.Inspect) // Inspect 상태인 동안은 아래 변수 true
        {
            isInspectSessionActive = true;
        }

        // '유품 조사' 버튼 눌렀는데 is~변수가 켜져있으면 Inspect로 변경
        if (target == PopupContent.Collection && isInspectSessionActive)
        {
            target = PopupContent.Inspect;
        }

        if (isOpen)
        {
            if (currentContent == target)
            {
                Close();
            }
            else
            {
                SetContent(target);
            }
            return;
        }

        Open(target);
    }

    public void Open(PopupContent target)
    {
        if (isOpen) return;

        StopCo();
        SetContent(target);

        SetBlocker(true);
        isOpen = true;
        isAnimating = true;

        co = StartCoroutine(OpenPanel());
    }

    public void Close()
    {
        if (!isOpen) return;

        StopCo();
        isOpen = false;
        isAnimating = true;

        co = StartCoroutine(ClosePanel());
    }

    IEnumerator OpenPanel()
    {
        yield return Slide(openPos);
        isAnimating = false;
    }

    // 패널 close 코루틴
    IEnumerator ClosePanel()
    {
        yield return Slide(closedPos);

        SetBlocker(false);

        if (currentContent == PopupContent.Inspect)
            isInspectSessionActive = false;

        isAnimating = false;
    }

    // 슬라이드 코루틴
    IEnumerator Slide(Vector2 target)
    {
        Vector2 start = panel.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            panel.anchoredPosition = Vector2.Lerp(start, target, eased);
            yield return null;
        }

        panel.anchoredPosition = target;
    }

    // 이미지 변경
    void SetContent(PopupContent target)
    {
        SetAllContents(false);

        currentContent = target;

        switch (target)
        {
            case PopupContent.Manual:
                manualPanel.SetActive(true);
                break;

            case PopupContent.Inspect:
                inspectPanel.SetActive(true);
                break;

            case PopupContent.Collection:
                collectionPanel.SetActive(true);
                break;
        }
    }

    void SetAllContents(bool value)
    {
        manualPanel.SetActive(value);
        inspectPanel.SetActive(value);
        collectionPanel.SetActive(value);
    }

    void SetBlocker(bool value)
    {
        if (blocker)
            blocker.SetActive(value);
    }

    void StopCo()
    {
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    public void ToggleCollection()
    {
        Toggle(PopupContent.Collection);
    }

    public void ToggleManual()
    {
        Toggle(PopupContent.Manual);
    }
}