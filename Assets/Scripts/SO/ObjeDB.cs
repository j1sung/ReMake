using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjeDB", menuName = "Scriptable Objects/ObjeDB")]
public class ObjeDB : ScriptableObject
{
    [SerializeField] private List<ObjeDBList> allObjes; // 편집용
    private Dictionary<string, ObjeData> map; // 검색용

    private void OnEnable() => BuildDictionary();

    private void BuildDictionary()
    {
        map = new Dictionary<string, ObjeData>();
        foreach (var stageList in allObjes) 
        {
            if(stageList == null) continue;

            foreach (var obj in stageList.list)
            {
                if(obj == null) continue;

                if (!map.TryAdd(obj.itemName, obj))
                    Debug.LogWarning($"[ObjeDB] 중복 Obje ID: {obj.itemName}");
            }
        }
    }

    public ObjeData FindById(string id)
    {
        if(map == null) BuildDictionary();
        map.TryGetValue(id, out var obj);
        return obj;
    }
}

[Serializable]
public class ObjeDBList
{
    public List<ObjeData> list = new();
}