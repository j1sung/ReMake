using UnityEngine;

public class SettingButtonProxy : MonoBehaviour
{
    public void OnSettingOpen()
    {
        SettingsPanelController.Instance.OpenSettingPanel();
    }
}
