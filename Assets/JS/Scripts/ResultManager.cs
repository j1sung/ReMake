using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using System.Linq;
using System;

[Serializable]
public class EndingOutcome
{
    public string endingId;
    public int score;
    public Sprite albumImage;
    [TextArea] public string comment;

    public List<ObjeData> objeDatas; // 제출한 오브제 데이터들
}

[Serializable]
public class EndingEntry
{
    // 조합 키(오름차순 정렬된 타입만 +로 연결, 예: "Clothes+Letter")
    public string[] comboKeys;
    public EndingOutcome outcome;
}

public class ResultManager : MonoBehaviour
{
    public static ResultManager instance { get; private set; }

    // 현재 스테이지 정보
    [SerializeField] private int currentStageInfo = 1; // GameManager에서 받아와야함
    public int CurrentStageInfo => currentStageInfo; // GameManager으로 나중에 옮김

    // 슬롯 꽉 채웠는지 확인 변수
    bool isFull;

    // 제출한 퍼즐 데이터 임시 저장
    [Header("Submit Puzzle Data")]
    public List<ObjeData> submitItems = new List<ObjeData>();

    // 엔딩 결과 목록 만들기 -> 추후 다른곳으로 옮겨야할듯
    [Header("Ending Table")]
    public EndingOutcome defaultOutcome;
    public List<EndingEntry> endingTable = new List<EndingEntry>();

    // 스테이지별 결과 저장(필수)
    [Header("Ending Result")]
    public List<EndingOutcome> endingOutcomes = new List<EndingOutcome>();
    
    // 업적 달성 저장(필수)

    private void Awake()
    {
        // 싱글톤 보장
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 스테이지 변수값 증가 -> 일단 ResultManager에 넣고 나중에 GameManager에 옮김
    public void SetNextStage()
    {
        currentStageInfo++;
        Debug.Log($"[Stage] CurrentStage = {currentStageInfo}");
    }


    // 제출한 결과 넘겨받기
    public void SetResults(List<ObjeData> obje, bool isFull)
    {
        submitItems = obje;
        this.isFull = isFull;
        Debug.Log($"submitItems={string.Join(" ", submitItems.Select(item => item.itemName))}");
        Evaluate();
    }
    // 넘겨받은 제출 결과 클리어
    public void ClearResults() => submitItems.Clear();

    // 결과 분기 평가
    public void Evaluate()
    {
        // EMPTY 엔딩 체크
        if (submitItems == null || submitItems.Count == 0)
        {
            FindOutcomeByKey("EMPTY");
            return; // 비었으면 종료
        }

        // 균형 엔딩 체크
        var hasMemory = submitItems.Any(i => i.itemType == "memory");
        var hasPresent = submitItems.Any(i => i.itemType == "present");
        var hasAttachment = submitItems.Any(i => i.itemType == "attachment");

        if (submitItems.Count == 3 && hasMemory && hasPresent && hasAttachment)
        {
            FindOutcomeByKey("Balance");
            return; // 균형이면 종료
        }

        // Full 엔딩 체크
        if (isFull)
        {
            FindOutcomeByKey("Full");
            return; // 꽉 채우면 종료
        }

        // 퍼즐 조합 키 만들기(오름차순 정렬)
        var uniqueTypes = submitItems
            .Select(i => i.itemName)
            .Distinct()
            .OrderBy(s => s);
        
        string key = string.Join("+", uniqueTypes); // 예: "Clothes+Letter

        FindOutcomeByKey(key);
    }

    // 결과 키 판별 -> 데이터 저장
    private void FindOutcomeByKey(string key)
    {
        EndingEntry entry = endingTable.FirstOrDefault(e => 
        e.comboKeys.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase)));
        
        if (entry != null)
        {
            entry.outcome.objeDatas = new List<ObjeData>(submitItems);
            ClearResults();
            endingOutcomes.Add(entry.outcome); // 스테이지 엔딩 저장
            Debug.Log($"[Ending] key={key} -> {entry.outcome.endingId}");
            Debug.Log($"endingOutcomes={string.Join(" ", endingOutcomes[0].endingId)}");
            return;
        }
        defaultOutcome.objeDatas = new List<ObjeData>(submitItems);
        ClearResults();
        endingOutcomes.Add(defaultOutcome); // 조합이 없으면 기본 엔딩
    }
}
