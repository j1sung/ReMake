using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics;

public class InventoryManager : MonoBehaviour
{   
    public static InventoryManager instance;
    
    [Header("UI")]
    [SerializeField] private Transform contentRoot;        // ScrollView/Viewport/Content
    //[SerializeField] private GameObject itemIconPrefab;    // InventoryItemIcon 프리팹

    public ObjectPoolScript itemButtonPool;
    public ObjectPoolScript itemEquipPool;
    public InvenGridManager invenManger;

    private List<ObjeData> itemList = new List<ObjeData>();
    public List<ObjeData> objeList = new List<ObjeData>();  // 인벤토리 업적용 리스트

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void DumpList(string tag)
    {
        var items = itemList.ConvertAll(x => x == null ? "<NULL>" :
            (string.IsNullOrWhiteSpace(x.itemType) ? "<EMPTY>" : x.itemType.Trim()));
        UnityEngine.Debug.Log($"{tag} :: Count={itemList.Count} [{string.Join("|", items)}]");
    }
    /*
    public void AddObje(ObjeData data)
    {
        objes.Add(data);

        // 아이콘 프리팹 생성
        GameObject icon = Instantiate(itemIconPrefab, contentRoot);
        Image img = icon.GetComponent<Image>();
        if (img != null) img.sprite = data.iconImage;
    }*/

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
        ObjeData it = ItemScript.selectedItem.GetComponent<ItemScript>()?.item; // 드래그 중인 선택된 퍼즐 정보
        if (it != null)
        {
            string key = it.itemType?.Trim();

            // 조건 걸기: 해당 itemType 의 버튼이 이미 없을 때만 AddButton
            bool alreadyExists = itemList.Exists(x => x != null && x.itemType?.Trim() == key); // 선택된 퍼즐 정보가 리스트에 있는지 판단
            if (!alreadyExists)
            {
                AddButton(it);
            }
        }
        

        // 풀로 반환하고 드래그 상태 해제
        itemEquipPool.ReturnObject(ItemScript.selectedItem);
        ItemScript.ResetSelectedItem();

        // (선택) 그리드 하이라이트 정리
        invenManger.RefrechColor(false);
    }

    // 버튼 생성
    public void AddButton(ObjeData data)
    {
        string key = data.itemType?.Trim()??"";

        // 중복 방지 가드
        if(itemList.Exists(x=>x!=null && x.itemType?.Trim() == key))
        {
            var st = new StackTrace(1, true); // 호출자 추적
            UnityEngine.Debug.LogWarning($"[AddButton] DUP SKIPPED: {key}\ncalled by {st}");
            return;
        }

        DumpList("[AddButton] before");
        itemList.Add(data); // 인벤토리 리스트에 오브제 추가
        GameObject newButton = itemButtonPool.GetObject(); // 버튼 풀링 생성
        newButton.transform.SetParent(contentRoot, false); // 버튼 계층 설정
        newButton.GetComponent<RectTransform>().localScale = Vector3.one;
        newButton.GetComponent<ItemButtonScript>().SetUpButton(data, this); // 버튼 세팅
        DumpList("[AddButton] after");
    }

    // 버튼 제거
    public void RemoveButton(GameObject buttonObj)
    {   
        buttonObj.GetComponent<CanvasGroup>().alpha = 1f;
        itemButtonPool.ReturnObject(buttonObj);
        RemoveItemFromList(buttonObj.GetComponent<ItemButtonScript>().item); // (선택)오브제 리스트를 비울 필요 있으면 활성화!
    }

    // 오브제 리스트 비우기
    public void RemoveItemFromList(ObjeData itemToRemove)
    {
        if (itemToRemove == null) return;
        string key = itemToRemove.itemType?.Trim();
        int removed = itemList.RemoveAll(x=> x!=null && x.itemType?.Trim() == key);
        UnityEngine.Debug.Log($"RemoveItemFromList: type={key}, removed={removed}");
    }
}
