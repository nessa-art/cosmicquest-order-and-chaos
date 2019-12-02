﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotorController : MonoBehaviour
{
    public float maxVelocity = 6.0f;
    public float maxAcceleration = 20.0f;
    public float rotationSpeed = 10.0f;

    private Rigidbody _rb;
    private Animator _anim;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private Vector3 _moveDirection;
    private Vector3 _lookDirection;
    
    private CameraController _cameraController;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _cameraController = Camera.main.GetComponent<CameraController>();
        // TODO Temporary - player should be registered after lobby
        PlayerManager.RegisterPlayer(gameObject);
    }

    private void OnEnable()
    {
        // Ensure player is not kinematic
        _rb.isKinematic = false;
    }

    private void OnDisable()
    {
        // Set kinematic when disabled so the player stops moving
        _rb.isKinematic = true;
    }

    private void OnDestroy()
    {
        // Ensure there's no invalid references hanging around
        PlayerManager.DeregisterPlayer(gameObject);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 inputMoveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 inputLookDirection = new Vector3(_lookInput.x, 0f, _lookInput.y);

        if (_lookInput != Vector2.zero)
        {
            // Rotate towards look direction
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(inputLookDirection, Vector3.up), rotationSpeed * Time.deltaTime));
        }
        else if (_moveInput != Vector2.zero)
        {
            // Rotate towards direction of movement
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(inputMoveDirection, Vector3.up), rotationSpeed * Time.deltaTime));
        }
        // for overriding legs when player is moving
        int walkLayerIndex = _anim.GetLayerIndex("WalkLayer");
        // for overriding legs when ranger is shooting
        int attackLayerIndex = _anim.GetLayerIndex("AttackLayer");
        if (attackLayerIndex >= 0)
        {
            // weight must be greater than walk layer weight
            _anim.SetLayerWeight(attackLayerIndex, 1);
        }
        // Animate player legs, legs will still move as they attack
        if (_moveInput != Vector2.zero)
        {
            // override default base layer walk animations
            _anim.SetLayerWeight(walkLayerIndex, .9f);
        }
        // Don't animate player legs
        else
        {
            _anim.SetLayerWeight(walkLayerIndex, 0);
        }

        float inputLookAngle = Vector3.Angle(inputMoveDirection, inputLookDirection);

        // Trigger walking animation
        _anim.SetFloat("WalkSpeed", _moveInput == Vector2.zero ? 0f : _moveInput.magnitude);
        // Set animation playback speed (if moving backwards animation will play in reverse)
        _anim.SetFloat("Direction", inputLookAngle < 90 ? 1f * _moveInput.magnitude : -1f *_moveInput.magnitude );
        // Set whether the player should strafe or not
        _anim.SetBool("Strafe", (inputLookAngle >= 45 && inputLookAngle <= 135));

        inputMoveDirection *= maxVelocity;
        AccelerateTo(inputMoveDirection);

        // Don't clamp if player is stationary
        if (inputMoveDirection != Vector3.zero)
        {
            Vector3 clamped = _cameraController.ClampToScreenEdge(_rb.position);
            if (clamped != _rb.position)
            {
                // TODO weird behaviour with gravity when position is clamped
                clamped.y = _rb.position.y;
                _rb.MovePosition(clamped);
            }
        }
    }

    private void AccelerateTo(Vector3 targetVelocity)
    {
        Vector3 dV = targetVelocity - _rb.velocity;
        Vector3 accel = dV / Time.fixedDeltaTime;

        if (accel.sqrMagnitude > maxAcceleration * maxAcceleration)
            accel = accel.normalized * maxAcceleration;
        
        _rb.AddForce(accel, ForceMode.Acceleration);
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void OnMove(Vector2 value)
    {
        if (!TestUtility.IsRunningInTest)
            throw new Exception("Do not use this method outside of PlayerInputMock!");
        _moveInput = value;
    }

    private void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }
    
    private void OnLook(Vector2 value)
    {
        if (!TestUtility.IsRunningInTest)
            throw new Exception("Do not use this method outside of PlayerInputMock!");
        _lookInput = value;
    }
}
