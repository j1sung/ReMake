using UnityEngine;

[CreateAssetMenu(fileName = "ResultData", menuName = "Scriptable Objects/ResultData")]
public class ResultData : ScriptableObject
{
    public StageInfo StageInfo; // 일단 명시적으로만
    public string id;
    public string[] comboKeys;
    public string endingId;
    [TextArea] public string comment;
}
public enum StageInfo
{
    stage1,
    stage2,
    stage3,
}