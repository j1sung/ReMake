using UnityEngine;
using UnityEngine.UI;

public class StageUIRefs : MonoBehaviour
{
    [Header("UI Button")]
    public Button changeSpaceBtn;
    public Button packageBtn;
    public Button packageYES;
    public Button packageNO;

    [Header("Enable/Disable")]
    public GameObject puzzleUI;
    public GameObject bag;
    public GameObject pop_up;
    public GameObject spaceMode_image;

    [Header("Change sprites")]
    public Sprite toBagMode;
    public Sprite toSpaceMode;
}
