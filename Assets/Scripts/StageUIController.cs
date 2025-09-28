using UnityEngine;

public class StageUIController : MonoBehaviour
{
    public StageUIRefs stage;

    private void Awake()
    {
        stage.changeSpaceBtn.onClick.AddListener(ToggleMode);
        stage.packageBtn.onClick.AddListener(Package);
        stage.packageNO.onClick.AddListener(PackageNo);
        //packageYES.onClick.AddListener(); YES 클릭시 다음 씬 넘어감
    }

    public void ToggleMode()
    {
        if (stage.changeSpaceBtn.image.sprite == stage.toSpaceMode)
        {
            ToSpace();
        }
        else
            ToBag();

    }

    void ToSpace()
    {
        stage.puzzleUI.SetActive(false);
        stage.bag.SetActive(false);
        stage.changeSpaceBtn.image.sprite = stage.toBagMode;
        stage.spaceMode_image.SetActive(true);
    }
    public void ToBag()
    {
        stage.puzzleUI.SetActive(true);
        stage.bag.SetActive(true);
        stage.changeSpaceBtn.image.sprite = stage.toSpaceMode;
        stage.spaceMode_image.SetActive(false);
    }

    public void Package()
    {
        stage.pop_up.SetActive(true);
    }

    public void PackageNo()
    {
        stage.pop_up.SetActive(false);
    }
}
