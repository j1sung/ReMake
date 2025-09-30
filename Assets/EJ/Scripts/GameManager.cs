using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { mainMenu, office, room }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentState = GameState.mainMenu;
    public float loadingDelay = 0.2f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void GoMain() => ChangeState(GameState.mainMenu);
    public void GoOffice() => ChangeState(GameState.office);
    public void GoRoom() => ChangeState(GameState.room);

    public void ChangeState(GameState nextScene)
    {
        currentState = nextScene;

        string sceneName = nextScene switch
        {
            GameState.mainMenu => "MainTitle",
            GameState.office => "Office",
            GameState.room => "Room1",
            _ => "MainTitle" // default
        };

        StartCoroutine(LoadSceneRoutine(sceneName));
    }
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // 씬 로딩시 loadingDelay 만큼 지연
        if (loadingDelay > 0f)
            yield return new WaitForSeconds(loadingDelay);

        // 비동기 로드로 끊김 최소화
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!op.isDone)
            yield return null; // 다음 프레임까지 대기, 완료될 때까지 반복
    }
}
