using System.Collections;
using System.Collections.Generic;
using GlobalTypes;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float raycastDistance = 0.2f;

    public LayerMask layerMask;

    public float slopeAngleLimit = 45f;

    public float downForceAdjustment = 1.2f;

    //flags
    public bool below;

    public bool left;

    public bool right;

    public bool above;

    public GroundType groundType;

    public bool hitGroundThisFrame;

    private Vector2 _moveAmount;

    private Vector2 _currentPostion;

    private Vector2 _lastPosition;

    private Rigidbody2D _rigidbody;

    private CapsuleCollider2D _capsuleCollider;

    private Vector2[] _raycastPosition = new Vector2[3];

    private RaycastHit2D[] _raycastHits = new RaycastHit2D[3];

    private bool _disableGroundCheck;

    //TODO: Change to private
    private Vector2 _slopeNormal;

    private float _slopeAngle;

    private bool _inAirLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _inAirLastFrame = !below;

        _lastPosition = _rigidbody.position;

        if (_slopeAngle != 0 && below == true)
        {
            if (
                (_moveAmount.x > 0f && _slopeAngle > 0f) ||
                (_moveAmount.x < 0f && _slopeAngle < 0f)
            )
            {
                _moveAmount.y =
                    -Mathf
                        .Abs(Mathf.Tan(_slopeAngle * Mathf.Deg2Rad) *
                        _moveAmount.x);
                _moveAmount.y *= downForceAdjustment;
            }
        }

        _currentPostion = _lastPosition + _moveAmount;

        _rigidbody.MovePosition (_currentPostion);

        _moveAmount = Vector2.zero;

        if (!_disableGroundCheck)
        {
            CheckGrounded();
        }

        CheckOtherCollisions();

        if (below && _inAirLastFrame)
        {
            hitGroundThisFrame = true;
        }
        else
        {
            hitGroundThisFrame = false;
        }
    }

    public void Move(Vector2 movement)
    {
        _moveAmount += movement;
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit =
            Physics2D
                .CapsuleCast(_capsuleCollider.bounds.center,
                _capsuleCollider.size,
                CapsuleDirection2D.Vertical,
                0f,
                Vector2.down,
                raycastDistance,
                layerMask);

        if (hit.collider)
        {
            groundType = DetermineGroundType(hit.collider);

            _slopeNormal = hit.normal;
            _slopeAngle = Vector2.SignedAngle(_slopeNormal, Vector2.up);

            if (_slopeAngle > slopeAngleLimit || _slopeAngle < -slopeAngleLimit)
            {
                below = false;
            }
            else
            {
                below = true;
            }
        }
        else
        {
            groundType = GroundType.None;
            below = false;
        }
    }

    private void CheckOtherCollisions()
    {
        //check left
        RaycastHit2D leftHit =
            Physics2D
                .BoxCast(_capsuleCollider.bounds.center,
                _capsuleCollider.size * 0.75f,
                0f,
                Vector2.left,
                raycastDistance * 2,
                layerMask);

        if (leftHit.collider)
        {
            left = true;
        }
        else
        {
            left = false;
        }

        //check right
        RaycastHit2D rightHit =
            Physics2D
                .BoxCast(_capsuleCollider.bounds.center,
                _capsuleCollider.size * 0.75f,
                0f,
                Vector2.right,
                raycastDistance * 2,
                layerMask);

        if (rightHit.collider)
        {
            right = true;
        }
        else
        {
            right = false;
        }

        //check above
        RaycastHit2D aboveHit =
            Physics2D
                .CapsuleCast(_capsuleCollider.bounds.center,
                _capsuleCollider.size,
                CapsuleDirection2D.Vertical,
                0f,
                Vector2.up,
                raycastDistance,
                layerMask);

        if (aboveHit.collider)
        {
            above = true;
        }
        else
        {
            above = false;
        }
    }

    private void DrawDebugRays(Vector2 direction, Color color)
    {
        for (int i = 0; i < _raycastPosition.Length; i++)
        {
            Debug
                .DrawRay(_raycastPosition[i],
                direction * raycastDistance,
                color);
        }
    }

    public void DisableGroundCheck()
    {
        below = false;
        _disableGroundCheck = true;
        StartCoroutine("EnableGroundCheck");
    }

    IEnumerator EnableGroundCheck()
    {
        yield return new WaitForSeconds(0.1f);
        _disableGroundCheck = false;
    }

    private GroundType DetermineGroundType(Collider2D collider)
    {
        if (collider.GetComponent<GroundEffector>())
        {
            GroundEffector groundEffector =
                collider.GetComponent<GroundEffector>();
            return groundEffector.groundType;
        }
        else
        {
            return GroundType.Ground;
        }
    }
}
