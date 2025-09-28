using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ItemButtonScript : MonoBehaviour {

    public Button buttonComponent;
    public Image iconImage;

    public ItemClass item;
    private Sprite iconSprite;
    private Sprite puzzleSprite;
    private ItemListManager listManager;
    public ObjectPoolScript itemEquipPool;

    public static InvenGridManager invenManager;

    private void Start()
    {
        buttonComponent.onClick.AddListener(SpawnStoredItem);
    }

    // 버튼 클릭시 퍼즐 스폰
    private void SpawnStoredItem()
    {
        // 이미 다른 선택 아이템이 있으면 기존 드래그 퍼즐 회수
        if (ItemScript.selectedItem != null)
        {
            invenManager.selectedButton.GetComponent<CanvasGroup>().alpha = 1f;
            listManager.itemEquipPool.ReturnObject(ItemScript.selectedItem);
            ItemScript.ResetSelectedItem();
        }

        GameObject newItem = itemEquipPool.GetObject(); // 선택한 버튼의 퍼즐 프리펩 스폰

        // 데이터 세팅
        var isComp = newItem.GetComponent<ItemScript>();
        isComp.item = item;

        // 풀 회전 잔상 초기화 -> 이미지 회전도 초기화 해야함
        isComp.currentRot = 0;
        var r = newItem.GetComponent<RectTransform>();
        if (r) r.localRotation = Quaternion.identity;
        
        // 이미지 넣기
        var img = newItem.GetComponent<Image>();
        img.sprite = listManager.SetPuzzleSprite(item.itemType);
        img.preserveAspect = true;

        // *** (핵심 수정) 여기서 RectTransform 피벗을 스프라이트 피벗과 동기화! ***
        var rect = newItem.GetComponent<RectTransform>();
        if (rect && img.sprite != null)
        {
            // 스프라이트 피벗(픽셀)을 RectTransform 피벗(0~1)으로 변환하여 설정
            rect.pivot = img.sprite.pivot / img.sprite.rect.size;
        }

        // 퍼즐 바운딩(rect) 사이즈 설정
        isComp.ResizeToCurrentShape(); 

        // 드래그 레이어로 올리기
        newItem.transform.SetParent(GameObject.FindGameObjectWithTag("DragParent").transform, false);
        newItem.GetComponent<RectTransform>().localScale = Vector3.one;

        // 드래그 상태로 전환
        ItemScript.SetSelectedItem(newItem);
        ItemScript.justSpawned = true; // 첫 프레임 면책
        invenManager.selectedButton = this.gameObject;

        // 버튼 희미하게
        GetComponent<CanvasGroup>().alpha = 0.5f;

        // 드래그 중인 아이템은 반투명 & 레이캐스트 꺼서 슬롯이 이벤트 받도록
        var cg = newItem.GetComponent<CanvasGroup>() ?? newItem.AddComponent<CanvasGroup>();
        cg.alpha = 0.5f;              // 바로 반투명
        cg.blocksRaycasts = false;     // 드래그 중엔 슬롯/섹터가 포인터 이벤트를 받도록
        if (img) img.raycastTarget = false; 
    }

    public void SetUpButton(ItemClass passedItem, ItemListManager passedListManager)
    {
        listManager = passedListManager;
        item = passedItem;

        // 아이콘 이미지 설정
        iconImage.sprite = listManager.SetIconSprite(item.itemType);

        // 리스트 쪽 레이아웃 높이 조절(정사각형 느낌)
        GetComponent<LayoutElement>().preferredHeight = transform.parent.GetComponent<RectTransform>().rect.width / 4f;

        itemEquipPool = passedListManager.itemEquipPool;
    }
    
    
}
