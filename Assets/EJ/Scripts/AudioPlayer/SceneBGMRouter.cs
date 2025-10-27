using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBGMRouter : MonoBehaviour
{
    [Header("씬별 BGM")]
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip officeBGM;
    [SerializeField] private AudioClip roomBGM;
    

    [Header("전환 페이드(초)")]
    [SerializeField] private float fadeSeconds = 0.5f;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // 현재 씬에도 즉시 반영 (플레이 버튼으로 시작할 때)
        ApplyFor(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyFor(scene.name);
    }

    private void ApplyFor(string sceneName)
    {
        if (BGMPlayer.Instance == null) return;

        switch (sceneName)
        {
            case "MainTitle_test":
                BGMPlayer.Instance.PlayBGM(titleBGM, fadeSeconds);
                break;
            case "Office_f":
                BGMPlayer.Instance.PlayBGM(officeBGM, fadeSeconds);
                break;
            case "Room1_test":
                BGMPlayer.Instance.PlayBGM(roomBGM, fadeSeconds);
                break;
            default:
                // 기본값이 필요없다면 정지
                BGMPlayer.Instance.StopBGM(fadeSeconds);
                break;
        }
    }
}