using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public QuestType questType; // 일단 명시적으로만
    public QuestEventId id;
    public string qName;
    [TextArea] public string qDescription;
}

public enum QuestType
{
    Auto,
    Action,
}