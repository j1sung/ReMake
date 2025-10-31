using UnityEngine;

public class playBookOpen : MonoBehaviour
{
    public AudioClip openBook;
    void OnDisable()
    {
        SFXPlayer.Instance.PlaySFX(openBook);
    }
}
