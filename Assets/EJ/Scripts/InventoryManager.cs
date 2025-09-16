using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{   
    public static InventoryManager instance;
    
    [Header("UI")]
    [SerializeField] private Transform contentRoot;        // ScrollView/Viewport/Content
    [SerializeField] private GameObject itemIconPrefab;    // InventoryItemIcon 프리팹

    private List<ObjeData> objes = new();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddObje(ObjeData data)
    {
        objes.Add(data);

        // 아이콘 프리팹 생성
        GameObject icon = Instantiate(itemIconPrefab, contentRoot);
        Image img = icon.GetComponent<Image>();
        if (img != null) img.sprite = data.icon;
    }
}
