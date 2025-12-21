using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EndingOutcome
{
    public string endingId;
    [TextArea] public string comment;

    public List<ObjeData> objeDatas; // 제출한 오브제 데이터들
}

[Serializable]
public class EndingEntry
{
    // 조합 키(오름차순 정렬된 타입만 +로 연결, 예: "Clothes+Letter")
    public string[] comboKeys;
    public EndingOutcome outcome;
}