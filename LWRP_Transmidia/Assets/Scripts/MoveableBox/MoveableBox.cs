using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBox : MonoBehaviour
{

    public bool placed = false;
    private bool runOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(placed && !runOnce) 
        {
            runOnce = true;
            transform.GetChild(0).gameObject.GetComponent<DetectPlayer>().ActiveObj(false);
        }
    }
}
