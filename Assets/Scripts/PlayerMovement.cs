using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed=5f;
    private bool canMove;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;
    void Start()
    {
        rb= gameObject.GetComponent<Rigidbody2D>();
        animator=gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate() {
        if(PauseController.IsGamePaused) {
            rb.velocity=Vector2.zero;
            animator.SetBool("isWalking", false);
            StopFootsteps();
            return;
        }
        rb.velocity= moveInput* moveSpeed;
        animator.SetBool("isWalking", rb.velocity.magnitude > 0);
        if(rb.velocity.magnitude >0 && !playingFootsteps) StartFootsteps();
        else if(rb.velocity.magnitude == 0f && playingFootsteps) StopFootsteps();
    }


    public void Move(InputAction.CallbackContext context){
        
        if(context.canceled) {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
            animator.SetBool("isWalking", false);
        }
        moveInput= context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    public void StartFootsteps() {
        playingFootsteps=true;
        InvokeRepeating(nameof(playFootSteps), 0f, footstepSpeed);
    }
    public void StopFootsteps() {
        playingFootsteps=false;
        CancelInvoke(nameof(playFootSteps));
    }
    void playFootSteps() {
        SoundEffectManager.Play("Footsteps");
    }
}
