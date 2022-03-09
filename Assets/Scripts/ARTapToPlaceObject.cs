using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ARTapToPlaceObject : MonoBehaviour
{

    public GameObject placementIndicator;

    public GameObject selectedFurniture;
    public Transform selectedFurnitureReference;
    //private float speedModifier = 0.01f;
    private float speedModifier = 1f;
    public GameObject instantiatedFurniture;

    private Touch touch;
    public bool dragging = false;
    private Vector3 touchPosition;
    private Vector3 backPosition;

    private Pose PlacementPose; //data structure that contains a Vector3 for position and a Quaternion for rotation. 
                                // tell us where to set the placement indicator
    private ARRaycastManager aRRaycastManager; // one of the components we added to AR Session Origin earlier
                                               // and will help us handle raycasts
                                               // do a raycast and find the first surface hit (imagine shining a ray of light to see which surfaces it collides with).
    private bool placementPoseIsValid = false; // signifies if there is a valid plane for the placement indicator to be on


    public static ARTapToPlaceObject instance;
    public static ARTapToPlaceObject Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ARTapToPlaceObject>();
            }
            return instance;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>(); // get the AR Raycast Manager component
            
    }

    // Update is called once per frame
    void Update()
    {

        UpdatePlacementPose();
        UpdatePlacementIndicator();

        touch = Input.GetTouch(0);
        instantiatedFurniture = null;

        //whenever the screen is tapped, either select/de-select an existing furniture or spawn a new furniture
        if (Input.touchCount>0 && touch.phase == TouchPhase.Began && !IsPointerOverUI(touch))
        {
            //if there is a furniture at position of touch, select it (if no other furniture is selected)
            RaycastHit hit;
            Ray ray = Camera.current.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit))
            {
                //if no furniture is selected, then select the furniture which is tapped on
                if (selectedFurniture == null)
                {
                    selectedFurniture = hit.transform.gameObject; //assign to the new furniture
                    selectedFurnitureReference = hit.transform; //for moving the new furniture
                    selectedFurniture.transform.GetChild(0).gameObject.transform.Find("Outline").gameObject.SetActive(true); //select the new furniture
                }
                dragging = true;
            }
            //if there is no furniture (hit), and there is a valid plane, and there is no collisions, spawn the furniture
            else if (placementPoseIsValid)
            {
                instantiatedFurniture = Instantiate(DataHandler.Instance.furniture, PlacementPose.position, PlacementPose.rotation); //spawns the object at the placement pose position and rotation
                //PlaceFurniture();
            }
            
        }

        //if there is dragging across the screen, drag the furniture (if selected and no collisions)
        if (dragging && touch.phase == TouchPhase.Moved)
        {
            //if collision, move the selected furniture backwards and stop it from moving (due to impact from collision)
            if (CollisionPrevent.Instance.moveSelectedFurnitureBack)
            {
                CollisionPrevent.Instance.moveSelectedFurnitureBack = false;
                
                backPosition = new Vector3(1f, 0, 1f);
                selectedFurniture.GetComponent<Rigidbody>().position = selectedFurnitureReference.position - backPosition;
                selectedFurniture.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ConfirmFurnitureButton.Instance.UnselectFurniture();
            }
            //drag furniture with touch
            else
            {
                touchPosition = new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y);
                selectedFurniture.GetComponent<Rigidbody>().MovePosition(selectedFurnitureReference.position + touchPosition * Time.deltaTime * speedModifier);
                
            }
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes); // do a raycast and find the first surface hit

        placementPoseIsValid = hits.Count > 0; //as long as there are hits, there is at least one valid plane
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose; //info on where to set the placement indicator (at valid plane)
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false); //placement indicator will disappear from screen
        }
    }

    private void PlaceFurniture()
    {
        Instantiate(DataHandler.Instance.furniture, PlacementPose.position, PlacementPose.rotation); //spawns the object at the placement pose position and rotation
    }

    bool IsPointerOverUI(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}

