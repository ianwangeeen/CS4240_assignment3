using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedFurnitureManager : MonoBehaviour
{
    private GraphicRaycaster _raycaster;
    public Transform selectedFurniturePoint;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private static SelectedFurnitureManager instance;
    public static SelectedFurnitureManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<SelectedFurnitureManager>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        pointerEventData = new PointerEventData(eventSystem);

        // draw raycast on the background selection pane and find out which furniture it is pointing to
        pointerEventData.position = selectedFurniturePoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool OnEntered(GameObject button) {
        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);

        foreach (var result in results) {
            // if the furniture button is being hit by the raycast which is casted from the centre ui guy, return true
            if (result.gameObject == button) {
                return true;
            }
        }
        return false;
    }
}
