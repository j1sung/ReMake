using UnityEngine;

public class Cabinet : OfficeInteractable
{
    [SerializeField] private OfficeUIContext _beforeInteractCtx;

    void Awake()
    {
        actions = new() { { OfficeState.BeforeInteracts, OnClickCabinet },
                          {OfficeState.AfterInteracts, OnClickCabinet} };
    }

    private void OnClickCabinet()
    {
        OfficeUIController.Instance.ShowUI(_beforeInteractCtx);
    }
}
