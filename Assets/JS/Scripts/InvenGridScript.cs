using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenGridScript : MonoBehaviour {

    /*to do list
     * create inventiry grid (done)
     * add panels (done)
     * dynamic inventory functions (done)
     * 
     * make test items (done)
     * move items  (done)
     * drop items (done)
     * retrieve items (done)
     * swap items (done)
     * drag checking highlighting colors (done)
     * rewrite color highlighting *too long/ hard to read* (done-ish) *****
     * 
     * make scroll list UI for items (done)
     * item buttons (done)
     * spawn item equip forn buttons (done)
     * remove item button from list when putting item on grid (done)
     * button object pool | button and item equip (done)
     * drop items back to list (done) ***slight delay when drooping back to list from invenGrid
     * add  delete item panel 
     * 
     * add item stat 
     * add item stat overlay 
     * 
     * quality will change backgroung color instead of text
     * 
     * make a whole item class inheritance "weapon, armor"
     * make the StatPanel dynamix size when adding more stats
     * add more item type, name and icons
     * 
     * have item stat affect player stats *later*
     * create item generator
     * make random item generator *later*
     * make item on grid glow green when no seletecItem (done)
     */

    /*optionals
     * create odd shaped items *very hard. require rewrite of whole thing*
     * add graphics
     * item rotate
     * add warning pop-up when deleting high quality items
     * save/load function *hard/no knowledge*
     * improve IntVector2 methods and parameters *ongoing*
     * add sort list
     * add sort grid *hard*
     */

    public GameObject[,] slotGrid;
    public GameObject slotPrefab;
    public IntVector2 gridSize;
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
                
                obj.transform.name = "slot[" + x + "," + y + "]";
                obj.transform.SetParent(this.transform, false); // worldPositionStays = false

                RectTransform rect = obj.transform.GetComponent<RectTransform>();

                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                // 중앙 기준 배치
                rect.localPosition = new Vector3(
                    startX + x * slotSize,
                    startY + y * slotSize,
                    0f
                );

                obj.GetComponent<RectTransform>().localScale = Vector3.one;
                obj.GetComponent<SlotScript>().gridPos = new IntVector2(x, y);
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
