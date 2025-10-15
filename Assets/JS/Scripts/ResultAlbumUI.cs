using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultAlbumUI : MonoBehaviour
{
    public Image[] albumImage;
    public TMP_Text[] comment;
    void OnEnable()
    {
        var result = ResultManager.instance.endingOutcomes;
        int size = result.Count;
        for (int i = 0; i < size; i++) 
        {
            albumImage[i].sprite = result[i].albumImage;
            comment[i].text = result[i].comment;
        }
    }
   
}
