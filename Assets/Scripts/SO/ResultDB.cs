using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "ResultDB", menuName = "Scriptable Objects/ResultDB")]
public class ResultDB : ScriptableObject
{
    [SerializeField] private List<ResultDBList> allResult; // 편집용
    private Dictionary<string, ResultData> map; // 검색용

    private void OnEnable() => BuildDictionary();

    private void BuildDictionary()
    {
        map = new Dictionary<string, ResultData>();
        foreach (var stageList in allResult)
        {
            if (stageList == null) continue;

            foreach (var res in stageList.list)
            {
                if (res == null) continue;

                if (!map.TryAdd(res.id, res))
                    Debug.LogWarning($"[ResultDB] 중복 Result ID: {res.id}");
            }
        }
    }

    public ResultData FindById(string id)
    {
        if (map == null) BuildDictionary();
        map.TryGetValue(id, out var res);
        return res;
    }
    public ResultData FindByStageWithFallback(StageInfo stage, string key)
    {
        foreach (ResultDBList stageList in allResult)
        {
            if (stageList == null) continue;
            if (stageList.list[0].StageInfo != stage) continue;

            foreach (ResultData result in stageList.list)
            {
                if (result == null) continue;

                string[] keys = result.comboKeys;

                // exact 우선
                if (keys.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase)))
                    return result;
            }
        }
        return null;
    }

    public ResultData FindByStageBySubset(StageInfo stage, HashSet<string> submittedIds)
    {
        ResultData defaultRes = null;

        ResultData bestRes = null;
        int bestKeyCount = int.MaxValue;
        // comboKeys가 더 적은 엔딩을 우선으로 선택하게 함 -> 한개짜리만인 엔딩 선택을 위해

        foreach(ResultDBList stageList in allResult)
        {
            if (stageList == null) continue;
            if (stageList.list[0].StageInfo != stage) continue;

            foreach(ResultData result in stageList.list)
            {
                if(result == null) continue;

                string[] keys = result.comboKeys;

                // Default 캐싱
                if (defaultRes == null && keys.Any(k => string.Equals(k, "Default", StringComparison.OrdinalIgnoreCase)))
                {
                    defaultRes = result;
                    continue;
                }

                // combokeys를 set으로 만들고(단일 항목들이라 split 필요 없음)
                HashSet<string> comboSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i<keys.Length; i++)
                {
                    string k = keys[i];
                    if(string.IsNullOrWhiteSpace(k)) continue;
                    if (string.Equals(k, "Default", StringComparison.OrdinalIgnoreCase)) continue;
                    comboSet.Add(k.Trim());
                }

                if(comboSet.Count == 0) continue;
                Debug.Log(comboSet);

                // 제출 부분집합
                if(IsSubset(submittedIds, comboSet))
                {
                    // 가장 작은 콤보키 엔딩 선택
                    if(comboSet.Count < bestKeyCount)
                    {
                        bestKeyCount = comboSet.Count;
                        bestRes = result;
                    }
                }
            }
        }
        if(bestRes != null)
            return bestRes;

        return defaultRes;
    }
    private static bool IsSubset(HashSet<string> submitted, HashSet<string> comboSet)
    {
        // submitted의 모든 원소가 comboSet에 존재해야 함
        foreach (var id in submitted)
        {
            if (!comboSet.Contains(id))
                return false;
        }
        return true;
    }
}

[Serializable]
public class ResultDBList
{
    public List<ResultData> list = new();
}
