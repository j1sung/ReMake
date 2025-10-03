using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotSectorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public GameObject slotParent;

    //public int QuadNum;
    //public static Vector2Int posOffset;
    //public static SlotSectorScript sectorScript;
    private InvenGridManager invenGridManager;
    private SlotScript parentSlotScript;

    private void Start()
    {
        invenGridManager = transform.parent.parent.GetComponent<InvenGridManager>();
        parentSlotScript = slotParent.GetComponent<SlotScript>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스가 어떤 슬롯 위에 있는지 알리기
        if(invenGridManager.highlightedSlot != slotParent)
            invenGridManager.highlightedSlot = slotParent;

        // 선택 아이템이 있으면 미리보기 갱신
        if(ItemScript.selectedItem != null)
        {
            // 드래그중이면 프리뷰 갱신
            invenGridManager.RefrechColor(true);
        }
        // 아이템이 슬롯에 저장되어있고, 드래그 중이 아니면 파랗게 하이라이트
        if (parentSlotScript.storedItem != null && ItemScript.selectedItem == null)
        {
            
            invenGridManager.HighlightItemCells(parentSlotScript, ColorHighlights.Blue);
        }
            
        /*
        sectorScript = this;
        PosOffset();

        if (invenGridManager.highlightedSlot != slotParent)
        {
            invenGridManager.highlightedSlot = slotParent;
        }
        if (ItemScript.selectedItem != null)
        {
            invenGridManager.RefrechColor(true);
        }
        if (parentSlotScript.storedItem != null && ItemScript.selectedItem == null)
        {
            invenGridManager.ColorChangeLoop(ColorHighlights.Blue, parentSlotScript.storedItemSize, parentSlotScript.storedItemStartPos);
        }
        */
    }
    /*
    public void PosOffset()
    {
        if (ItemScript.selectedItemSize.x != 0 && ItemScript.selectedItemSize.x % 2 == 0)
        {
            switch (QuadNum)
            {
                case 1:
                    posOffset.x = 0; break;
                case 2:
                    posOffset.x = -1; break;
                case 3:
                    posOffset.x = 0; break;
                case 4:
                    posOffset.x = -1; break;
                default: break;
            }
        }
        if (ItemScript.selectedItemSize.y != 0 && ItemScript.selectedItemSize.y % 2 == 0)
        {
            switch (QuadNum)
            {
                case 1:
                    posOffset.y = -1; break;
                case 2:
                    posOffset.y = -1; break;
                case 3:
                    posOffset.y = 0; break;
                case 4:
                    posOffset.y = 0; break;
                default: break;
            }
        }
    }*/

    public void OnPointerExit(PointerEventData eventData)
    {
        //sectorScript = null;
        invenGridManager.highlightedSlot = null;

        if (ItemScript.selectedItem != null)
        {
            // 프리뷰 리셋
            invenGridManager.RefrechColor(false);
        }
        else if(parentSlotScript.storedItem != null)
        {
            // 원래 색으로 복귀
            invenGridManager.HighlightItemCells(parentSlotScript, Color.white);
        }
        /*
        posOffset = Vector2Int.zero;
        if (parentSlotScript.storedItem != null && ItemScript.selectedItem == null)
        {
            invenGridManager.ColorChangeLoop(Color.white, parentSlotScript.storedItemSize, parentSlotScript.storedItemStartPos);
        }
        */
    }
}
