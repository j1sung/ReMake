using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private Image img1;
    [SerializeField] private Image img2;
    [SerializeField] private Image journal;
    [SerializeField] private GameObject sign;
    [SerializeField] private GameObject stamp;

    /*
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text genderText;
    [SerializeField] private TMP_Text birthDate;
    [SerializeField] private TMP_Text residence;
    [SerializeField] private TMP_Text note;
    */

    [SerializeField] private JournalDB journalDB;
    private JournalData data;

    private void OnEnable()
    {
        var _stageInfo = ResultManager.instance.CurrentStageInfo;
        data = journalDB.FindJournal(_stageInfo);

        // 룸 이미지 채우기
        img1.sprite = data.img[0];
        img2.sprite = data.img[1];
        img1.preserveAspect = true;
        img2.preserveAspect = true;

        // 진행 상태에 따라 업무일지 이미지 채우기
        journal.sprite = data.blurredImage;

        switch (_stageInfo)
        {
            case 1:
                if (OfficeStateMachine.currentState == OfficeState.AfterInteracts)
                {
                    journal.sprite = data.blurredImage;
                    if (sign.activeSelf == false) sign.SetActive(false);
                    stamp.SetActive(false);
                    break;
                }
                else
                {
                    journal.sprite = data.normalImage;
                    sign.SetActive(true);
                    stamp.SetActive(true);
                    break;
                }
            case 2:
                if (OfficeStateMachine.currentState == OfficeState.ReadyStage2)
                {
                    journal.sprite = data.blurredImage;
                    if (sign.activeSelf == false) sign.SetActive(false);
                    stamp.SetActive(false);
                    break;
                }
                else
                {
                    journal.sprite = data.normalImage;
                    sign.SetActive(true);
                    stamp.SetActive(true);
                    break;
                } 
        }
        
        /*
        nameText.text = data.nameText;
        genderText.text = data.genderText;
        birthDate.text = data.birthDateText;
        residence.text = data.residenceText;
        note.text = data.noteText;
        */
    }

    public void OnUnblur()
    {
        journal.sprite = data.normalImage;
    }
}
