using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //player properties
    public float walkSpeed = 10f;

    public float gravity = 20f;

    public float jumpSpeed = 15f;

    public float doubleJumpSpeed = 10f;

    //player ability toggles
    public bool canDoubleJump;

    public bool canTripleJump;

    //player state
    public bool isJumping;

    public bool isDoubleJumping;

    public bool isTripleJumping;

    //input flags
    private bool _startJump;

    private bool _releaseJump;

    private Vector2 _input;

    private Vector2 _moveDirection;

    private CharacterController2D _characterController;

    //***********************************************************Animation
    public Animator anim;

    //***********************************************************Animation
    // Start is called before the first frame update
    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection.x = _input.x;
        _moveDirection.x *= walkSpeed;

        if (_moveDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (_moveDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        //Debug.Log(_moveDirection.y);
        if (
            _characterController.below //On the ground
        )
        {
            _moveDirection.y = 0f;

            isJumping = false;
            isDoubleJumping = false;
            isTripleJumping = false;
            anim.SetBool("IsJumping", false);

            if (_startJump)
            {
                SoundManager.Instance.PlayJumpSound();
                _startJump = false;
                _moveDirection.y = jumpSpeed;
                isJumping = true;
                _characterController.DisableGroundCheck();
            }
        } //In the air
        else
        {
            if (_releaseJump)
            {
                _releaseJump = false;

                if (_moveDirection.y > 0)
                {
                    _moveDirection.y *= 0.5f;
                }
            }

            //double Jumping
            if (_startJump)
            {
                if (
                    canTripleJump &&
                    (!_characterController.left && !_characterController.right)
                )
                {
                    if (isDoubleJumping && !isTripleJumping)
                    {
                        _moveDirection.y = doubleJumpSpeed;
                        isTripleJumping = true;
                    }
                }

                if (
                    canDoubleJump &&
                    (!_characterController.left && !_characterController.right)
                )
                {
                    if (!isDoubleJumping)
                    {
                        _moveDirection.y = doubleJumpSpeed;
                        isDoubleJumping = true;
                    }
                }

                _startJump = false;
            }

            GravityCalculations();
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    void GravityCalculations()
    {
        if (_moveDirection.y > 0f && _characterController.above)
        {
            _moveDirection.y = 0f;
        }

        _moveDirection.y -= gravity * Time.deltaTime;
    }

    //Input Methods
    public void OnMovement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        anim.SetFloat("speed", Mathf.Abs(_input.x));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _startJump = true;
            anim.SetBool("IsJumping", true);

            _releaseJump = false;
        }
        else if (context.canceled)
        {
            _releaseJump = true;
            anim.SetBool("IsJumping", false);
            _startJump = false;
        }
    }
}
