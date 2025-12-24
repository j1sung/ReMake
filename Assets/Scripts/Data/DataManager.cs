using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public string sceneName;
    public int stageNum;
    // 사운드 세팅(볼륨 크기) -> BGM/SFX
    public List<EndingOutcome> ending;
    public List<QuestEventId> questId;

}
public class DataManager: MonoBehaviour
{
    public static DataManager Instance;
}
