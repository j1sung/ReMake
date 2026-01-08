using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening; // 도트윈 추가


public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string officeScene;
    [SerializeField] private string room1Scene;
    [SerializeField] private string room2Scene;
    [SerializeField] private string resultScene;


    [Header("Transition Settings")]
    [SerializeField] private float loadingDelay = 0.2f;
    [SerializeField] private float fadeDuration = 0.4f;
    [SerializeField] private CanvasGroup fadeCanvas; // 검은 패널

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void LoadScene(SceneData nextScene)
    {
        string sceneName = ConvertSceneName(nextScene);
        StartCoroutine(LoadRoutine(sceneName));
    }

    // enum형 씬 이름을 string으로 변환
    public string ConvertSceneName(SceneData scene)
    {
        switch (scene)
        {
            case SceneData.MainMenu: return mainMenuScene;
            case SceneData.Office: return officeScene;
            case SceneData.Stage1: return room1Scene;
            case SceneData.Stage2: return room2Scene;
            case SceneData.Result: return resultScene;
            default: return mainMenuScene;
        }
    }
    
    // 🎬 페이드 인/아웃 포함한 씬 로드 루틴
    private IEnumerator LoadRoutine(string sceneName)
    {
        // 1. 페이드아웃 (검은색으로 덮기)
        if (fadeCanvas != null)
        {
            fadeCanvas.blocksRaycasts = true;
            yield return fadeCanvas.DOFade(1f, fadeDuration)
                                   .SetUpdate(true) // 타임스케일 0에서도 작동
                                   .WaitForCompletion();
        }

        // 2. 약간의 연출 지연
        if (loadingDelay > 0f)
            yield return new WaitForSeconds(loadingDelay);

        // 3. 실제 씬 로드
        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null;

        // 4. 페이드인 (화면 밝히기)
        if (fadeCanvas != null)
        {
            yield return fadeCanvas.DOFade(0f, fadeDuration)
                                   .SetUpdate(true)
                                   .WaitForCompletion();
            fadeCanvas.blocksRaycasts = false;
        }
    }
}
