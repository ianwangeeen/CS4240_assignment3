using System.Collections;
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

    private IEnumerator LerpObjectScale(Vector3 a, Vector3 b, float time, GameObject lerpObject)
    {
        float i = 0.0f;
        float rate = (1.0f / time);
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            lerpObject.transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }   

    private void PlaceObject()
    {
        /**
         * ASSIGNMENT 3 HINT
         * Can we set the obj to spawn based on the furniture we choose? That way we can spawn the furniture selected during runtime
         */
        GameObject furnitureObject = Instantiate(DataHandler.Instance.GetFurniture(), PlacementPose.position, PlacementPose.rotation);

        // Set the initial scale to zero
        furnitureObject.transform.localScale = Vector3.zero;

        // Start the animation coroutine to scale the furniture object
        StartCoroutine(LerpObjectScale(Vector3.zero, Vector3.one, 0.5f, furnitureObject));
    }
}
