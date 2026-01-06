using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDB", menuName = "Scriptable Objects/QuestDB")]
public class QuestDB : ScriptableObject
{
    [SerializeField] private List<QuestData> allQuests; // 편집용

    private Dictionary<QuestEventId, QuestData> map; // 검색용(캐싱)

    private void OnEnable()
    {
        BuildDictionary();
    }

    // 캐싱용 딕셔너리 채우기
    private void BuildDictionary()
    {
        map = new Dictionary<QuestEventId, QuestData>();

        foreach (var q in allQuests)
        {
            if(q == null) continue;
            if (!map.TryAdd(q.id, q))
                Debug.LogWarning($"중복 Quest ID 발견: {q.id}");
        }
    }
    public QuestData FindById(QuestEventId id)
    {
        if (map == null) BuildDictionary();
        map.TryGetValue(id, out QuestData quest);
        return quest;
    }
}
