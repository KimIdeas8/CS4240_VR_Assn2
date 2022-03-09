using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPrevent : MonoBehaviour
{
    public bool moveSelectedFurnitureBack = false;

    public static CollisionPrevent instance;
    public static CollisionPrevent Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CollisionPrevent>();
            }
            return instance;

        }
    }

    //if there is a collision, then set to true
    void OnCollisionEnter(Collision collision)
    {
        /*
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        //if collide with instantiated furniture, then destroy the instantiated furniture and stop this object from moving
        if (collision.gameObject == ARTapToPlaceObject.Instance.instantiatedFurniture)
        {
            Destroy(ARTapToPlaceObject.Instance.instantiatedFurniture);
        }
        //if collide with selected furniture, then move the selected furniture back to orginal position and stop this object from moving
        else if (collision.gameObject == ARTapToPlaceObject.Instance.selectedFurniture)
        {
            moveSelectedFurnitureBack = true;
        }
        */
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (collision.gameObject == ARTapToPlaceObject.Instance.instantiatedFurniture && collision.gameObject == ARTapToPlaceObject.Instance.selectedFurniture)
        {
            moveSelectedFurnitureBack = true;
        }
        else if (collision.gameObject == ARTapToPlaceObject.Instance.instantiatedFurniture)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject == ARTapToPlaceObject.Instance.selectedFurniture && gameObject == ARTapToPlaceObject.Instance.instantiatedFurniture)
        {
            moveSelectedFurnitureBack = true;
        }
        else if (collision.gameObject == ARTapToPlaceObject.Instance.selectedFurniture)
        {
            moveSelectedFurnitureBack = true;
        }
    }
}
