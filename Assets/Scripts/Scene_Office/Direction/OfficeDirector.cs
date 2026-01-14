using UnityEngine;
using System.Collections;
using UnityEditor.Profiling.Memory.Experimental;

/// Office 씬 연출 전담
/// - 씬 진입 2초 후 전화벨/전화기 ON 연출
/// - 상태 변화(전화 받음/조사 시작/조사 완료)에 따라 방 이미지 전환
public class OfficeDirector : MonoBehaviour
{
    [Header("Room Image")]
    [SerializeField] GameObject roomIdle;      // 기본 방
    [SerializeField] GameObject roomPhoneOn;   // 전화기 불/벨 울림 방
    [SerializeField] GameObject roomBackground; // UI 모드

    [Header("Audio")]
    [SerializeField] AudioSource phoneRing;

    Coroutine _introCo;

    void OnDisable()
    {
         OfficeStateMachine.OnStateChanged -= HandleStateChanged;

        if (_introCo != null)
        {
            StopCoroutine(_introCo);
            _introCo = null;
        }
    }

    void Start()
    {   
        // OnStateChanged 이벤트 구독
        OfficeStateMachine.OnStateChanged += HandleStateChanged;

        // 씬 시작 시 기본 방 표시
        HandleStateChanged(OfficeStateMachine.currentState);
        Debug.Log("현재 상태" + OfficeStateMachine.currentState);

    }

    IEnumerator OfficeIntro()
    {
        // 기본 상태에서만 인트로 실행
        if (OfficeStateMachine.currentState != OfficeState.BeforeStart)
            yield break;

        yield return new WaitForSeconds(2f);

        // 여전히 BeforeCall이면 전화벨 연출
        if (OfficeStateMachine.currentState == OfficeState.BeforeStart)
        {
            OfficeStateMachine.SetState(OfficeState.BeforeCall);
            if (phoneRing != null) phoneRing.Play();
        }

        _introCo = null;
    }

    void HandleStateChanged(OfficeState state)
    {   
        switch (state)
        {   
            case OfficeState.BeforeStart:
                // 인트로 연출 시작
                SetRoomImage(roomIdle);
                _introCo = StartCoroutine(OfficeIntro());
                break;

            case OfficeState.BeforeCall:
                // 전화 대기 상태 → 전화기 켜진 방
                SetRoomImage(roomPhoneOn);
                if (phoneRing != null && !phoneRing.isPlaying)
                    phoneRing.Play();
                break;

            case OfficeState.BeforeInteracts or OfficeState.ReadyStage2:
                // 전화 종료 후 → 기본 방
                SetRoomImage(roomIdle);
                break;

            case OfficeState.AfterInteracts:
                // 조사 완료 후 → 기본 방 유지
                SetRoomImage(roomIdle);
                break;

            case OfficeState.Calling:
                StopRing();
                SetRoomImage(roomPhoneOn);
                break;

            case OfficeState.Stage1Clear:
                SetRoomImage(roomPhoneOn);
                if (phoneRing != null && !phoneRing.isPlaying)
                    phoneRing.Play();
                break;
                   
            default:
                return;
        }
    }

    void StopRing()
    {
        if (phoneRing != null && phoneRing.isPlaying)
            phoneRing.Stop();
    }

    void SetRoomImage(GameObject activeRoom)
    {
        if (roomIdle != null) roomIdle.SetActive(activeRoom == roomIdle);
        if (roomPhoneOn != null) roomPhoneOn.SetActive(activeRoom == roomPhoneOn);
        if (roomBackground != null) roomBackground.SetActive(activeRoom == roomBackground);
    }
}