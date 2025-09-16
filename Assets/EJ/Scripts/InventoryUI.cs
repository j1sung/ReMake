using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Transform content;         // ScrollRect의 Content
    [SerializeField] GameObject itemSlotPrefab; // Image 1개짜리 프리팹
    [SerializeField] ScrollRect scroll;         // Scroll View의 ScrollRect(선택)

    // 스프라이트 1장 추가
    public void AddItem(Sprite icon)
    {
        var go = Instantiate(itemSlotPrefab, content);
        var img = go.GetComponent<Image>();     // 프리팹 루트가 Image라고 가정
        if (img != null)
        {
            img.sprite = icon;
            img.preserveAspect = true;
            img.raycastTarget = true;           // 슬롯 클릭 받고 싶으면 true 유지
        }

        // 최신 아이템이 보이도록 살짝 아래로 스크롤 (옵션)
        if (scroll != null)
        {
            Canvas.ForceUpdateCanvases();
            scroll.verticalNormalizedPosition = 0f; // 0=맨 아래
        }
    }

    public void ClearAll()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);
    }
}