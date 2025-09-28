using System.Collections.Generic;
using UnityEngine;

public class ItemListManager : MonoBehaviour {

    public ObjectPoolScript itemButtonPool;
    public ObjectPoolScript itemEquipPool;
    public InvenGridManager invenManger;

    public Sprite[] itemIconArr;
    public Sprite[] puzzleArr;
    public List<ItemClass> itemList;

    private Transform contentPanel;

    private void Start()
    {
        contentPanel = transform;
        RefreshList();
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > GetComponent<Transform>().position.x) // drop item back
        {
            if (invenManger.selectedButton == null && ItemScript.selectedItem != null) //dropping back from ivenGrid
            {
                itemList.Add(ItemScript.selectedItem.GetComponent<ItemScript>().item);
                AddButton(ItemScript.selectedItem.GetComponent<ItemScript>().item);// shorten later
                itemEquipPool.ReturnObject(ItemScript.selectedItem);
                ItemScript.ResetSelectedItem();
            } 
        }

        if (Input.GetMouseButtonDown(1) && invenManger.selectedButton != null) //delesect selected item and button  by right-click
        {
            invenManger.RefrechColor(false);// not refresh color to blue when mouse is non top of occupied slot after putting back itemEquip
            invenManger.selectedButton.GetComponent<CanvasGroup>().alpha = 1f;
            invenManger.selectedButton = null;
            itemEquipPool.ReturnObject(ItemScript.selectedItem);
            ItemScript.ResetSelectedItem();

        }
        */
    }

    
    // 되돌리기 공용 함수
    public void ReturnSelectedToList()
    {
        // 드래그 중인 퍼즐 없으면 바로 리턴
        if (ItemScript.selectedItem == null) return;

        // 버튼 선택 표시 복구
        if (invenManger.selectedButton != null)
        {
            var cg = invenManger.selectedButton.GetComponent<CanvasGroup>();
            if (cg) cg.alpha = 1f;
            invenManger.selectedButton = null;
        }

        // 드래그 중인 퍼즐의 ItemClass 추출해서 리스트에 복귀
        var it = ItemScript.selectedItem.GetComponent<ItemScript>()?.item;
        if (it != null)
        {
            // 조건 걸기: 해당 itemType 의 버튼이 이미 없을 때만 AddButton
            bool alreadyExists = itemList.Exists(x => x.itemType == it.itemType);
            if (!alreadyExists)
            {
                itemList.Add(it);
                AddButton(it);
            }
        }

        // 풀로 반환하고 드래그 상태 해제
        itemEquipPool.ReturnObject(ItemScript.selectedItem);
        ItemScript.ResetSelectedItem();

        // (선택) 그리드 하이라이트 정리
        invenManger.RefrechColor(false);
    }
    

    private void RefreshList()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            AddButton(itemList[i]);
        }
    }

    // 버튼 생성
    private void AddButton(ItemClass addItem)
    {
        GameObject newButton = itemButtonPool.GetObject();
        newButton.transform.SetParent(contentPanel, false);
        newButton.GetComponent<RectTransform>().localScale = Vector3.one;
        newButton.GetComponent<ItemButtonScript>().SetUpButton(addItem, this);
    }

    public void RemoveButton(GameObject buttonObj)
    {
        buttonObj.GetComponent<CanvasGroup>().alpha = 1f;
        itemButtonPool.ReturnObject(buttonObj);
        RevomeItemFromList(buttonObj.GetComponent<ItemButtonScript>().item);
    }

    public void RevomeItemFromList(ItemClass itemToRemove)//may add a list variable when sort list is added
    {
        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            if (itemList[i] == itemToRemove)
            {
                itemList.RemoveAt(i);
                break;//temporary for now
            }
        }
    }

    // 아이콘 이미지 임시 넣음
    public Sprite SetIconSprite(string s) 
    {
        switch (s)
        {
            case "Muffler": return itemIconArr[1];
            case "Lipstick": return itemIconArr[2];
            case "Letter": return itemIconArr[3];

            default: return itemIconArr[0];
        }
    }

    // 퍼즐 이미지 임시 넣음
    public Sprite SetPuzzleSprite(string s)
    {
        switch (s)
        {
            case "Muffler": return puzzleArr[1];
            case "Lipstick": return puzzleArr[2];
            case "Letter": return puzzleArr[3];

            default: return puzzleArr[0];
        }
    }
}
