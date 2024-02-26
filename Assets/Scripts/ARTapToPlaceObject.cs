using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    // public GameObject objToSpawn;

    private Pose PlacementPose; // Stores position + rotation data
    private ARRaycastManager raycastManager;
    private bool placementPoseIsValid = false;
    private Touch touch;

    private void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        // touch = Input.GetTouch(0);

        // If no touch has been detected or touch has not began yet, just return
        if (Input.touchCount < 0 || touch.phase != TouchPhase.Began) return;
        // if (IsValidPointer(touch)) return;

        // if there is a valid location + we tap the screen, spawn an item at that location
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    /**
     * Based on where we are pointing towards, is there any planes. 
     */
    void UpdatePlacementPose()
    {
        /* 
            ********************** I COMMENTED THIS OUT COS I COULDNT TEST AND I THOUGH THERE MIGHT BE SOME KIND OF PROBLEM WITH MY CAMERA INITIALISATION WITH LINE 48
            PLS TRY TO UNCOMMENT LINES 47 to 59 COS THATS THE PROF'S CODE **************************
        */
        // // convert viewport position to screen position. Center of screen may not be (0.5, 0.5) since different phones have different sizes
        // var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); 

        // // shoot a ray out from middle of screen to see if it hits anything
        // var hits = new List<ARRaycastHit>();
        // raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        // // is there a plane and are we currently facing it
        // placementPoseIsValid = hits.Count > 0;
        // if (placementPoseIsValid)
        // {
        //     PlacementPose = hits[0].pose;
        // }



        // my code
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        // shoot a ray out from middle of screen to see if it hits anything
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        // is there a plane and are we currently facing it
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    /**
     * Move the placement indicator object
     */
    void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            // if there is a valid plane, activate placement indicator object and make it follow around
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            // no valid place, deactivate
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        /**
         * ASSIGNMENT 3 HINT
         * Can we set the obj to spawn based on the furniture we choose? That way we can spawn the furniture selected during runtime
         */
        Instantiate(DataHandler.Instance.GetFurniture(), PlacementPose.position, PlacementPose.rotation);
    }

    // bool IsValidPointer(Touch touch) {
    //     PointerEventData eventData = new PointerEventData(EventSystem.current);
    //     eventData.position = new Vector2(touch.position.x, touch.position.y);
    //     List<RaycastResult> results = new List<RaycastResult>();
    //     EventSystem.current.RaycastAll(eventData, results);
    //     return results.Count > 0;
    // }
}
