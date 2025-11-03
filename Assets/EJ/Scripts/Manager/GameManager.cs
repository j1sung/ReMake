using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private SceneController sceneController;
    public CursorManager cursorManager { get; private set; }
    public SettingsPanelController settingPanel;

    public SceneData CurrentScene { get; private set; }
    public GameState CurrentState { get; private set; }


    void Awake()
    {
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPanel.exitPanel.activeSelf == true) return;
            settingPanel.OpenExitPanel();
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            ResultManager.instance.Initialized();
            MoveScene(SceneData.MainMenu);
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