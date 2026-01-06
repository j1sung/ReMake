using UnityEngine;

public class GlobalInputHandler : MonoBehaviour
{   
    public SettingsPanelController settingPanel;

    private void Update()
    {
        HandleEscape();
        HandleRestart();
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPanel.exitPanel.activeSelf == true) return;
            settingPanel.OpenExitPanel();
        }
    }

    private void HandleRestart()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            DataManager.Instance.Initialize();
            ResultManager.instance.Initialize();
            GameManager.Instance.MoveScene(SceneData.MainMenu);
        }
    }
}
