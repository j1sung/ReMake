using UnityEngine;

[CreateAssetMenu(fileName = "ObjeData", menuName = "NewObjeData")]
public class ObjeData : ScriptableObject
{
    public string objeName;
    [TextArea] public string objeDescription;
    public Sprite icon;
}
