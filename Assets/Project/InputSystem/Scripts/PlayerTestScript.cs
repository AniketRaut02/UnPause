using DarkPixelGD.Input;
using UnityEngine;

public class PlayerTestScript : MonoBehaviour
{

    [Header("Input Reader Asset")]
    public InputReader _input;


    private Vector2 moveVal;
    private Vector2 lookVal;

    private void OnEnable()
    {
        _input.InteractEvent += HandleInteract;
        _input.AttackEvent += HandleAttack;
        _input.CrouchEvent += HandleCrouch;
        _input.NextEvent += HandleNext;
        _input.PreviousEvent += HandlePrevious;
        _input.SprintEvent += HandleSprint;
        _input.DashEvent += HandleJump;

    }

    private void OnDisable()
    {
        _input.InteractEvent -= HandleInteract;
        _input.AttackEvent -= HandleAttack;
        _input.CrouchEvent += HandleCrouch;
        _input.NextEvent -= HandleNext;
        _input.PreviousEvent -= HandlePrevious;
        _input.SprintEvent -= HandleSprint;
        _input.DashEvent -= HandleJump;
    }

    private void Awake()
    {
        _input.Initialize();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleLook();
    }

    private void HandleMove()
    {
        Vector2 lastVal = moveVal;
        moveVal = _input.MoveInput;
        if(lastVal != moveVal){ Debug.Log($"X:{moveVal.x} Y:{moveVal.y}"); }

    }
    private void HandleLook()
    {
        Vector2 lastVal = lookVal;
        lookVal = _input.LookInput;
        if (lastVal != lookVal) { Debug.Log($"X:{lookVal.x} Y:{lookVal.y}"); }

    }

    private void HandleInteract()
    {
        Debug.Log("Interacted");
    }
    private void HandleAttack()
    {
        Debug.Log("Attacked");
    }
    private void HandleSprint(bool isSprinting)
    {
        if (isSprinting) Debug.Log("Enter Sprint State");
        else if (!isSprinting) Debug.Log("Exit sprint State");
    }

    private void HandleCrouch(bool isCrouching)
    {
        if(isCrouching) Debug.Log("Enter Crouched State");
        else if(!isCrouching) Debug.Log("Exit Crouched State");
    }
    private void HandleJump()
    {
        Debug.Log("Jumped");
    }
    private void HandleNext()
    {
        Debug.Log("Next Pressed");
    }

    private void HandlePrevious()
    {
        Debug.Log("Previous Pressed");
    }

}
