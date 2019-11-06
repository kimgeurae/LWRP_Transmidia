using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    [SerializeField]
    GameObject selectedCharacterFeedbackPrefab;
    Transform selectedCharacter = null;
    Transform selectedCharacterFeedback = null;

    [SerializeField]
    Transform SelectionIndicatorFeedback;

    [SerializeField]
    Grid grid;
    [SerializeField]
    Transform player;

    Vector3 nullPosition = Vector3.zero;

    [SerializeField]
    LayerMask mask;

    [SerializeField]
    LayerMask playerMask;

    [SerializeField]
    GameObject frontWall, leftWall, backWall, rightWall, leftFrontCorner, leftBackCorner, rightFrontCorner, rightBackCorner;

    Vector3 heightOffset = new Vector3(0.5f, 0.001f, 0.5f);

    Vector2 playerGridPosition;

    enum CameraRotation {right_front, right_back, left_front, left_back};
    CameraRotation currentCameraRotation;

    enum DesiredRotation {left, right};
    bool hasRotated = true;

    void Start()
    {
        playerGridPosition.x = player.position.x;
        playerGridPosition.y = player.position.z;
        currentCameraRotation = CameraRotation.right_front;
        DisplayWalls();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) TryToSelect();
        UpdateSelectionIndicatorFeedback();
        KeyboardMovementInput();
        KeyboardCameraInput();
    }

    private void KeyboardMovementInput() 
    {
        if (Input.GetKeyDown(KeyCode.Keypad9)) 
        {
            Vector2 desiredPos = playerGridPosition + Vector2.left;
            if(grid.ValidateMovement(desiredPos)) MovePlayer(desiredPos);
            else return;
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.Keypad1)) 
            {
                Vector2 desiredPos = playerGridPosition + Vector2.right;
                if(grid.ValidateMovement(desiredPos)) MovePlayer(desiredPos);
                else return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad7)) 
        {
            Vector2 desiredPos = playerGridPosition + Vector2.down;
            if(grid.ValidateMovement(desiredPos)) MovePlayer(desiredPos);
            else return;
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.Keypad3)) 
            {
                Vector2 desiredPos = playerGridPosition + Vector2.up;
                if(grid.ValidateMovement(desiredPos)) MovePlayer(desiredPos);
                else return;
            }
        }
    }

    private void MovePlayer(Vector2 desiredPos)
    {
        Vector3 newPosition = new Vector3(desiredPos.x, player.position.y, desiredPos.y);
        player.position = newPosition;
        playerGridPosition = desiredPos;
    }

    private void UpdateSelectionIndicatorFeedback() 
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, mask)) 
        {
            if (hit.transform.CompareTag("Ground")) 
            {
                int x = Mathf.FloorToInt(hit.point.x);
                int y = Mathf.FloorToInt(hit.point.y);
                int z = Mathf.FloorToInt(hit.point.z);
                Vector3 intHitPosition = new Vector3(x, y, z);
                SelectionIndicatorFeedback.position = intHitPosition + heightOffset;
            }
            else 
            {
                SelectionIndicatorFeedback.position = nullPosition;
            }
        }
    }

    private void TryToSelect()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 250f, playerMask)) 
        {
            if (hit.transform.CompareTag("Player")) 
            {
                selectedCharacter = hit.transform;
                if(selectedCharacterFeedback == null) selectedCharacterFeedback = Instantiate(selectedCharacterFeedbackPrefab, selectedCharacter).transform;
            }
            else 
            {
                if(selectedCharacter != null) selectedCharacter = null;
                if(selectedCharacterFeedback != null) Destroy(selectedCharacterFeedback.gameObject);
            }
        }
    }

    private void KeyboardCameraInput() 
    {
        if(Input.GetKeyDown(KeyCode.Q)) 
        {
            RotateCamera(DesiredRotation.left);
        }
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            RotateCamera(DesiredRotation.right);
        }
    }

    private void RotateCamera(DesiredRotation desiredRotation) 
    {
        switch(desiredRotation) 
        {
            case DesiredRotation.left:
                Camera.main.transform.parent.eulerAngles = new Vector3(Camera.main.transform.parent.eulerAngles.x, Mathf.RoundToInt(Camera.main.transform.parent.eulerAngles.y-90), Camera.main.transform.parent.eulerAngles.z);
                SetCurrentCameraRotationEnum(desiredRotation);
                DisplayWalls();
                break;
            case DesiredRotation.right:
                Camera.main.transform.parent.eulerAngles = new Vector3(Camera.main.transform.parent.eulerAngles.x, Camera.main.transform.parent.eulerAngles.y+90, Camera.main.transform.parent.eulerAngles.z);
                SetCurrentCameraRotationEnum(desiredRotation);
                DisplayWalls();
                break;
        }
    }

    private void SetCurrentCameraRotationEnum(DesiredRotation d) 
    {
        Debug.Log($"Previous Camera Rotation: {currentCameraRotation}, Desired Rotation: {d}");
        if(currentCameraRotation == CameraRotation.left_back) {
            if(d == DesiredRotation.right) currentCameraRotation = CameraRotation.left_front;
            else currentCameraRotation = CameraRotation.right_back;
        }
        if(currentCameraRotation == CameraRotation.left_front) {
            if(d == DesiredRotation.right) currentCameraRotation = CameraRotation.right_front;
            else currentCameraRotation = CameraRotation.left_back;
        }
        if(currentCameraRotation == CameraRotation.right_front) {
            if(d == DesiredRotation.right) currentCameraRotation = CameraRotation.right_back;
            else currentCameraRotation = CameraRotation.left_front;
        }
        if(currentCameraRotation == CameraRotation.right_back) {
            if(d == DesiredRotation.right) currentCameraRotation = CameraRotation.left_back;
            else currentCameraRotation = CameraRotation.right_front;
        }
        Debug.Log($"Next Camera Rotation: {currentCameraRotation}, Desired Rotation: {d}");
    }

    private void DisplayWalls() 
    {
        switch(currentCameraRotation) 
        {
            case CameraRotation.right_front:
                frontWall.SetActive(false);
                rightWall.SetActive(false);
                backWall.SetActive(true);
                leftWall.SetActive(true);
                rightFrontCorner.SetActive(false);
                rightBackCorner.SetActive(false);
                leftFrontCorner.SetActive(false);
                leftBackCorner.SetActive(true);
                break;
            case CameraRotation.right_back:
                frontWall.SetActive(true);
                rightWall.SetActive(false);
                backWall.SetActive(false);
                leftWall.SetActive(true);
                rightFrontCorner.SetActive(false);
                rightBackCorner.SetActive(false);
                leftFrontCorner.SetActive(true);
                leftBackCorner.SetActive(false);
                break;
            case CameraRotation.left_back:
                frontWall.SetActive(true);
                rightWall.SetActive(true);
                backWall.SetActive(false);
                leftWall.SetActive(false);
                rightFrontCorner.SetActive(true);
                rightBackCorner.SetActive(false);
                leftFrontCorner.SetActive(false);
                leftBackCorner.SetActive(false);
                break;
            case CameraRotation.left_front:
                frontWall.SetActive(false);
                rightWall.SetActive(true);
                backWall.SetActive(true);
                leftWall.SetActive(false);
                rightFrontCorner.SetActive(false);
                rightBackCorner.SetActive(true);
                leftFrontCorner.SetActive(false);
                leftBackCorner.SetActive(false);
                break;
        }
    }
}
