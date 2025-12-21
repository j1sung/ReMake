using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDB", menuName = "Scriptable Objects/QuestDB")]
public class QuestDB : ScriptableObject
{
    [SerializeField] private List<QuestData> allQuests;

    public QuestData FindById(string id)
    {
        return allQuests.Find(q => q.qID == id);
    }
}
