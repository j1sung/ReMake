using UnityEngine;

[CreateAssetMenu(fileName = "ResultData", menuName = "Scriptable Objects/ResultData")]
public class ResultData : ScriptableObject
{
    public StageInfo StageInfo; // 일단 명시적으로만
    public EndingEntry ending;
}

public enum StageInfo
{
    stage1,
    stage2,
    stage3,
}