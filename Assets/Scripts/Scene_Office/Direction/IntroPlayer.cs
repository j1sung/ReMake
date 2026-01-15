using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;

public enum IntroPhase
{
    TextIntro,
    Interview,
    TextOutro
}

public class IntroPlayer : MonoBehaviour
{   
    [SerializeField] private DialogueScript _introScript;
    [SerializeField] private DialogueScript _interviewScript;
    [SerializeField] private DialogueScript _outroScript;

    [Header("UI/CanvasGroup")]
    [SerializeField] private CanvasGroup _introCanvas; // 인트로 캔버스
    [SerializeField] private CanvasGroup _monoGroup; // 독백용 이미지
    [SerializeField] private CanvasGroup _introGroup; // 인트로 텍스트(k 독백)
    [SerializeField] private CanvasGroup _outroGroup; // 아웃트로 텍스트(나레이션)
    
    [Header("Text/Monologue")]
    [SerializeField] private TMP_Text _kMonologue;
    [SerializeField] private TMP_Text _narration;

    [Header("Text/Interview")]
    [SerializeField] private TMP_Text _seniorText;
    [SerializeField] private TMP_Text _kText;

    [Header("Fade")]
    [SerializeField] private float _fadeIn = 0.25f;
    [SerializeField] private float _fadeOut = 0.25f;
    [SerializeField] private float _hold = 2f;     // 글자 유지 시간

    [SerializeField] private GameObject interviewImg; // 인터뷰 이미지

    [SerializeField] private AudioClip textClip; // 텍스트 넘기는 소리

    private int _index;
    private IntroPhase _currentPhase;
    private TMP_Text _currentText;

    Coroutine _intro;
    Coroutine _outro;

    void Awake()
    {   
        if (OfficeStateMachine.currentState == OfficeState.Intro)
        {   
            _introCanvas.gameObject.SetActive(true);
        }
        else
        {
            _introCanvas.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        return;
    }

    void OnEnable()
    {
        if (OfficeStateMachine.currentState != OfficeState.Intro) return;

        _currentPhase = IntroPhase.TextIntro;
        _index = 0;
        _intro = StartCoroutine(IntroMonologue());
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (_currentPhase == IntroPhase.Interview)
            ShowDialogue(_index);
    }

    IEnumerator IntroMonologue()
    {   
        // 1. 1초 대기
        yield return new WaitForSeconds(1f);

        // 2. 대사 출력
        for (int i = 0; i < _introScript.lines.Count; i++)
        {
            _kMonologue.text = _introScript.lines[i].text;

            yield return Fade(_introGroup, 0f, 1f, _fadeIn);  // 페이드 인
            yield return new WaitForSeconds(_hold);               // 유지
            yield return Fade(_introGroup, 1f, 0f, _fadeOut); // 페이드 아웃
        }

        // 3. 2초 대기
        yield return new WaitForSeconds(2f);  

        // 4. introCanvas 페이드 아웃
        yield return Fade(_monoGroup, 1f, 0f, 1f); // 페이드 아웃

        // 5. 페이즈 변경
        _currentPhase = IntroPhase.Interview;

        // 6. 첫 대사만 출력
        ShowDialogue(_index);
    }

    IEnumerator OutroMonologue()
    {   
        // 1. 페이즈 변경 
        _currentPhase = IntroPhase.TextOutro;

        // 2. introCanvas 페이드 인
        yield return Fade(_monoGroup, 0f, 1f, _fadeIn);  // 페이드 인

        // 3. 인터뷰 이미지 끄기
        interviewImg.SetActive(false);

        // 3. 대사 출력
        for (int i = 0; i < _outroScript.lines.Count; i++)
        {
            _narration.text = _outroScript.lines[i].text;

            yield return Fade(_outroGroup, 0f, 1f, _fadeIn);  // 페이드 인
            yield return new WaitForSeconds(_hold);               // 유지
            yield return Fade(_outroGroup, 1f, 0f, _fadeOut); // 페이드 아웃
        }

        // 5. 잠시 대기
        yield return new WaitForSeconds(2f);

        // 6. 캔버스 페이드 아웃
        yield return Fade(_introCanvas, 1f, 0f, _fadeOut); // 페이드 아웃

        // 7. 캔버스 끄기
        _introCanvas.gameObject.SetActive(false);

        // 8. 상태 변경
        OfficeStateMachine.SetState(OfficeState.BeforeStart);

        // 9. Office BGM 다시 재생 요청
        BGMPlayer.Instance.gameObject.GetComponent<SceneBGMRouter>()?.ApplyCurrentSceneBGM();
    }

    // 인터뷰 대화 텍스트
    void ShowDialogue(int index)
    {   
        if (_currentText != null) 
            _currentText.gameObject.SetActive(false);

        if (_index >= _interviewScript.lines.Count) 
        {
            IntroEnd();
            return;
        }

        if (_interviewScript.lines[index].speaker == Speaker.Senior)
        {
            SFXPlayer.Instance.PlaySFX(textClip);
            _seniorText.text = _interviewScript.lines[index].text;
            _currentText = _seniorText;
        }
        else
        {
            SFXPlayer.Instance.PlaySFX(textClip);
            _kText.text = _interviewScript.lines[index].text;
            _currentText = _kText;
        }
        _currentText.gameObject.SetActive(true);
        _index++;
    }

    private void IntroEnd()
    {
        _currentText?.gameObject.SetActive(false);
        _outro = StartCoroutine(OutroMonologue());
    }
    IEnumerator Fade(CanvasGroup g, float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            g.alpha = to;
            yield break;
        }

        g.alpha = from;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            g.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        g.alpha = to;
    }
}
