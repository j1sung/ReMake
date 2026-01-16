using TMPro;
using UnityEngine;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    [Header("필수")]
    [SerializeField] private TextMeshProUGUI textUI;

    [Header("말풍선(확장 대상)")]
    [SerializeField] private RectTransform balloonRect;
    [SerializeField] private RectTransform textRect;

    [Header("레이아웃")]
    [SerializeField] private Vector2 minSize = new Vector2(160, 90);
    [SerializeField] private Vector2 padding = new Vector2(90, 60);
    [SerializeField] private float maxTextWidth = 420f;

    [Header("타이핑")]
    [SerializeField] private float typingDelay = 0.04f;

    [Header("말풍선 따라가기(연출)")]
    [SerializeField] private bool smoothResize = true;
    [SerializeField] private float resizeFollowSpeed = 12f;

    [SerializeField] private float singleLineHeightTrim = 4f; // 2~6 사이로 튜닝

    // ===== 추가된 핵심 =====
    public bool IsTyping { get; private set; }
    private System.Action _onCompleted;
    // ======================

    private Vector2 _currentSize;

    // ======= 사운드 추가 =======
    [SerializeField] private AudioClip _textClip;

    void Awake()
    {
        if (textRect != null)
            textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxTextWidth);

        if (textUI != null)
        {
            textUI.textWrappingMode = TextWrappingModes.Normal;
            textUI.overflowMode = TextOverflowModes.Overflow;
        }

        if (balloonRect != null)
            _currentSize = balloonRect.sizeDelta;
    }

    /// <summary>
    /// 문장 하나 타이핑 시작
    /// </summary>
    public void Type(string message, System.Action onCompleted)
    {
        StopAllCoroutines();
        SFXPlayer.Instance.StopLoop(); // 루프 소리 정리
        _onCompleted = onCompleted;
        StartCoroutine(StartType(message));
    }

    private IEnumerator StartType(string message)
    {
        if (textUI == null) yield break;

        IsTyping = true;
        textUI.text = "";

        SFXPlayer.Instance.PlayLoop(_textClip);

        if (balloonRect != null)
        {
            _currentSize = minSize;
            balloonRect.sizeDelta = _currentSize;
            ResizeBalloonImmediate();
        }

        try
        {
            foreach (char c in message)
            {
                if (!HangulUtil.IsHangul(c))
                {
                    textUI.text += c;
                    yield return ResizeDuringDelay();
                    continue;
                }

                HangulUtil.Decompose(c, out int cho, out int jung, out int jong);

                // 초성
                textUI.text += HangulUtil.CHO[cho];
                yield return ResizeDuringDelay();

                // 중성
                textUI.text = textUI.text[..^1];
                textUI.text += HangulUtil.Compose(cho, jung, 0);
                yield return ResizeDuringDelay();

                // 종성
                if (jong != 0)
                {
                    textUI.text = textUI.text[..^1];
                    textUI.text += HangulUtil.Compose(cho, jung, jong);
                    yield return ResizeDuringDelay();
                }
            }

            // 마지막 정렬
            if (balloonRect != null)
            {
                if (smoothResize) ResizeBalloonSmooth();
                else ResizeBalloonImmediate();
            }
        }
        finally
        {
            // 어떤 이유로든 코루틴이 끝나면 루프는 반드시 정지
            if (SFXPlayer.Instance != null)
                SFXPlayer.Instance.StopLoop();
        }

        IsTyping = false;
        _onCompleted?.Invoke();   //“타이핑 완료” 신호
    }

    // ----------------- 말풍선 확장 로직 -----------------

    IEnumerator ResizeDuringDelay()
    {
        if (balloonRect == null)
        {
            yield return new WaitForSeconds(typingDelay);
            yield break;
        }

        float t = 0f;
        while (t < typingDelay)
        {
            if (smoothResize) ResizeBalloonSmooth();
            else ResizeBalloonImmediate();

            t += Time.deltaTime;
            yield return null;
        }

        if (smoothResize) ResizeBalloonSmooth();
        else ResizeBalloonImmediate();
    }

    void ResizeBalloonImmediate()
    {
        Vector2 target = CalcTargetSize();
        balloonRect.sizeDelta = target;
        _currentSize = target;
    }

    void ResizeBalloonSmooth()
    {
        Vector2 target = CalcTargetSize();
        _currentSize = Vector2.Lerp(_currentSize, target, Time.deltaTime * resizeFollowSpeed);
        balloonRect.sizeDelta = _currentSize;
    }

    Vector2 CalcTargetSize()
    {
        textUI.ForceMeshUpdate();

        int lineCount = Mathf.Max(1, textUI.textInfo.lineCount);
        float lineHeight = textUI.textInfo.lineInfo[0].lineHeight;

        float h = lineCount * lineHeight + padding.y;

        if (lineCount == 1)
            h -= singleLineHeightTrim; // 1줄만 살짝 줄이기

        h = Mathf.Max(minSize.y, h);

        // 가로는 기존대로
        Vector2 preferred = textUI.GetPreferredValues(textUI.text, maxTextWidth, 0);
        float w = Mathf.Max(minSize.x, preferred.x + padding.x);
        w = Mathf.Min(w, maxTextWidth + padding.x);

        return new Vector2(w, h);
    }
}