using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectPlacement : MonoBehaviour
{

    private GameObject placedObject;
    private KeyboardController keyboardController;

    private void Awake() 
    {
        keyboardController = GameObject.FindGameObjectWithTag("PlayerControls").GetComponent<KeyboardController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placedObject = null;
    }

    private void OnTriggerStay(Collider c) 
    {
        if(placedObject == null) 
        {
            if(c.gameObject.CompareTag("MoveableCube")) 
            {
                placedObject = c.gameObject;
                c.transform.SetParent(this.transform);
                c.transform.localPosition = Vector3.zero + new Vector3(0.5f, 0.5f, 0.5f);
                c.transform.localEulerAngles = Vector3.zero;
                c.gameObject.GetComponent<MoveableBox>().placed = true;
                keyboardController.ClearAttachedObj();
            }
        }
    }
}
