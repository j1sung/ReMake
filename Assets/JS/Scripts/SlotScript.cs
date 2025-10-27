using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    public Vector2Int gridPos;
    public TextMeshProUGUI text;

    public GameObject storedItem;
    public Vector2Int storedItemStartPos; // origin (이 아이템의 pivot이 놓인 슬롯)
    public bool isOccupied; // 점유중인지?
    public bool isFirst; // 퍼즐 슬롯들 중 대표 슬롯으로 설정(결과 넘길때 체크)

    private void Start()
    {
        if(text != null)
            text.text = gridPos.x + "," + gridPos.y;
    }
}
