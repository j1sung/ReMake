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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        Debug.Log("소리 재생");
        ApplyBGM(GameManager.Instance.CurrentScene);
    }

    public void ApplyBGM(SceneData sceneData)
    {
        switch (sceneData)
        {
            case SceneData.MainMenu:
                BGMPlayer.Instance.PlayBGM(titleBGM, fadeSeconds);
                break;

            case SceneData.Office:
                BGMPlayer.Instance.PlayBGM(officeBGM, fadeSeconds);
                break;

            case SceneData.Room:
                BGMPlayer.Instance.PlayBGM(roomBGM, fadeSeconds);
                break;

            /* case SceneData.Result:
                break; */
            default:
                BGMPlayer.Instance.StopBGM(fadeSeconds);
                break;
        }
    }
}