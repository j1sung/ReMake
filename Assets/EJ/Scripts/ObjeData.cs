using UnityEngine;

[CreateAssetMenu(fileName = "ObjeData", menuName = "NewObjeData")]
public class ObjeData : ScriptableObject
{
    public string objeName; // 오브제 이름
    [TextArea] public string objeDescription; // 오브제 설명

    public string itemType; // 오브제 종류(로직용)
    
    // -- 추가: ㄴ자/ㄱ자 등 자유 모양을 표현하기 위한 마스크 또는 셀 목록
    // 1. 편한 입력 방식: 마스크(문자열) -- 둘 중 하나만 써도 됨. maskRows가 있으면 런타임에 shapeCells로 변환해 캐시함.
    [TextArea(1, 8)]
    public string[] maskRows; // 예: new[] { "111", "100" } <- 첫 문자열이 첫 라인, 다음 문자열은 다음 라인, '1'은 점유, '0'은 비어있음
    public int pivotX;        // 마크스 기반일 때 피벗 - maskRows 기준 좌표(열 X, 행 Y)
    public int pivotY;

    // 2. 직접 지정 방식: 좌표 목록
    public Vector2Int[] shapeCells; //(0,0) 기준 상대 좌표, 캐시된 셀 목록
    public int pivotIndex; // shapeCells 배열에서의 피벗 인덱스(캐시)

    public Sprite iconImage;
    public Sprite puzzleImage;
}
