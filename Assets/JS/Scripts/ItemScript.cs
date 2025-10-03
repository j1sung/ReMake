using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    private GameObject invenPanel;
    public static GameObject selectedItem;
    //public static Vector2Int selectedItemSize;
    public static bool isDragging = false;
    public static bool justSpawned;
   
    // 데이터
    public ObjeData item;
    public int currentRot; // 현재 회전값 -  0,1,2,3
    
    // UI 배율(칸 크기 - 바운딩 박스용)
    private float slotSize;

    private void Awake()
    {
        // 그리드 패널에서 슬롯 크기 읽어와 바운딩 사이즈 세팅에만 사용
        invenPanel = GameObject.FindGameObjectWithTag("InvenPanel");
        if (invenPanel != null)
            slotSize = invenPanel.GetComponent<InvenGridScript>().slotSize;
    }

    // 90도 시계 회전 (데이터/비주얼 동기화)
    public void RotateCW() {
        currentRot = (currentRot + 1) & 3;
        
        // 회전은 부모 RectTransform에만 준다.
        var r = GetComponent<RectTransform>();
        if (r) r.localRotation = Quaternion.Euler(0, 0, -currentRot * 90f);
    }

    // (스폰 직후 1회) 마스크 외곽 크기로 퍼즐의 RectTransform 사이즈 맞춤.
    // 현재 회전/모양 기준으로 맞춤, 회전해도 폭/높이는 바꾸지 않음.
    public void ResizeForParent(Transform parent, InvenGridManager grid)
    {
        /*
        // 퍼즐 셀 배열 좌표 설정
        Vector2Int[] cells = ShapeUtil.MaskToCells(item.maskRows);
        if (cells == null || cells.Length == 0) cells = new[] { Vector2Int.zero };

        // 회전 없는 "기본" 셀 기준 외곽 크기
        Vector2Int baseSize = ShapeUtil.GetBoundsSize(cells);

        // 바운딩(rect) 크기 지정
        var rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseSize.x * slotSize);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, baseSize.y * slotSize);*/

        // 0° 기준 셀 (캐시 사용)
        InvenGridManager.EnsureCellsCached(item);
        var cells0 = item.shapeCells != null && item.shapeCells.Length > 0
            ? item.shapeCells
            : new[] { Vector2Int.zero };

        ShapeUtil.GetAABB(cells0, out var bmin, out var bmax);
        int w0 = bmax.x - bmin.x + 1, h0 = bmax.y - bmin.y + 1;

        // parent 좌표계의 셀 크기 (lossyScale 반영)
        var cellSize = grid.GetCellSizeIn(parent);

        var rt = GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w0 * cellSize.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h0 * cellSize.y);

        // 이미지 비율 유지(찌그러짐 방지)
        var img = GetComponent<UnityEngine.UI.Image>();
        if (img) img.preserveAspect = true;
    }

    private void Update()
    {
        if (isDragging && selectedItem)
        {
            selectedItem.transform.position = Input.mousePosition;
        }
    }

    public static void SetSelectedItem(GameObject obj)
    {
        selectedItem = obj;
        //selectedItemSize = obj.GetComponent<ItemScript>().item.size;
        isDragging = true;
    }

    public static void ResetSelectedItem()
    {
        selectedItem = null;
        //selectedItemSize = Vector2Int.zero;
        isDragging = false;
    }
}
