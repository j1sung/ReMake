using UnityEngine;

public class IDCard : OfficeInteractable
{
    [SerializeField] private OfficeUIContext _beforeInteractCtx;
    [SerializeField] private OfficeUIContext _stage1ClearCtx;
    [SerializeField] private Request _request; 
    private bool isClicked = false;
    

    void Awake()
    {
        actions = new(){{OfficeState.BeforeInteracts, OnClickBefore},
                        {OfficeState.AfterInteracts, () => OnClickIDCard(_beforeInteractCtx)},
                        {OfficeState.ReadyStage2, () => OnClickIDCard(_stage1ClearCtx)},
                        {OfficeState.ReadyStage3, () => OnClickIDCard(_stage1ClearCtx)} };
    }

    private void OnClickBefore()
    {
        OnClickIDCard(_beforeInteractCtx);

        if (!isClicked)
        {
            isClicked = true;
            _request.CheckCondition();
        }
    }

    private void OnClickIDCard(OfficeUIContext ctx)
    {
        OfficeUIController.Instance.ShowUI(ctx);
    }
}
