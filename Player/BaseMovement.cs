using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BaseMovement : MonoBehaviour
{
    public CharacterController2D characterController;

    public float movementSpeed;
    public float rotateSpeed;
    float horizontalInput;


    public Transform[] groundCheckPoses;
    public Transform groundChecker;
    public Transform ceilingChecker;

    [HideInInspector] public float groundedTime;
    [HideInInspector] public float ungroundedTime;
    float spinGroundedTime;
    float spinForce;

    public float definateGroundedCheckDistance;
    public bool definateGrounded;
    float timeAfterJump;

    public float fixAngleSpeed;

    float sideWallSpeedMultiplier;

    public GameObject moveLeftUI;
    public GameObject moveRightUI;
    private void Start()
    {
        Cache.player = gameObject;
        Cache.groundCheck = groundChecker;

        InputManager.mobileMoveSystem = PlayerPrefs.GetInt("InputType");
    }
    private void OnEnable()
    {
        InputManager.CancleHorizontalAxisRawInput();
        if (InputManager.mobileMoveSystem == 0||true) { moveLeftUI.SetActive(false); moveRightUI.SetActive(false); }
    }
    private void Update()
    {
        definateGrounded = Physics2D.Raycast(groundChecker.position,-Vector3.up, definateGroundedCheckDistance, characterController.m_WhatIsGround);

        if (horizontalInput > 0 && Physics2D.Raycast(transform.position, Vector2.right, 0.8f, characterController.m_WhatIsGround))
        {
            sideWallSpeedMultiplier -= Time.deltaTime * 10;
            sideWallSpeedMultiplier = Mathf.Clamp(sideWallSpeedMultiplier, 0, 1);
        }
        else if (horizontalInput < 0 && Physics2D.Raycast(transform.position, -Vector2.right, 0.8f, characterController.m_WhatIsGround))
        {
            sideWallSpeedMultiplier -= Time.deltaTime * 10;
            sideWallSpeedMultiplier = Mathf.Clamp(sideWallSpeedMultiplier, 0, 1);
        }
        else
        {
            sideWallSpeedMultiplier = 1;
        }

        horizontalInput = InputManager.GetHorizontalAxisRaw();

        if (Application.isMobilePlatform && InputManager.mobileMoveSystem == 1)
        {
            if (horizontalInput > 0) { moveRightUI.SetActive(true); moveLeftUI.SetActive(false); }
            else if (horizontalInput < 0) { moveRightUI.SetActive(false); moveLeftUI.SetActive(true); }
            else { moveRightUI.SetActive(false); moveLeftUI.SetActive(false); }
        }

        //if (Input.GetButtonDown("Jump") && definateGrounded && timeAfterJump > 0.25f) Jump();
        timeAfterJump += Time.deltaTime;

        if (characterController.grounded) { groundedTime += Time.deltaTime; ungroundedTime = 0; }
        else { groundedTime = 0; ungroundedTime += Time.deltaTime; }

        if (horizontalInput != 0 && characterController.grounded) spinGroundedTime = 0;
        else spinGroundedTime += Time.deltaTime;

        float lowestCheckerY = Mathf.Infinity;
        int lowestCheckerIndex = 0;
        for (int i = 0; i < groundCheckPoses.Length; i++)
        {
            if (groundCheckPoses[i].position.y < lowestCheckerY){
                lowestCheckerY = groundCheckPoses[i].position.y;
                lowestCheckerIndex = i;
            }
        }
        groundChecker.position = groundCheckPoses[lowestCheckerIndex].position;
        ceilingChecker.position = groundCheckPoses[Utilities.GetArrayWrappedIndex(groundCheckPoses.Length, lowestCheckerIndex, 2)].position;
    }
    private void Jump()
    {
        characterController.Jump();
        timeAfterJump = 0;
    }
    private void FixedUpdate()
    {
        characterController.Move(horizontalInput * movementSpeed * sideWallSpeedMultiplier * Time.deltaTime, false);
        if (spinGroundedTime < 0.2f) spinForce = 1;
        else spinForce -= Time.deltaTime;
        spinForce = Mathf.Clamp(spinForce, 0.5f, 1);
        transform.Rotate(new Vector3(0, 0, horizontalInput * movementSpeed * rotateSpeed * spinForce * -Time.deltaTime));
        if (definateGrounded && (transform.eulerAngles.z > 15 && transform.eulerAngles.z < 45) || (transform.eulerAngles.z > 105 && transform.eulerAngles.z < 135) || (transform.eulerAngles.z > 195 && transform.eulerAngles.z < 225) || (transform.eulerAngles.z > 285 && transform.eulerAngles.z < 315)){
            transform.Rotate(new Vector3(0, 0, -fixAngleSpeed * Time.deltaTime));
        }
        if (definateGrounded && horizontalInput == 0 && (transform.eulerAngles.z > 45 && transform.eulerAngles.z < 75) || (transform.eulerAngles.z > 135 && transform.eulerAngles.z < 165) || (transform.eulerAngles.z > 225 && transform.eulerAngles.z < 255) || (transform.eulerAngles.z > 315 && transform.eulerAngles.z < 345)){
            transform.Rotate(new Vector3(0, 0, fixAngleSpeed * Time.deltaTime));
        }
    }
}
