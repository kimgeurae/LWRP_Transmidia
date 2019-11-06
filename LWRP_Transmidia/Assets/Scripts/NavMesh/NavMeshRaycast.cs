using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class NavMeshRaycast : MonoBehaviour
{

    #region Serialized Variables
    #endregion

    #region Private Variables
    NavMeshAgent selectedAgent;
    ThirdPersonCharacter[] character;
    NavMeshAgent[] agent;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        FindAndReferToAllAgents();
        DisableAgentRotation();
        SetCharacterReferences();
    }

    private void FindAndReferToAllAgents() 
    {
        GameObject[] go;
        go = GameObject.FindGameObjectsWithTag("Player");
        agent = new NavMeshAgent[go.Length];
        for(int i =0; i < go.Length; i++) 
        {
            agent[i] = go[i].GetComponent<NavMeshAgent>();
        }
    }

    private void DisableAgentRotation() 
    {
        for(int i = 0; i < agent.Length; i++) 
        {
            agent[i].updateRotation = false;
        }
    }

    private void SetCharacterReferences() 
    {
        character = new ThirdPersonCharacter[agent.Length];
        for(int i = 0; i < agent.Length; i++)
        {
            character[i] = agent[i].GetComponent<ThirdPersonCharacter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDestSelectedCharacter();
        AttemptToSelectCharacter();
        MoveCharacters();

    }

    private void AttemptToSelectCharacter() 
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit physicsHit;
            float raycastLength = 100f;
            if(Physics.Raycast(ray, out physicsHit, raycastLength)) {
                if(physicsHit.transform.CompareTag("Player")) 
                {
                    selectedAgent = physicsHit.transform.GetComponent<NavMeshAgent>();
                }
                else 
                {
                    selectedAgent = null;
                }
                ActiveSelectedCharacterIndicator();
            }
        }
    }

    private void ChangeDestSelectedCharacter() 
    {
        if(Input.GetMouseButtonDown(1)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit physicsHit;
            float raycastLength = 100f;
            if(Physics.Raycast(ray, out physicsHit, raycastLength)) {
                NavMeshHit navMeshHit;
                int walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
                if(NavMesh.SamplePosition(physicsHit.point, out navMeshHit, 1.0f, walkableMask) && selectedAgent != null) {
                    selectedAgent.SetDestination(navMeshHit.position);
                }
            }
        }
    }

    private void MoveCharacters() 
    {
        for(int i = 0; i < agent.Length; i++) 
        {
            if(agent[i].remainingDistance > agent[i].stoppingDistance) 
            {
                character[i].Move(agent[i].desiredVelocity, false, false);
            }
            else 
            {
                character[i].Move(Vector3.zero, false, false);
            }
        }
    }

    private void ActiveSelectedCharacterIndicator() 
    {
        for(int i = 0; i < agent.Length; i++) 
        {
            agent[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        if(selectedAgent != null) {
            if(!selectedAgent.transform.GetChild(0).gameObject.activeSelf) selectedAgent.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
