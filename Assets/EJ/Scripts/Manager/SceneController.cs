using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string officeScene;
    [SerializeField] private string room1Scene;
    [SerializeField] private string resultScene;
    

    [Header("Transition Settings")]
    [SerializeField] private float loadingDelay = 0.2f;

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
    private string ConvertSceneName(SceneData scene)
    {
        switch (scene)
        {
            case SceneData.MainMenu: return mainMenuScene;
            case SceneData.Office: return officeScene;
            case SceneData.Room: return room1Scene;
            case SceneData.Result: return resultScene;
            default: return mainMenuScene;
        }
    }
    
    // 여기서 페이드 인, 아웃 구현
    private IEnumerator LoadRoutine(string sceneName)
    {
        // 약간의 연출 지연
        if (loadingDelay > 0f)
            yield return new WaitForSeconds(loadingDelay);

        // 실제 로드
        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null;
    }
}
