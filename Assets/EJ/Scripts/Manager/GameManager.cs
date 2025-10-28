using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private SceneController sceneController;

    public SceneData CurrentScene { get; private set; }
    public GameState CurrentState { get; private set; }

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        CurrentScene = SceneData.MainMenu;

        if (BGMPlayer.Instance != null)
        {
            BGMPlayer.Instance.GetComponent<SceneBGMRouter>()?.ApplyBGM(CurrentScene);
        }
    }

    public void MoveScene(SceneData next)
    {
        CurrentScene = next;
        sceneController.LoadScene(next);

        if (BGMPlayer.Instance != null)
        {
            BGMPlayer.Instance.GetComponent<SceneBGMRouter>()?.ApplyBGM(next);
        }
    }

    public void SetGameState(GameState nextState)
    {
        CurrentState = nextState;
    }
}