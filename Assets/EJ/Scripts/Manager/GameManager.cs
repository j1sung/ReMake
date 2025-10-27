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
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void MoveScene(SceneData next)
    {
        CurrentScene = next;
        sceneController.LoadScene(next);
    }

    public void SetGameState(GameState nextState)
    {
        CurrentState = nextState;
    }
}