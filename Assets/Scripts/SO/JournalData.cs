using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalData", menuName = "Scriptable Objects/JournalData")]
public class JournalData : ScriptableObject
{
    // 스테이지별로 필요한 것

    // 업무일지 이미지
    public Sprite normalImage;
    public Sprite blurredImage;

    // 룸 이미지
    public List<Sprite> img;

    /*
    // 내용 텍스트
    public string nameText; // 성명
    public string genderText; // 성별
    public string birthDateText; // 생년월일
    public string residenceText; // 거주지
    [TextArea] public string noteText; // 특이사항
    */
}

// 공용DB에서 그나마 필요한거
// 일반 이미지
// 사인된 이미지