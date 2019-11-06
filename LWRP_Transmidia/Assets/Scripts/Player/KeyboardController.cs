using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class KeyboardController : MonoBehaviour
{
    #region Private Variables
    private Camera movementCamera;
    private Transform player;
    private ThirdPersonCharacter character;
    private GameObject attachedGameObject;
    private Transform scenario;
    private LayerMask moveablesMask;
    #endregion

    #region Serialized Variables
    [SerializeField]
    private Transform sphereNull;
    #endregion

    #region Public Variables
    public ClimbableObject climbableObject;
    #endregion

    private void Awake() 
    {
        movementCamera = GameObject.FindGameObjectWithTag("MovementCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        character = player.GetComponent<ThirdPersonCharacter>();
    }

    private void Start() 
    {
        attachedGameObject = null;
        moveablesMask = LayerMask.GetMask("Moveables");
    }

    private void Update() 
    {
        Movement();
        AttachInput();
        ClimbInput();
    }

    private Vector3 MovementInput() 
    {
        float x,z;
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector3 mov = new Vector3(x, 0, z);
        mov = movementCamera.transform.TransformDirection(mov);
        return mov;
    }

    private void Movement() 
    {
        character.Move(MovementInput(), false, false);
    }

    private void ClimbInput() 
    {
        if(Input.GetButtonDown("Jump")) 
        {
            Debug.Log("Jump Input");
            AttemptToClimb();
        }
    }

    private void AttemptToClimb() 
    {
        if(climbableObject != null) 
        {
            // Play animation.
            Climb();
        }
    }

    private void Climb() 
    {
        Vector3 climbOffset = new Vector3(0.5f, 0, 0.5f);
        player.transform.position = climbOffset+climbableObject.destination.position;
    }

    private void AttachInput() 
    {
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("Attach Input");
            AttemptToAttach();
        }
    }

    private void AttemptToAttach() 
    {
        Debug.Log("Attempted To Attach");
        RaycastHit hit;
        Debug.DrawLine(sphereNull.position, sphereNull.position + sphereNull.forward*1, Color.yellow, 2f);
        if(Physics.Raycast(sphereNull.position, sphereNull.forward, out hit, 1f, moveablesMask))
        {
            if(attachedGameObject != null) 
            {
                Dettach();
            }
            else if(hit.transform.CompareTag("MoveableCube") && hit.transform.gameObject.GetComponent<MoveableBox>().placed == false) 
            {
                Attach(hit.transform.gameObject);
            }
        }
    }

    private void Attach(GameObject go) 
    {
        attachedGameObject = go;
        attachedGameObject.transform.SetParent(player.transform);
    }

    private void Dettach() 
    {
        attachedGameObject.transform.SetParent(scenario);
        attachedGameObject = null;
    }

    public void ClearAttachedObj() 
    {
        attachedGameObject = null;
    }

}
