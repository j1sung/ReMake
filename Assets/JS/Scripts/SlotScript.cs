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
    public Vector2Int storedItemSize; // 호환용(안써도됨)
    public Vector2Int storedItemStartPos; // origin (이 아이템의 pivot이 놓인 슬롯)
    public bool isOccupied;

    private void Start()
    {
        if(text != null)
            text.text = gridPos.x + "," + gridPos.y;
    }
}
