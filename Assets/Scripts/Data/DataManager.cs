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
    public int stageNum; // �ֱ� �������� ���� ����

    // �繫�� ���� ���� �߰�
    public OfficeState officeState;

    public List<string> resultId = new List<string>(); // ��� ���� ����
    public List<StageObjeSave> objeByStage = new List<StageObjeSave>();// ���� ������ ���� ����
    public List<QuestEventId> questId = new List<QuestEventId>();// ����Ʈ ���� ����
}

[Serializable]
public class StageObjeSave
{
    public List<string> objeIds = new List<string>();
}

[Serializable]
public class SettingsData
{
    // ���� ����(���� ũ��) -> BGM/SFX
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

public class DataManager: MonoBehaviour
{
    public static DataManager Instance;

    private readonly List<IGameSaveParticipant> gameParticipants = new();
    private readonly List<ISettingsParticipant> settingsParticipants = new();
    private GameSaveData loadedGameCache;
    private SettingsData loadedSettingsCache; 
    
    // Save���� ���� ��� ����
    private string GetGamePath() => Path.Combine(Application.persistentDataPath, "gameSave.json");
    private string GetSettingsPath() => Path.Combine(Application.persistentDataPath, "settings.json");

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PreloadSaveOnStartup(); // ���̺����� �ҷ�����

        // �� �ε� �Ϸ� ������ Settings Apply!
        if(loadedSettingsCache != null)
            SceneManager.sceneLoaded += OnSceneLoadedForStartup;
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoadedForStartup;
    }

    // ���� ���� �ʱ� ���� ����
    private void PreloadSaveOnStartup()
    {
        // ���� ���۽� ���� �ҷ�����
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
           
        Debug.Log("loadCache ���� �Ϸ�!");
    }
  
    private void OnSceneLoadedForStartup(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ApplyStartupNextFrame());
        SceneManager.sceneLoaded -= OnSceneLoadedForStartup;
    }

    private IEnumerator ApplyStartupNextFrame()
    {
        yield return null; // �����ڵ� Register �� ������ ��ٸ�
        LoadSettings(); // settings Apply!
    }

    public void Initialize()
    {
        // 1. �޸� ĳ�� �ʱ�ȭ
        loadedGameCache = null;

        // 2. ���� ���� ����
        string gamePath = GetGamePath();
        if (File.Exists(gamePath))
        {
            File.Delete(gamePath);
            Debug.Log($"[DataManager] Game save deleted: {gamePath}");
        }
    }

    public void RegisterGame(IGameSaveParticipant p)
    {
        if(!gameParticipants.Contains(p)) gameParticipants.Add(p);
        Debug.Log("RegisterGame �Ϸ�!");
    }

    public void RegisterSettings(ISettingsParticipant p)
    {
        if (!settingsParticipants.Contains(p)) settingsParticipants.Add(p);
        Debug.Log("RegisterSettings �Ϸ�!");
    }

    public void UnRegisterGame(IGameSaveParticipant p)
    {
        gameParticipants.Remove(p);
    }
    public void UnRegisterSettings(ISettingsParticipant p)
    {
        settingsParticipants.Remove(p);
    }

    // ==== ������ ���� ��� ====
    public GameSaveData CaptureGame()
    {
        var data = new GameSaveData();
        foreach(var p in gameParticipants.OrderBy(x => x.Order)) 
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

    // ==== �ܺ� API Save() ====
    public void SaveGame() // ���� ���� ������ Save
    {
        GameSaveData data = CaptureGame();

        // �ֽ� ĳ�� ����
        loadedGameCache = data;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetGamePath(), json);
        Debug.Log($"[DataManager] Saved: {GetGamePath()}");
    }

    public void SaveSettings() // �ܺ� Settings Save
    {
        SettingsData data = CaptureSettings();

        // �ֽ� ĳ�� ����
        loadedSettingsCache = data;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSettingsPath(), json);
        Debug.Log($"[DataManager] Saved: {GetSettingsPath()}");
    }

    // ==== �ܺ� API Load() ====
    public void LoadGame() // ���� ���� ������ Load
    {
        if (loadedGameCache == null) return;

        // After �����ڰ� �ִ� ��쿡�� ���� -> �ݵ�� ���̵��� �������� �߰��ؾ���!
        /*
        bool hasAfter = participants.Any(p => p.phase == ApplyPhase.AfterSceneLoad);
        if(hasAfter)
        {
            // SceneController �� ��ȯ �̺�Ʈ ����
            SceneController.Instance.OnSceneLoaded -= HandleSceneLoaded;
            SceneController.Instance.OnSceneLoaded += HandleSceneLoaded;
        }*/

        // Before ������ ����
        ApplyGame(loadedGameCache, ApplyPhase.BeforeSceneLoad);

        // �繫�� �� �̵�
        GameManager.Instance.MoveScene(SceneData.Office);
    }

    public void LoadSettings() // �ܺ� Settings Load
    {
        if(loadedSettingsCache == null) return;

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

    // After ������ �������� �̺�Ʈ ȣ���
    /*
    private void HandleSceneLoaded()
    {
        // Ȥ�ö� �ߺ� ȣ��/�̹� ������ ��� ���
        if (loadedCache == null)
        {
            SceneController.Instance.OnSceneLoaded -= HandleSceneLoaded;
            return;
        }
        
        // After ������ �������� �����
        Apply(loadedCache, ApplyPhase.AfterSceneLoad);

        SceneController.Instance.OnSceneLoaded -= HandleSceneLoaded;
    }
    */
}
