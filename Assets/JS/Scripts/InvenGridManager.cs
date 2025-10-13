using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InvenGridManager : MonoBehaviour {

    [Header("UI Refs")]
    [SerializeField] private RectTransform gridRect; // 그리드 패널
    [SerializeField] private Canvas canvas; // 같은 캔버스
    private Camera uiCam;

    public GameObject[,] slotGrid; // 슬롯 오브젝트 배열
    public GameObject highlightedSlot;
    public Transform dropParent;
    public Transform dragParent;
    [HideInInspector] public Vector2Int gridSize; // 그리드 사이즈 ex) 4,4

    public InventoryManager listManager;
    public GameObject selectedButton;

    // 내부 상태
    private bool isOverEdge = false;
    private int checkState; // 0=가능, 1=스왑, 2=불가
    //private Vector2Int totalOffset, checkSize, checkStartPos; // origin (피벗 기준)
    //private Vector2Int otherItemPos, otherItemSize; // 스왑 표시에만 사용
    private Vector2Int origin; // 이번 프리뷰/배치의 기준(origin = pivot이 올 좌표)
    private GameObject overlapItem; // 스왑 대상
    private GameObject overlapSlot; // 겹친 슬롯
    private Vector2Int overlapOrigin; // 스왑 대상의 origin

    // 미리보기 색상 복원용
    private readonly List<Vector2Int> lastPreviewCells = new();
    private readonly List<Vector2Int> lastOverlapCells = new();

    private void Awake()
    {
        if(canvas == null) canvas = GetComponentInParent<Canvas>();
        uiCam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay)?canvas.worldCamera : null;
    }
    private void Start()
    {
        ItemButtonScript.invenManager = this;
    }

    private bool PointerOnGrid()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(gridRect, Input.mousePosition, uiCam);
    }

    private void Update()
    {
        // 드래그 중 우클릭 회전
        if(ItemScript.selectedItem != null && Input.GetMouseButtonDown(1))
        {
            var isComp = ItemScript.selectedItem.GetComponent<ItemScript>();
            if(isComp != null)
            {
                // 데이터 회전 (폭/높이 유지)
                isComp.RotateCW();


                // 프리뷰(그리드 하이라이트) 새로 계산
                RefrechColor(false); // 이전 색 지우기
                RefrechColor(true); // 새 회전값으로 다시 표시
            }
        }

        // 오브젝트 슬롯에 드롭하기 분기
        if (Input.GetMouseButtonUp(0))
        {
            // 퍼즐 스폰 직후 1프레임 면책: 버튼 스폰 직후 프레임은 되돌리기 처리 안함
            if (ItemScript.justSpawned) { ItemScript.justSpawned = false; return; }

            // 드래그 중인 아이템이 있을 때
            if (ItemScript.selectedItem != null)
            {
                // 최신 상태로 한번 더 판정
                ComputePlacementInfo();

                bool validDrop =
                        highlightedSlot != null &&
                        !isOverEdge &&
                        (checkState == 0 || checkState == 1);

                // 먼저 그리드 위에 있는지 체크
                if (PointerOnGrid())
                {
                    if (validDrop)
                    {
                        if (checkState == 0) //빈 슬롯에 저장
                        {
                            StoreItem(ItemScript.selectedItem);
                            ItemScript.ResetSelectedItem();
                            ResetSelectedButton();
                        }
                        else //퍼즐 스왑
                        {
                            ItemScript.SetSelectedItem(SwapItem(ItemScript.selectedItem));
                            // 바로 새 배치가 가능하니 프리뷰 갱신
                            RefrechColor(true);
                            ResetSelectedButton();
                        }
                    }
                    else // 그리드 위지만 invalid → 반환
                    {
                        listManager.ReturnSelectedToList();
                    }
                }
                else
                {
                    // 그리드 밖에서 놓음 -> 무조건 반환
                    listManager.ReturnSelectedToList();
                }
                return;
            }
            // 드래그 중이 아니고, 슬롯 점유 중 -> 꺼내기
            else if (highlightedSlot != null && ItemScript.selectedItem == null && highlightedSlot.GetComponent<SlotScript>().isOccupied == true)
            {
                ColorCells(lastPreviewCells, Color.white); // 혹시 남아있으면 정리
                ItemScript.SetSelectedItem(GetItem(highlightedSlot)); // 선택 & 드래그 상태로
                RefrechColor(true);
            }
        }
    }

    // 미리보기/판정(셀 기반)
    public void RefrechColor(bool enter)
    {
        // 이전 프리뷰 지우기
        ClearPreviewTints();
        
        if (!enter || ItemScript.selectedItem == null || highlightedSlot == null) return;

        // 현재 상태 계산
        ComputePlacementInfo();

        // 프리뷰 셀들 얻기
        var rotated = GetRotatedCells(ItemScript.selectedItem);
        var preview = rotated.Select(c => origin + c).ToList();
        lastPreviewCells.AddRange(preview);

        // 색칠
        Color32 col = checkState switch
        {
            0 => ColorHighlights.Green,
            1 => ColorHighlights.Green, // 내 조각은 초록(스왑셀은 노랑으로 별도)
            _ => ColorHighlights.Red,
        };
        ColorCells(preview, col);

        // 스왑 대상이 있다면 그 아이템의 셀도 노란색으로 표시
        if (checkState == 1 && overlapItem != null) {
            var overCells = GetWorldCells(overlapItem);
            lastOverlapCells.AddRange(overCells);
            ColorCells(overCells, ColorHighlights.Yellow);
        }
    }

    private void ClearPreviewTints()
    {
        if (lastPreviewCells.Count > 0)
        {
            ColorCells(lastPreviewCells, Color.white);
            lastPreviewCells.Clear();
        }
        if (lastOverlapCells.Count > 0)
        {
            ColorCells(lastOverlapCells, Color.white);
            lastOverlapCells.Clear();
        }
    }

    private void ColorCells(IEnumerable<Vector2Int> cells, Color32 color)
    {
        foreach (var p in cells)
        {
            if (p.x < 0 || p.y < 0 || p.x >= gridSize.x || p.y >= gridSize.y) continue;
            slotGrid[p.x, p.y].GetComponent<Image>().color = color;
        }
    }

    // 현재 선택 아이템을 하이라이트 기준으로 둘 때의 배치 정보 계산
    private void ComputePlacementInfo()
    {
        isOverEdge = false;
        overlapItem = null;
        overlapSlot = null;

        if (ItemScript.selectedItem == null || highlightedSlot == null)
        {
            checkState = 2; 
            return;
        }

        // 1. 회전 반영된 셀 목록
        var rotated = GetRotatedCells(ItemScript.selectedItem);

        // 2. pivot과 하이라이트 슬롯을 이용해 origin 계산
        var it = ItemScript.selectedItem.GetComponent<ItemScript>().item;
        EnsureCellsCached(it);
        var pivot = rotated[it.pivotIndex];
        var target = highlightedSlot.GetComponent<SlotScript>().gridPos;
        origin = target - pivot;

        // 3. 경계/겹침 체크
        bool multiOverlap = false;
        foreach (var c in rotated)
        {
            var p = origin + c;
            if (p.x < 0 || p.y < 0 || p.x >= gridSize.x || p.y >= gridSize.y)
            {
                isOverEdge = true; break;
            }

            GameObject cellGo = slotGrid[p.x, p.y];
            var ss = cellGo.GetComponent<SlotScript>();
            if (ss.isOccupied)
            {
                if(overlapItem == null)
                {
                    overlapItem = ss.storedItem;
                    overlapOrigin = ss.storedItemStartPos; // origin 기준
                    overlapSlot = cellGo;
                }
                else if(overlapItem != ss.storedItem)
                {
                    multiOverlap = true; break;
                }
            }
        }

        if (isOverEdge || multiOverlap) checkState = 2;
        else if (overlapItem != null)   checkState = 1;
        else                            checkState = 0;

        Vector2Int[] rotated2 = GetRotatedCells(ItemScript.selectedItem);
        Debug.Log($"origin={origin} rotated2={string.Join(" ", rotated)}");

    }


        // ─────────────────────────────────────────────────────────────
        // 저장/회수/스왑 (셀 기반)
        // ─────────────────────────────────────────────────────────────
    private void StoreItem(GameObject item)
    {
        var isComp = item.GetComponent<ItemScript>();
        var it = isComp.item;

        // 회전 적용된 셀(미리보기/점유용)
        var rotatedCells = GetRotatedCells(item); // currentRot 반영

        // 1. 슬롯 점유 메타 기록(origin 기준)
        foreach (var c in rotatedCells)
        {
            var p = origin + c;
            var ss = slotGrid[p.x, p.y].GetComponent<SlotScript>();
            ss.storedItem = item;
            ss.storedItemStartPos = origin;
            ss.storedItemSize = Vector2Int.one; // 호환용(안 써도 값은 채움)
            ss.isOccupied = true;
            slotGrid[p.x, p.y].GetComponent<Image>().color = Color.white;
        }


        // 3. UI 스냅: 아이템 pivot=좌하단(0,0)으로 두고, '바운딩박스 좌하단 셀'의 좌하단에 정확히 맞춘다.
        //RectTransform dropParentRect = (RectTransform)dropParent;
        //RectTransform slotRect = slotGrid[origin.x, origin.y].GetComponent<RectTransform>();
        //RectTransform baseSlotRect = slotGrid[bl.x, bl.y].GetComponent<RectTransform>();
        
        // 배치대상 RectTransform 준비
        var itemRect = item.GetComponent<RectTransform>();
        itemRect.SetParent(dropParent, false);
        itemRect.localScale = Vector3.one;
        //itemRect.pivot = Vector2.zero; // 좌하단 기준 (중요)

        var baseCells = GetCells(it); // 회전 X(0)
        ShapeUtil.GetAABB(baseCells, out var bmin, out var bmax);
        // AABB 크기(셀) -> 픽셀/유닛
        int wCells = bmax.x - bmin.x + 1;
        int hCells = bmax.y - bmin.y + 1;

        // dropParent 좌표계로 환산된 '셀 크기'
        var cellSizeInDrop = GetCellSizeIn(dropParent);

        float widthLocal = wCells * cellSizeInDrop.x;
        float heightLocal = hCells * cellSizeInDrop.y;
        // 부모 크기 = AABB 크기
        //var slotRect0 = slotGrid[0, 0].GetComponent<RectTransform>(); // 슬롯 한 칸 크기 얻으려고(같으면 임의 0,0)
        //float cellW = slotRect0.rect.width;
        //float cellH = slotRect0.rect.height;
        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthLocal);
        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightLocal);

        // 피벗 설정: 0도 AABB 기준으로 "피벗 셀의 좌하단" 위치가 되도록
        Vector2Int basePivotCell = baseCells[it.pivotIndex];
        Vector2 pivotNorm = new Vector2(
            (float)(basePivotCell.x - bmin.x) / wCells,
            (float)(basePivotCell.y - bmin.y) / hCells
        );
        itemRect.pivot = pivotNorm;

        // 부모 Rect 자체 회전 적용
        itemRect.localRotation = Quaternion.Euler(0, 0, -90f * isComp.currentRot);

        // ── 6) 월드 위치 맞추기:
        //     (origin + "회전된" pivot 셀)의 "좌하단 슬롯 월드 좌표"에
        //     Rect의 피벗이 오도록 배치.
        var pivotCellRot = rotatedCells[it.pivotIndex];
        var slotRT = slotGrid[(origin + pivotCellRot).x, (origin + pivotCellRot).y].GetComponent<RectTransform>();
        Vector3 slotLLWorld = slotRT.TransformPoint(new Vector3(slotRT.rect.xMin, slotRT.rect.yMin, 0f));

        // 로컬 좌하단(피벗 기준 좌표): (-pivot.x*width, -pivot.y*height)
        Vector3 localLL = new Vector3(-pivotNorm.x * widthLocal, -pivotNorm.y * heightLocal, 0f);
        Vector3 rectLLWorld = itemRect.TransformPoint(localLL);

        // 우리가 맞추고 싶은 목표 좌하단 = slotLLWorld
        Vector3 delta = slotLLWorld - rectLLWorld;
        itemRect.position += delta;

        // 시각 상태 복구
        var cg = item.GetComponent<CanvasGroup>() ?? item.AddComponent<CanvasGroup>();
        if (cg) cg.alpha = 1f;
        //cg.blocksRaycasts = true; // 고정 상태에선 슬롯 드래그 막힘
        //var img = item.GetComponent<Image>(); if (img) img.raycastTarget = true;

        // 5. 프리뷰 색 복원
        ClearPreviewTints();

        //Debug.Log($"bl={bl} world={string.Join(" ", world)}");
    }

    private GameObject GetItem(GameObject slotObject)
    {
        // 슬롯 메타 비우고 아이템을 드래그 상태로
        SlotScript slot = slotObject.GetComponent<SlotScript>();
        GameObject item = slot.storedItem;
        if(item == null) return null;

        Vector2Int originPos = slot.storedItemStartPos;
        Vector2Int[] rotated = GetRotatedCells(item);

        // rotated의 좌하단 셀(minCell) 계산
        //int minX = int.MaxValue, minY = int.MaxValue;
        //foreach (var c in rotated) { if (c.x < minX) minX = c.x; if (c.y < minY) minY = c.y; }
        //Vector2Int minCell = new Vector2Int(minX, minY);

        // 해당 아이템이 점유했던 셀 모두 비우기
        foreach (var c in rotated)
        {
            var p = originPos + c;
            if (p.x < 0 || p.y < 0 || p.x >= gridSize.x || p.y >= gridSize.y)
            {
                Debug.LogError($"GetItem OOB: origin={originPos}, c={c}, grid={gridSize}");
                continue;
            }
            var ss = slotGrid[p.x, p.y].GetComponent<SlotScript>();
            ss.storedItem = null;
            ss.storedItemSize = Vector2Int.zero;
            ss.storedItemStartPos = Vector2Int.zero;
            ss.isOccupied = false;
        }

        // 드래그 부모로 이동 + 커서 위치로 스냅
        RectTransform dragParentRect = (RectTransform)dragParent;
        RectTransform itemRect = item.GetComponent<RectTransform>();
        itemRect.SetParent(dragParent, false);
        itemRect.localScale = Vector3.one;
        itemRect.pivot = new Vector2(0.5f, 0.5f);
        item.transform.SetAsLastSibling(); // 항상 최상단

        // *** (핵심 수정) 여기서 RectTransform 피벗을 스프라이트 피벗과 동기화! ***
        var img = item.GetComponent<Image>();
        if (itemRect && img.sprite != null)
        {
            // 스프라이트 피벗(픽셀)을 RectTransform 피벗(0~1)으로 변환하여 설정
            itemRect.pivot = img.sprite.pivot / img.sprite.rect.size;
        }

        // 스크린 좌표 → 드래그 부모 로컬(anchored)
        Vector2 localMouse;
        var canvas = dragParentRect.GetComponentInParent<Canvas>();
        var cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragParentRect,
            Input.mousePosition,
            cam,
            out localMouse
        );
        itemRect.anchoredPosition = localMouse;

        var cg = item.GetComponent<CanvasGroup>() ?? item.AddComponent<CanvasGroup>();
        if (cg) cg.alpha = 0.5f;
        cg.blocksRaycasts = false;
        img = item.GetComponent<Image>(); if (img) img.raycastTarget = false;

        return item;
    }

    private GameObject SwapItem(GameObject item)
    {
        // overLapOrigin은 ComputePlacementInfo에서 이미 채워짐
        GameObject tempItem = GetItem(overlapSlot);
        StoreItem(item);
        return tempItem;
    }

    private void ResetSelectedButton()
    {
        if (selectedButton != null)
        {
            listManager.RemoveButton(selectedButton);
            selectedButton = null;
        }
    }

    // ─────────────────────────────────────────────────────────────
    // 헬퍼: 셀 얻기/회전/보장
    // ─────────────────────────────────────────────────────────────
    public static void EnsureCellsCached(ObjeData it)
    {
        if (it == null) return;

        // 1. maskRows가 있으면 항상 거기서 다시 만들기(에디터에 남은 shapeCells 무시)
        if (it.maskRows != null && it.maskRows.Length > 0)
        {
            // 항상 maskRows로 다시 생성
            it.shapeCells = ShapeUtil.MaskToCells(it.maskRows);

            // 정규화된 폭/높이로 피벗 인덱스 계산
            int h = it.maskRows.Length;
            int w = ShapeUtil.NormalizedWidth(it.maskRows);
            
            int px = Mathf.Clamp(it.pivotX, 0, Mathf.Max(0, w - 1));
            int py = Mathf.Clamp(it.pivotY, 0, Mathf.Max(0, h - 1));

            // MaskToCells는 y를 (h-1-y)로 뒤집어 넣었습니다 -> 동일 좌표계로 변환
            Vector2Int pivotCoord = new Vector2Int(px, (h-1) - py);

            int idx = 0; // 기본 0
            for (int i = 0; i < it.shapeCells.Length; i++)
            {
                if (it.shapeCells[i] == pivotCoord) { idx = i; break; }
            }
            it.pivotIndex = idx;
            return;
        }

        // 2. maskRows가 없다면 shapeCells만 검증
        if(it.shapeCells == null || it.shapeCells.Length == 0)
        {
            it.shapeCells = new[] { Vector2Int.zero };
            it.pivotIndex = 0;
        }
        else
        {
            it.pivotIndex = Mathf.Clamp(it.pivotIndex, 0, it.shapeCells.Length - 1);
        }
    }

    // 그리드 셀 한 칸의 크기를 dropParent 좌표계로 환산
    public Vector2 GetCellSizeIn(Transform targetParent)
    {
        var cell = slotGrid[0, 0].GetComponent<RectTransform>();
        var parentRT = targetParent as RectTransform;

        // 월드상 크기 = 로컬 rect * lossyScale
        Vector2 cellWorldSize = Vector2.Scale(cell.rect.size, cell.lossyScale);
        Vector2 parentWorldScale = parentRT != null ? (Vector2)parentRT.lossyScale : Vector2.one;

        // targetParent 로컬에서의 크기
        return new Vector2(cellWorldSize.x / parentWorldScale.x,
                           cellWorldSize.y / parentWorldScale.y);
    }

    private static Vector2Int[] GetCells(ObjeData it)
    {
        // 항상 EnsureCellsCashed를 통해 최신화
        EnsureCellsCached(it);
        return it.shapeCells != null && it.shapeCells.Length > 0
            ? it.shapeCells
            : new[] { Vector2Int.zero }; // 최소 1셀
    }

    private Vector2Int[] GetRotatedCells(GameObject go)
    {
        var isComp = go.GetComponent<ItemScript>();
        var cells = GetCells(isComp.item);
        return ShapeUtil.GetRotated(cells, isComp.currentRot /* 0~3 */);
    }

    private List<Vector2Int> GetWorldCells(GameObject go)
    {
        var isComp = go.GetComponent<ItemScript>();
        var cells = ShapeUtil.GetRotated(GetCells(isComp.item), isComp.currentRot);

        // 그 아이템의 origin은 해당 셀들 중 아무 슬롯에서든 SlotScript.storedItemStartPos 로 알 수 있음
        // 여기서는 overlapOrigin(스왑 대상) 또는 현재 origin(프리뷰 대상)을 사용
        Vector2Int baseOrigin;

        // 스왑 대상이면 overlapOrigin, 아니면 슬롯에서 가져오자
        // 겹친 대상이면 overLapOrigin을 우선 사용
        if (go == overlapItem)
        {
            baseOrigin = overlapOrigin;
        }
        else
        {
            // 그 외에는 그 아이템이 점유중인 아무 슬롯에서 origin 찾기
            var anySlot = slotGrid.Cast<GameObject>()
                              .Select(o => o.GetComponent<SlotScript>())
                              .FirstOrDefault(s => s.isOccupied && s.storedItem == go);
            baseOrigin = (anySlot != null) ? anySlot.storedItemStartPos : origin;
        }

        return cells.Select(c => baseOrigin + c).ToList();
    }

    // 범용: 시작좌표(startPos)에서 size 범위를 color 로 칠함
    public void HighlightItemCells(Vector2Int startPos, Vector2Int size, Color32 color)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var gx = startPos.x + x;
                var gy = startPos.y + y;
                if (gx < 0 || gy < 0 || gx >= gridSize.x || gy >= gridSize.y) continue;
                slotGrid[gx, gy].GetComponent<Image>().color = color;
            }
        }
    }

    // 편의: 부모 슬롯의 저장 메타(저장 시작좌표/크기)를 읽어 칠함
    public void HighlightItemCells(SlotScript slot, Color32 color)
    {
        HighlightItemCells(slot.storedItemStartPos, slot.storedItemSize, color);
    }
}

// 그리드 하이라이트 색 변환 
public struct ColorHighlights
{
    public static Color32 Green => new Color32(128, 255, 128, 255);
    public static Color32 Yellow => new Color32(255, 255, 64, 255);
    public static Color32 Red => new Color32(255, 128, 128, 255);
    public static Color32 Blue => new Color32(192, 192, 255, 255);
}
