using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultManager : MonoBehaviour
{
    public static ResultManager instance { get; private set; }

    public GameObject resultCanvas; // 앨범 캔버스 넣기
    public ResultAlbumUI resultAlbumUI; 
    public ResultQuestUI resultQuestUI;

    // 현재 스테이지 정보
    [SerializeField] private int currentStageInfo = 1; // GameManager에서 받아와야함
    public int CurrentStageInfo => currentStageInfo; // GameManager으로 나중에 옮김

    // 엔딩 결과 목록 만들기 -> 추후 다른곳으로 옮겨야할듯
    [Header("Ending Table")]
    public List<EndingEntry> endingTable = new List<EndingEntry>();

    // 스테이지별 결과 저장(필수)
    [Header("Ending Result")]
    public List<EndingOutcome> endingOutcomes = new List<EndingOutcome>(); // 나중에는 스크립터블 오브젝트나 다른 데이터 저장소로 옮겨야될듯

    [Header("All Quest List")]
    //public List<QuestData> allQuestList = new List<QuestData>();
    [SerializeField] private QuestDB questDB;

    // 업적 달성 저장(필수)
    [Header("Unlocked Quest Result")]
    private HashSet<string> unlockedQuests = new HashSet<string>(); // 중복 방지용
    public List<QuestData> unlockedQuestList = new List<QuestData>(); // UI 표시용

    private void Awake()
    {
        // 싱글톤 보장
        if (instance != null && instance != this)
        {
            Destroy(resultCanvas); // 삭제는 필수! 삭제 순서도 중요!
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if(resultCanvas != null)
        {
            DontDestroyOnLoad (resultCanvas);
        }
    }

    private void OnEnable()
    {
        QuestEventManager.OnEventTriggered += OnGameEvent;
    }

    private void OnDisable()
    {
        QuestEventManager.OnEventTriggered -= OnGameEvent;
    }

    private void OnGameEvent(string eventName)
    {
        Debug.Log($"[ResultManager] 이벤트 수신: {eventName}");

        // 이미 달성된 퀘스트라면 무시
        if (unlockedQuests.Contains(eventName))
        {
            Debug.Log($"[ResultManager] 중복 업적 무시: {eventName}");
            return;
        }

        QuestData quest = questDB.FindById(eventName);
        
        // 업적 팝업 띄우기
        GetComponent<QuestPopup>().EnablePopup(quest);

        // UI용 리스트에 추가
        unlockedQuestList.Add(quest);

        // 중복 검사용 해시셋에 기록
        unlockedQuests.Add(eventName);

        Debug.Log($"[ResultManager] 새 업적 달성: {quest.qName}");

        // 효과음 재생

        /*
        // allQuestList 중에서 triggerName이 같은 퀘스트를 찾는다
        foreach (var quest in allQuestList)
        {
            if (quest.qID == eventName)
            {
                // 업적 팝업 띄우기
                GetComponent<QuestPopup>().EnablePopup(quest);

                // UI용 리스트에 추가
                unlockedQuestList.Add(quest);

                // 중복 검사용 해시셋에 기록
                unlockedQuests.Add(eventName);

                Debug.Log($"[ResultManager] 새 업적 달성: {quest.qName}");

                // 효과음 재생
            }
        }
        */
    }


    public void Initialized()
    {
        currentStageInfo = 1;
        endingOutcomes.Clear();
        unlockedQuests.Clear();
        unlockedQuestList.Clear();

        resultAlbumUI.ResetAlbumUI();
        resultQuestUI.ResetQuestUI();
    }

    // 스테이지 변수값 증가 -> 일단 ResultManager에 넣고 나중에 GameManager에 옮김
    public void SetNextStage()
    {
        currentStageInfo++;
        Debug.Log($"[Stage] CurrentStage = {currentStageInfo}");
    }


    // 제출한 결과 넘겨받기
    public void SetResults(List<ObjeData> objes, bool isFull)
    {
        Debug.Log($"objes={string.Join(" ", objes.Select(item => item.itemName))}");

        // EMPTY 엔딩 체크
        if (objes == null || objes.Count == 0)
        {
            FindOutcomeByKey("EMPTY", objes);
            return; // 비었으면 종료
        }

        // 균형 엔딩 체크
        var hasMemory = objes.Any(i => i.itemType == "memory");
        var hasPresent = objes.Any(i => i.itemType == "present");
        var hasAttachment = objes.Any(i => i.itemType == "attachment");

        if (objes.Count == 3 && hasMemory && hasPresent && hasAttachment)
        {
            FindOutcomeByKey("Balance", objes);
            return; // 균형이면 종료
        }

        // Full 엔딩 체크
        if (isFull)
        {
            FindOutcomeByKey("Full", objes);
            return; // 꽉 채우면 종료
        }

        // 퍼즐 조합 키 만들기(오름차순 정렬)
        var uniqueTypes = objes
            .Select(i => i.itemName)
            .Distinct()
            .OrderBy(s => s);

        string key = string.Join("+", uniqueTypes); // 예: "Clothes+Letter

        FindOutcomeByKey(key, objes);
    }

    // 결과 키 판별 -> 데이터 저장
    private void FindOutcomeByKey(string key, List<ObjeData> objes)
    {
        EndingEntry entry = endingTable.FirstOrDefault(e => 
        e.comboKeys.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase)));
        
        if (entry == null)
        {
            entry = endingTable.FirstOrDefault(e =>
                    e.comboKeys.Any(k => string.Equals(k, "Default", StringComparison.OrdinalIgnoreCase)));
        }

        entry.outcome.objeDatas = objes; // 제출 오브제 데이터 저장
        endingOutcomes.Add(entry.outcome); // 스테이지 엔딩 저장

        Debug.Log($"[Ending] key={key} -> {entry.outcome.endingId}");
        Debug.Log($"endingOutcomes={string.Join(" ", endingOutcomes[0].endingId)}");
    }

    public void UnlockQuest(QuestData quest)
    {
        if (quest == null || unlockedQuests.Contains(quest.qID))
            return;
        unlockedQuests.Add(quest.qID);
        unlockedQuestList.Add(quest);
    }
}
