using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableObject : MonoBehaviour
{

    public Transform destination;
    private KeyboardController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerControls").GetComponent<KeyboardController>();
        destination = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider c) 
    {
        Debug.Log("TriggerEnter");
        if(c.gameObject.CompareTag("Player")) 
        {
            player.climbableObject = this.gameObject.GetComponent<ClimbableObject>();
        }
    }

    private void OnTriggerExit(Collider c) 
    {
        Debug.Log("TriggerExit");
        if(c.gameObject.CompareTag("Player")) 
        {
            player.climbableObject = null;
        }
    }
}
