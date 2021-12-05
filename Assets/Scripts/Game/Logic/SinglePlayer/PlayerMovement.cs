using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Handles the logic of player movement
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f; // How fast the player will move
    [SerializeField] private float distToGround = 1f; // The distance that it will count as isGrounded
    [SerializeField] private float skipForce = 1f; // How high will be the skipping(when walking horizontaly)   
    [SerializeField] private float rotationSpeed = 2f; // The speed of the character's rotation 
    [SerializeField] private LayerMask groundLayer;

    protected Transform character; // The charatcer's transform that we want to rotate or animate
    private Rigidbody rb; // Rigidbody to move the player
    private float zRotation; // The z rotation of the character

    // Checks if the character is grounded
    private bool isGrounded => Physics.Raycast(transform.position, Vector3.down, distToGround, groundLayer);

    protected void Start()
    {
        character = transform.GetChild(GameObjectHelper.characterPosition);
        rb = GetComponent<Rigidbody>();
        zRotation = transform.localRotation.z;
    }

    public virtual void movePlayer(float xDirection, float zDirection)
    {
        skip(zDirection);       
        Vector3 movementVector = new Vector3(xDirection, 0f, zDirection).normalized * movementSpeed * Time.deltaTime;
        movementVector = rb.position + rb.transform.TransformDirection(movementVector);
        rb.MovePosition(movementVector);
    }

    public virtual void rotatePlayer(float xDirection)
    {
        zRotation -= xDirection * rotationSpeed * Time.deltaTime;
        character.localEulerAngles = new Vector3(0, 0, zRotation);
    }

    private void skip(float zDirection)
    {
        if (isGrounded && (Mathf.Abs(zDirection) > 0.1))
        {
            rb.AddForce(0, skipForce, 0, ForceMode.Impulse);
        }
    }
    
}
