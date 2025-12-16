using UnityEngine;
using UnityEngine.UI;

public class SetChangeGrayColor : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private SpriteRenderer[] spriters;
    private void OnEnable()
    {
        Color targetColor = new Color(222f / 255f, 222f / 255f, 222f / 255f, 1f);
        foreach (var img in images)
        {
            img.color = targetColor;
        }
        foreach (var img in spriters)
        {
            img.color = targetColor;
        }
    }

    private void OnDisable()
    {
        foreach (var img in images)
        {
            img.color = Color.white;
        }
        foreach (var img in spriters)
        {
            img.color = Color.white;
        }
    }
}
