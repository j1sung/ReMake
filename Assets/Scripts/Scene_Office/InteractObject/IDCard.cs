using UnityEngine;

public class IDCard : OfficeInteractable
{
    [SerializeField] private OfficeUIContext _beforeInteractCtx;

    void Awake()
    {
        actions = new(){{OfficeState.BeforeInteracts, OnClickIDCard} };
    }

    private void OnClickIDCard()
    {
        OfficeUIController.Instance.ShowUI(_beforeInteractCtx);
    }
}
