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
        ResultData defaultRes = null;

        foreach (var stageList in allResult)
        {
            if (stageList == null) continue;

            foreach (var res in stageList.list)
            {
                if (res == null) continue;
                if (res.StageInfo != stage) continue;

                var keys = res.comboKeys;
                if (keys == null || keys.Length == 0) continue;

                // Default 캐싱(먼저 잡아두기)
                if (defaultRes == null && keys.Any(k => string.Equals(k, "Default", StringComparison.OrdinalIgnoreCase)))
                    defaultRes = res;

                // exact 우선
                if (keys.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase)))
                    return res;
            }
        }

        if (defaultRes == null)
            Debug.LogError($"[ResultDB] Default 결과가 없습니다. stage={stage}, requestedKey={key}");

        return defaultRes;
    }

}

[Serializable]
public class ResultDBList
{
    public List<ResultData> list = new();
}
