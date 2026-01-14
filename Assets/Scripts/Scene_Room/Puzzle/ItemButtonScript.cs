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

    public ObjeData item;
    private Sprite iconSprite;
    private Sprite puzzleSprite;
    private InventoryManager listManager;
    public ObjectPoolScript itemEquipPool;

    public static InvenGridManager invenManager;

    private void Start()
    {
        buttonComponent.onClick.AddListener(SpawnStoredItem);
    }

    public void OnPointerClick(PointerEventData e)
    {
        Debug.Log("Button got click: " + name);
    }

    // 버튼 클릭시 퍼즐 스폰
    private void SpawnStoredItem()
    {
        // 룸이면 퍼즐 안만들고 사운드 재생만
        if (RoomUIManager.instance.State == UIState.Room)
        {
            SFXPlayer.Instance.PlaySFX(item.objeSound);
            return;
        }

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

        var dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
        var rt = newItem.GetComponent<RectTransform>();
        rt.SetParent(dragParent, false);
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
 
        
        // 이미지 넣기
        var img = newItem.GetComponent<Image>();
        img.sprite = item.puzzleImage;
        img.preserveAspect = true;

        // RectTransform 피벗을 스프라이트 피벗과 동기화!
        if (img.sprite != null)
        {
            // 스프라이트 피벗(픽셀)을 RectTransform 피벗(0~1)으로 변환하여 설정
            rt.pivot = img.sprite.pivot / img.sprite.rect.size;
        }

        // 여기서 바로 마우스 위치로 스냅
        var dragParentRect = (RectTransform)dragParent;
        var canvas = dragParentRect.GetComponentInParent<Canvas>();
        var cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

        Vector2 localMouse;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragParentRect, Input.mousePosition, cam, out localMouse
        );
        rt.anchoredPosition = localMouse;


        // 풀 회전 잔상 초기화 -> 이미지 회전도 초기화 해야함
        isComp.currentRot = 0;

        // 퍼즐 바운딩(rect) 사이즈 설정
        isComp.ResizeForParent(dragParent, invenManager);


        // 드래그 상태로 전환
        ItemScript.SetSelectedItem(newItem);
        ItemScript.justSpawned = true; // 첫 프레임 면책
        invenManager.selectedButton = this.gameObject;

        // 버튼 희미하게
        GetComponent<CanvasGroup>().alpha = 0.5f;

        // 드래그 중인 아이템은 반투명 & 레이캐스트 꺼서 슬롯이 이벤트 받도록
        var cg = newItem.GetComponent<CanvasGroup>() ?? newItem.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;     // 드래그 중엔 슬롯/섹터가 포인터 이벤트를 받도록
        if (img) img.raycastTarget = false; 
    }

    public void SetUpButton(ObjeData data, InventoryManager passedListManager)
    {
        listManager = passedListManager;
        item = data;

        // 아이콘 이미지 설정
        iconImage.sprite = item.iconImage;

        // 리스트 쪽 레이아웃 높이 조절(정사각형 느낌)
        //GetComponent<LayoutElement>().preferredHeight = transform.parent.GetComponent<RectTransform>().rect.width / 4f;

        itemEquipPool = passedListManager.itemEquipPool;
    }
    
    
}
