using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{

    private MoveableBox moveableBox;
    [SerializeField]
    private GameObject text;

    private void Awake() 
    {
        moveableBox = transform.parent.gameObject.GetComponent<MoveableBox>();
    }

    private void OnTriggerEnter(Collider c) 
    {
        if(c.CompareTag("Player")) 
        {
            if(moveableBox.placed == false) ActiveObj(true);
        }
    }
    private void OnTriggerExit(Collider c) 
    {
        if(c.CompareTag("Player")) 
        {
            ActiveObj(false);
        }
    }

    public void ActiveObj(bool b) 
    {
        text.SetActive(b);
    }
}
