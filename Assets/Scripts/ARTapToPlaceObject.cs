using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject placementIndicator;
    // public GameObject objToSpawn;
    [SerializeField] private TextMeshProUGUI errorText;

    private Pose PlacementPose; // Stores position + rotation data
    private ARRaycastManager raycastManager;
    private bool placementPoseIsValid = false;
    private Touch touch;

    //private bool hold = false;
    private Transform activeObject = null;
    private Vector3 translationVector;
    private float speedModifier = 0.0005f;

    private void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        errorText.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Drag object
            if (touch.phase == TouchPhase.Moved && activeObject != null)
            {
                MoveObject();
            }

            // Only proceed if it's the beginning of a touch
            if (touch.phase == TouchPhase.Began)
            {
                // If there is a valid placement pose, spawn an object at that location
                if (placementPoseIsValid)
                {
                    PlaceObject();
                }
                /*
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
 
                // If touch an object that is already placed
                if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Furniture"))
                {
                    hold = true;
                    activeObject = hit.transform;
                }
                */
            }

            // Release touch
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
 
                // If touch an object that is already placed
                if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Furniture"))
                {
                    if (activeObject != null)
                    {
                        activeObject.gameObject.GetComponent<Outline>().enabled = false;
                    }

                    activeObject = hit.transform;
                    activeObject.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    activeObject.gameObject.GetComponent<Outline>().enabled = false;
                    activeObject = null;
                }
            }
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

    private IEnumerator HideDebugMessageAfterDelay()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Disable the debug text after 3 seconds
        errorText.gameObject.SetActive(false);
    }

    private void PlaceObject()
    {
        // Check for any colliders in the area where you want to spawn the furniture using an overlap sphere
        Collider[] colliders = Physics.OverlapSphere(placementIndicator.transform.position, 0.5f); // Adjust the radius as needed

        // Loop through all colliders to check if any belong to another furniture object
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Furniture"))
            {
                errorText.text = "Another furniture object is in the way.";
                errorText.gameObject.SetActive(true);
                // Start the coroutine to hide the debug message after 3 seconds
                StartCoroutine(HideDebugMessageAfterDelay());
                return; // Exit the method without spawning the object
            }
        }
        errorText.text = "Place object";
        errorText.gameObject.SetActive(true);
        // Start the coroutine to hide the debug message after 3 seconds
        StartCoroutine(HideDebugMessageAfterDelay());
        /**
         * ASSIGNMENT 3 HINT
         * Can we set the obj to spawn based on the furniture we choose? That way we can spawn the furniture selected during runtime
         */
        GameObject furnitureObject = Instantiate(DataHandler.Instance.GetFurniture(), PlacementPose.position, PlacementPose.rotation);

        // Disable outline when furniture is spawned
        furnitureObject.GetComponent<Outline>().enabled = false;

        // Set the initial scale to zero
        furnitureObject.transform.localScale = Vector3.zero;

        // Start the animation coroutine to scale the furniture object
        StartCoroutine(LerpObjectScale(Vector3.zero, Vector3.one, 0.5f, furnitureObject));

        // Add Rigidbody component to enable physics interactions with default settings
        Rigidbody rb = furnitureObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 100.0f;

        // Tag the spawned furniture object so that we can identify it later
        furnitureObject.tag = "Furniture";
    }

    private void MoveObject()
    {
        // Convert X-Y touch movement to object translation in world space
        translationVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        activeObject.Translate(translationVector * Input.GetTouch(0).deltaPosition.y * speedModifier, Space.World);

        translationVector = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
        activeObject.Translate(translationVector * Input.GetTouch(0).deltaPosition.x * speedModifier, Space.World);
    }

    public void DeleteObject()
    {
        if (activeObject != null) 
        {
            activeObject.gameObject.SetActive(false);
        }
    }
}
