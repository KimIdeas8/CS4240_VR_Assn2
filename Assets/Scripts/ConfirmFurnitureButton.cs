using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmFurnitureButton : MonoBehaviour
{
    private Button btn;

    private static ConfirmFurnitureButton instance;
    public static ConfirmFurnitureButton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ConfirmFurnitureButton>();
            }
            return instance;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(UnselectFurniture);
    }

    // Update is called once per frame
    public void UnselectFurniture()
    {
        ARTapToPlaceObject.Instance.dragging = false; //prevent the dragging of the furniture
        ARTapToPlaceObject.Instance.selectedFurniture.transform.GetChild(0).gameObject.transform.Find("Outline").gameObject.SetActive(false); //de-select the furniture
        ARTapToPlaceObject.Instance.selectedFurniture = null;
        ARTapToPlaceObject.Instance.selectedFurnitureReference = null;
    }
}
