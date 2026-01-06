using UnityEngine;

public class AutoSaveGame : MonoBehaviour
{
    private void OnEnable()
    {
        DataManager.Instance.SaveGame();
    }
}
