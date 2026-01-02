using UnityEngine;

public class Request : OfficeInteractable
{
    [SerializeField] private OfficeUIContext _beforeInteractCtx;

    void Awake()
    {
        actions = new() { { OfficeState.BeforeInteracts, OnClickRequst } };
    }

    private void OnClickRequst()
    {
        OfficeUIController.Instance.ShowUI(_beforeInteractCtx);
    }
}
