using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenGridScript : MonoBehaviour {

    public GameObject[,] slotGrid;
    public GameObject slotPrefab;
    public Vector2Int gridSize;
    public float slotSize;
    public float edgePadding;
    

    public void Awake()
    {
        
        slotGrid = new GameObject[gridSize.x, gridSize.y];
        ResizePanel();
        CreateSlots();
        GetComponent<InvenGridManager>().gridSize = gridSize;
    }

    private void CreateSlots()
    {
        // 중앙 정렬 기준 계산
        float startX = -((gridSize.x - 1) * 0.5f) * slotSize + edgePadding;
        float startY = -((gridSize.y - 1) * 0.5f) * slotSize + edgePadding;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject obj = (GameObject)Instantiate(slotPrefab);
                
                obj.name = "slot[" + x + "," + y + "]"; // 슬롯 이름 붙이기
                obj.transform.SetParent(transform, false); // 이 오브젝트의 부모를 설정, worldPositionStays = false -> 로컬 좌표 유지

                RectTransform rect = obj.GetComponent<RectTransform>();

                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                // 부모 중앙 기준 로컬 슬롯 배치
                rect.localPosition = new Vector3(
                    startX + x * slotSize,
                    startY + y * slotSize,
                    0f
                );

                obj.GetComponent<RectTransform>().localScale = Vector3.one; // 단위 스케일
                obj.GetComponent<SlotScript>().gridPos = new Vector2Int(x, y); // 슬롯의 grid 좌표 설정
                slotGrid[x, y] = obj;
            }
        }
        GetComponent<InvenGridManager>().slotGrid = slotGrid;
    }

    private void ResizePanel()
    {
        float width, height;
        width = (gridSize.x * slotSize) + (edgePadding * 2);
        height = (gridSize.y * slotSize) + (edgePadding * 2);

        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rect.localScale = Vector3.one;
    }
}
