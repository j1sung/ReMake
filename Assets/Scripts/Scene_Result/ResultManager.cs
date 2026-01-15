using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
[Serializable]
public class EndingObje
{
    public List<ObjeData> objs;
}

public class ResultManager : MonoBehaviour
{
    public static ResultManager instance { get; private set; }

    public GameObject resultCanvas; // 앨범 캔버스 넣기
    public ResultAlbumUI resultAlbumUI; 
    public ResultQuestUI resultQuestUI;
    private QuestPopup questPopup;

    // 임시 엔딩 크래딧 상태
    public bool IsFirstCredit {  get; set; } = false;

    // 현재 스테이지 정보
    [SerializeField] private int currentStageInfo = 1; // GameManager에서 받아와야함
    public int CurrentStageInfo => currentStageInfo; // GameManager으로 나중에 옮김

    // 오브제 리스트 캐시
    public List<ObjeData> Objes { get; private set; } = new List<ObjeData>();

    // 스테이지별 결과 저장(필수)
    [Header("Ending Obje")]
    public List<EndingObje> endingObjes = new List<EndingObje>();

    [Header("Ending Result")]
    public List<ResultData> endingResult = new List<ResultData>();

    // 업적 달성 저장(필수)
    [Header("Unlocked Quest Result")]
    public HashSet<QuestEventId> unlockedQuests = new HashSet<QuestEventId>(); // 중복 방지용
    public List<QuestData> unlockedQuestList = new List<QuestData>(); // UI 표시용

    [Header("All Obje List")]
    [SerializeField] private ObjeDB objeDB;

    [Header("All Result List")]
    [SerializeField] private ResultDB resultDB;

    [Header("All Quest List")]
    [SerializeField] private QuestDB questDB;

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

        questPopup = GetComponent<QuestPopup>();
    }

    private void OnEnable()
    {
        QuestEventManager.OnEventTriggered += OnGameEvent;
    }

    private void OnDisable()
    {
        QuestEventManager.OnEventTriggered -= OnGameEvent;
    }

    private void OnDestroy()
    {
        QuestEventManager.OnEventTriggered -= OnGameEvent;
    }

    private void OnGameEvent(QuestEventId eventId)
    {
        Debug.Log($"[ResultManager] 이벤트 수신: {eventId}");

        // 이미 달성된 퀘스트라면 무시
        if (unlockedQuests.Contains(eventId))
        {
            Debug.Log($"[ResultManager] 중복 업적 무시: {eventId}");
            return;
        }
        // 중복 검사용 해시셋에 기록
        unlockedQuests.Add(eventId);

        QuestData quest = questDB.FindById(eventId);

        // 업적 팝업 띄우기
        questPopup.EnablePopup(quest);

        // UI용 리스트에 추가
        unlockedQuestList.Add(quest);

        Debug.Log($"[ResultManager] 새 업적 달성: {quest.qName}");

        // 효과음 재생
    }


    public void Initialize()
    {
        IsFirstCredit = false;
        currentStageInfo = 1;
        endingObjes.Clear();
        endingResult.Clear();
        unlockedQuests.Clear();
        unlockedQuestList.Clear();

        resultAlbumUI.ResetAlbumUI();
        resultQuestUI.ResetQuestUI();
    }

    // ==== Save/Load 기능들 ====
    public void SetStage(int stage)
    {
        currentStageInfo = stage;
    }

    public void SetResult(List<string> resultIds)
    {
        endingResult.Clear();
        if(resultIds == null) return;

        for(int i =0; i < resultIds.Count; i++)
        {
            AddEndingResultAtIndex(i, resultDB.FindById(resultIds[i]));
        }
    }

    // ==== Obje 추가 로직 ====
    public void AddEndingObje(List<ObjeData> objs)
    {
        int index = currentStageInfo - 1;
        AddEndingObjeAtIndex(index, objs);
    }
    public void AddEndingObjeAtIndex(int index,  List<ObjeData> objs)
    {
        if (index < 0) return;

        // null이면 빈 리스트로 치환
        if (objs == null)
           objs = new List<ObjeData>();

        while (endingObjes.Count <= index)
            endingObjes.Add(null);

        if (endingObjes[index] == null)
            endingObjes[index] = new EndingObje { objs = new List<ObjeData>() };

        List<ObjeData> target = endingObjes[index].objs;

        for(int i=0; i< objs.Count; i++)
        {
            ObjeData obj = objs[i];
            if(obj == null) continue;

            // 중복 방지해서 넣기
            bool exists = target.Exists(x => x != null &&
                string.Equals(x.id, obj.id, StringComparison.OrdinalIgnoreCase));
            if(!exists) target.Add(obj);
        }
    }

    public void SetObje(List<StageObjeSave> datas)
    {
        endingObjes.Clear();

        if (datas == null) return;

        for (int i = 0; i < datas.Count; i++)
        {
            StageObjeSave stageSave = datas[i];
            List<ObjeData> restoredObjs = new List<ObjeData>();

            if (stageSave?.objeIds != null)
            {
                for (int j = 0; j < stageSave.objeIds.Count; j++)
                {
                    string id = stageSave.objeIds[j];
                    ObjeData obje = objeDB.FindById(id);   // ResultManager 내부니까 접근 가능
                    if (obje != null)
                        restoredObjs.Add(obje);
                }
            }
            // Obje 적용
            AddEndingObjeAtIndex(i, restoredObjs);
        }
    }

    public void SetQuest(List<QuestEventId> questIds)
    {
        unlockedQuests.Clear();
        unlockedQuestList.Clear();

        if (questIds == null) return;

        for (int i = 0; i < questIds.Count; i++)
        {
            QuestEventId id = questIds[i];

            // 중복 방지 (List에서 중복 들어와도 안전)
            if (!unlockedQuests.Add(id))
                continue;

            QuestData quest = questDB.FindById(id);
            if (quest != null)
            {
                unlockedQuestList.Add(quest);
            }
            else
            {
                Debug.LogWarning($"[ResultManager] Quest not found: {id}");
            }
        }
    }

    // 스테이지 변수값 증가 -> 일단 ResultManager에 넣고 나중에 GameManager에 옮김
    public void SetNextStage()
    {
        currentStageInfo++;
        Debug.Log($"[Stage] CurrentStage = {currentStageInfo}");
    }


    // 제출한 결과 넘겨받기
    public void ProcessStageResult(List<ObjeData> objes, bool isFull)
    {
        Debug.Log($"objes={string.Join(" ", objes.Select(item => item.id))}");

        // Obje 캐시 등록
        Objes = new List<ObjeData>(objes);

        // EMPTY 엔딩 체크
        if (objes == null || objes.Count == 0)
        {
            FindOutcomeByKey_Special("EMPTY");
            AddEndingObje(objes);
            return;
        }
        
        
         // 균형 엔딩 체크 -> 타입이 추가되거나 바뀌게 되면 구조가 좀 바뀌어야 함!
         bool hasMemory = objes.Any(i => i.itemType == "memory");
         bool hasPresent = objes.Any(i => i.itemType == "present");
         bool hasAttachment = objes.Any(i => i.itemType == "attachment");

         if (objes.Count == 3 && hasMemory && hasPresent && hasAttachment)
         {
            FindOutcomeByKey_Special("Balance");
            AddEndingObje(objes);
            return;
         }

         if (isFull) // Full 엔딩 체크
         {
            FindOutcomeByKey_Special("Full");
            AddEndingObje(objes);
            return;
         }

        // 일반 케이스: 제출 ids를 Set으로 넘김
        HashSet<string> submittedIds = new HashSet<string>(objes.Select(o => o.id), StringComparer.OrdinalIgnoreCase);

        FindOutcomeByKey(submittedIds);

        // 오브젝트 결과 저장
        AddEndingObje(objes);
    }

    // special 키 판별
    private void FindOutcomeByKey_Special(string key)
    {
        StageInfo stage = GetStageInfo();
        // 키로 찾기
        ResultData result = resultDB.FindByStageWithFallback(stage, key);

        // SO 결과 저장(스테이지 엔딩 결과)
        AddEndingResult(result);

        Debug.Log($"[Ending] stage={stage} key={key} -> endingId={result.endingName}");
    }

    // 결과 키 판별 -> 데이터 저장
    private void FindOutcomeByKey(HashSet<string> submittedIds)
    {
        StageInfo stage = GetStageInfo();
        // 키로 찾기
        ResultData result = resultDB.FindByStageBySubset(stage, submittedIds);

        // SO 결과 저장(스테이지 엔딩 결과)
        AddEndingResult(result);

        Debug.Log($"[Ending] stage={stage} submitted=[{string.Join(",", submittedIds)}] -> endingId={result.endingName}");
    }

    // Result 추가 로직
    private void AddEndingResult(ResultData result)
    {
        int index = currentStageInfo - 1;
        AddEndingResultAtIndex(index, result);
    }

    private void AddEndingResultAtIndex(int index, ResultData result)
    {
        if (index < 0) return;
        if(result == null) return;

        while (endingResult.Count <= index)
            endingResult.Add(null);

        endingResult[index] = result;
    }

    // StageInfo -> Enum 타입으로 변환
    private StageInfo GetStageInfo()
    {
        // stage1=0, stage2=1, stage3=2
        int idx = Mathf.Clamp(currentStageInfo - 1, 0, Enum.GetValues(typeof(StageInfo)).Length - 1);
        return (StageInfo)idx;
    }

    public void UnlockQuest(QuestData quest)
    {
        if (quest == null || unlockedQuests.Contains(quest.id))
            return;
        unlockedQuests.Add(quest.id);
        unlockedQuestList.Add(quest);
    }
}
