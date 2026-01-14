using UnityEngine;

[CreateAssetMenu(menuName = "SO/CallScript")]
public class CallScript : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] lines;

    public OfficeState nextStateAfterCall;
}