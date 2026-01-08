using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private Image img1;
    [SerializeField] private Image img2;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text genderText;
    [SerializeField] private TMP_Text birthDate;
    [SerializeField] private TMP_Text residence;
    [SerializeField] private TMP_Text note;

    [SerializeField] private JournalDB journalDB;

    private void OnEnable()
    {
        var data = journalDB.FindJournal(ResultManager.instance.CurrentStageInfo);
        img1.sprite = data.img[0];
        img2.sprite = data.img[1];
        nameText.text = data.nameText;
        genderText.text = data.genderText;
        birthDate.text = data.birthDateText;
        residence.text = data.residenceText;
        note.text = data.noteText;
    }
}
