using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    private GameObject furniture;
    [SerializeField] private UIManager buttonPrefab;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private List<Item> items; // first item: id = 0, 2nd item: id = 1, etc
    [SerializeField] private UIContentFitter contentFitter;
    private int curr_id = 0;

    private static DataHandler instance;

    public static DataHandler Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<DataHandler>();
            }
            return instance;
        }
    }

    private void Start() {
        LoadItems();
        CreateButtons();
        contentFitter.FitContent();
    }

    void LoadItems() {
        var items_obj = Resources.LoadAll("Items", typeof(Item));
        foreach (var item in items_obj) {
            items.Add(item as Item);
        }
    }

    void CreateButtons() {
        foreach (Item item in items) {
            UIManager uiManager = Instantiate(buttonPrefab, buttonContainer.transform);
            uiManager.ItemId = curr_id;
            uiManager.BtnSprite = item.itemImage;
            curr_id++;
        }
    }

    public void SetFurniture(int id) {
        furniture = items[id].itemPrefab;
    }

    public GameObject GetFurniture() {
        return furniture;
    }
}
