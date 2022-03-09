using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DeleteButton : MonoBehaviour
{
    private Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(DeleteObject);
    }

    void DeleteObject()
    {
        Destroy(ARTapToPlaceObject.Instance.selectedFurniture);
    }
    
}
