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
    public Sprite albumImage;
    [TextArea] public string comment;
}

[Serializable]
public class EndingEntry
{
    // 조합 키(오름차순 정렬된 타입만 +로 연결, 예: "Clothes+Letter")
    public string comboKey;
    public EndingOutcome outcome;
}

public class ResultManager : MonoBehaviour
{
    public static ResultManager instance { get; private set; }

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

    // 스테이지 퍼즐 최대 갯수
    int stage1Max = 6;

    // 현재 스테이지 정보
    int currentStageInfo; // GameManager에서 받아와야함

    public int CurrentStageInfo => currentStageInfo;
    

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
    public void SetResults(List<ObjeData> obje)
    {
        submitItems = obje;
        Debug.Log($"submitItems={string.Join(" ", submitItems.Select(item => item.itemType))}");
        Evaluate();
    }

    public void ClearResults() => submitItems.Clear();

    // 결과 분기 평가
    public void Evaluate()
    {
        // 제출이 비었으면 EMPTY 키로 조회
        if (submitItems == null || submitItems.Count == 0)
            FindOutcomeByKey("EMPTY");

        // 퍼즐 조합 키 만들기(오름차순 정렬)
        var uniqueTypes = submitItems
            .Select(i => i.itemType)
            .Distinct()
            .OrderBy(s => s);
        
        string key = string.Join("+", uniqueTypes); // 예: "Clothes+Letter

        FindOutcomeByKey(key);
    }

    private void FindOutcomeByKey(string key)
    {
        var entry = endingTable.FirstOrDefault(e => e.comboKey == key);
        ClearResults();
        if (entry != null)
        {
            endingOutcomes.Add(entry.outcome); // 스테이지 엔딩 저장
            Debug.Log($"[Ending] key={key} -> {entry.outcome.endingId}");
            Debug.Log($"endingOutcomes={string.Join(" ", endingOutcomes[0].endingId)}");
            return;
        }
        endingOutcomes.Add(defaultOutcome); // 조합이 없으면 기본 엔딩
    }
}
