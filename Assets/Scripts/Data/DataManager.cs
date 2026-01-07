using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameSaveData
{
    public int stageNum; // 최근 스테이지 진행 저장

    // 사무실 상태 저장 추가
    public OfficeState officeState;

    public List<string> resultId = new List<string>(); // 결과 정보 저장
    public List<StageObjeSave> objeByStage = new List<StageObjeSave>();// 제출 오브제 정보 저장
    public List<QuestEventId> questId = new List<QuestEventId>();// 퀘스트 정보 저장
}

[Serializable]
public class StageObjeSave
{
    public List<string> objeIds = new List<string>();
}

[Serializable]
public class SettingsData
{
    // 사운드 세팅(볼륨 크기) -> BGM/SFX
    public SoundSaveData sound = new SoundSaveData();
}

[Serializable]
public class SoundSaveData
{
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    public void Set(float bgm, float sfx)
    {
        bgmVolume = bgm;
        sfxVolume = sfx;
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private readonly List<IGameSaveParticipant> gameParticipants = new();
    private readonly List<ISettingsParticipant> settingsParticipants = new();
    private GameSaveData loadedGameCache;
    private SettingsData loadedSettingsCache;

    // Save파일 저장 경로 설정
    private string GetGamePath() => Path.Combine(Application.persistentDataPath, "gameSave.json");
    private string GetSettingsPath() => Path.Combine(Application.persistentDataPath, "settings.json");

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PreloadSaveOnStartup(); // 세이브파일 불러오기

        // 씬 로드 완료 시점에 Settings Apply!
        if (loadedSettingsCache != null)
            SceneManager.sceneLoaded += OnSceneLoadedForStartup;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoadedForStartup;
    }

    // 게임 시작 초기 사운드 설정
    private void PreloadSaveOnStartup()
    {
        // 게임 시작시 파일 불러오기
        if (HasGameSaveFile())
        {
            loadedGameCache = LoadGameDataOnly();
        }
        else
        {
            Debug.Log("[DataManager] No GameSave file!");
        }

        if (HasSettingsFile())
        {
            loadedSettingsCache = LoadSettingsOnly();
        }
        else
        {
            Debug.Log("[DataManager] No Settings file!");
        }

        Debug.Log("loadCache 저장 완료!");
    }

    private void OnSceneLoadedForStartup(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ApplyStartupNextFrame());
        SceneManager.sceneLoaded -= OnSceneLoadedForStartup;
    }

    private IEnumerator ApplyStartupNextFrame()
    {
        yield return null; // 참여자들 Register 한 프레임 기다림
        LoadSettings(); // settings Apply!
    }

    public void Initialize()
    {
        // 1. 메모리 캐시 초기화
        loadedGameCache = null;

        // 2. 저장 파일 삭제
        string gamePath = GetGamePath();
        if (File.Exists(gamePath))
        {
            File.Delete(gamePath);
            Debug.Log($"[DataManager] Game save deleted: {gamePath}");
        }
    }

    public void RegisterGame(IGameSaveParticipant p)
    {
        if (!gameParticipants.Contains(p)) gameParticipants.Add(p);
        Debug.Log("RegisterGame 완료!");
    }

    public void RegisterSettings(ISettingsParticipant p)
    {
        if (!settingsParticipants.Contains(p)) settingsParticipants.Add(p);
        Debug.Log("RegisterSettings 완료!");
    }

    public void UnRegisterGame(IGameSaveParticipant p)
    {
        gameParticipants.Remove(p);
    }
    public void UnRegisterSettings(ISettingsParticipant p)
    {
        settingsParticipants.Remove(p);
    }

    // ==== 데이터 접근 기능 ====
    public GameSaveData CaptureGame()
    {
        var data = new GameSaveData();
        foreach (var p in gameParticipants.OrderBy(x => x.Order))
            p.Capture(data);
        return data;
    }

    public SettingsData CaptureSettings()
    {
        var data = new SettingsData();
        foreach (var p in settingsParticipants.OrderBy(x => x.Order))
            p.Capture(data);
        return data;
    }

    public void ApplyGame(GameSaveData data, ApplyPhase phase)
    {
        foreach (var p in gameParticipants
                    .Where(x => x.Phase == phase)
                    .OrderBy(x => x.Order))
        {
            p.Apply(data);
        }
    }

    public void ApplySettings(SettingsData data)
    {
        foreach (var p in settingsParticipants
                    .OrderBy(x => x.Order))
        {
            p.Apply(data);
        }
    }

    // ==== 외부 API Save() ====
    public void SaveGame() // 게임 내부 데이터 Save
    {
        GameSaveData data = CaptureGame();

        // 최신 캐시 갱신
        loadedGameCache = data;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetGamePath(), json);
        Debug.Log($"[DataManager] Saved: {GetGamePath()}");
    }

    public void SaveSettings() // 외부 Settings Save
    {
        SettingsData data = CaptureSettings();

        // 최신 캐시 갱신
        loadedSettingsCache = data;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSettingsPath(), json);
        Debug.Log($"[DataManager] Saved: {GetSettingsPath()}");
    }

    // ==== 외부 API Load() ====
    public void LoadGame() // 게임 내부 데이터 Load
    {
        if (loadedGameCache == null) return;

        // After 참가자가 있는 경우에만 구독 -> 반드시 씬이동이 있을때만 추가해야함!
        /*
        bool hasAfter = participants.Any(p => p.phase == ApplyPhase.AfterSceneLoad);
        if(hasAfter)
        {
            // SceneController 씬 전환 이벤트 구독
            SceneController.Instance.OnSceneLoaded -= HandleSceneLoaded;
            SceneController.Instance.OnSceneLoaded += HandleSceneLoaded;
        }*/

        // Before 참가자 적용
        ApplyGame(loadedGameCache, ApplyPhase.BeforeSceneLoad);

        // 사무실 씬 이동
        GameManager.Instance.MoveScene(SceneData.Office);
    }

    public void LoadSettings() // 외부 Settings Load
    {
        if (loadedSettingsCache == null) return;

        ApplySettings(loadedSettingsCache);
    }

    public GameSaveData LoadGameDataOnly()
    {
        string json = File.ReadAllText(GetGamePath());
        return JsonUtility.FromJson<GameSaveData>(json);
    }

    public SettingsData LoadSettingsOnly()
    {
        string json = File.ReadAllText(GetSettingsPath());
        return JsonUtility.FromJson<SettingsData>(json);
    }

    public bool HasGameSaveFile()
    {
        string path = GetGamePath();
        return File.Exists(path);
    }

    public bool HasSettingsFile()
    {
        string path = GetSettingsPath();
        return File.Exists(path);
    }

    // After 참가자 있을때만 이벤트 호출됨
    /*
    private void HandleSceneLoaded()
    {
        // 혹시라도 중복 호출/이미 정리된 경우 방어
        if (loadedCache == null)
        {
            SceneController.Instance.OnSceneLoaded -= HandleSceneLoaded;
            return;
        }
        
        // After 참가자 있을때만 적용됨
        Apply(loadedCache, ApplyPhase.AfterSceneLoad);

        SceneController.Instance.OnSceneLoaded -= HandleSceneLoaded;
    }
    */
}