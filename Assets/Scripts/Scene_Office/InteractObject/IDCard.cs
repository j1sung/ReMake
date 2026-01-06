using UnityEngine;

public class IDCard : OfficeInteractable
{
    [SerializeField] private OfficeUIContext _beforeInteractCtx;
    [SerializeField] private Request _request; 
    private bool isClicked = false;
    

    void Awake()
    {
        actions = new(){{OfficeState.BeforeInteracts, OnClickIDCard},
                        {OfficeState.AfterInteracts, OnClickIDCard}};
    }

    private void OnClickIDCard()
    {
        OfficeUIController.Instance.ShowUI(_beforeInteractCtx);

        if (!isClicked)
        {   
            isClicked = true;
            _request.CheckCondition();
        }
    }
}
