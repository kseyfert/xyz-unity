﻿using System;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] private float speed = 1;

        [Header("Jump")] 
        [SerializeField] private float jumpForce = 1;
        [SerializeField] private float jumpCoolDown = 0.5f;

        [Header("Debug")] 
        [SerializeField] private bool debugEnabled = true;
        [SerializeField] private float debugSphereRadius;
        [SerializeField] private Vector3 debugSpherePosition;

        private static readonly int _keyVelocityX = Animator.StringToHash("velocity-x");
        private static readonly int _keyVelocityY = Animator.StringToHash("velocity-y");
        private static readonly int _keyIsGrounded = Animator.StringToHash("is-grounded");
        
        private Vector3 _direction;
        private Rigidbody2D _rb;
        private LayerChecker _groundChecker;
        private Animator _animator;
        private bool _doJump;
        private bool _didJump;

        private int _coins = 0;

        public void SetDirection(float directionX)
        {
            _direction = new Vector3(Math.Sign(directionX), 0, 0);
        }

        public void SetDirection(Vector2 direction)
        {
            SetDirection(direction.x);
        }

        public void SetJump(bool value)
        {
            _doJump = value;
            if (value) _didJump = false;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _groundChecker = GetComponentInChildren<LayerChecker>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            var velocity = _direction * speed;
            _rb.velocity = new Vector2(velocity.x, _rb.velocity.y);
            if (velocity.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
            if (velocity.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);

            var isGrounded = IsGrounded();
            if (_doJump && !_didJump && isGrounded)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                _didJump = true;
            } else if (!_doJump && _didJump && _rb.velocity.y > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * jumpCoolDown);
            }
            
            _animator.SetBool(_keyIsGrounded, isGrounded);
            _animator.SetFloat(_keyVelocityX, _rb.velocity.x);
            _animator.SetFloat(_keyVelocityY, _rb.velocity.y);
        }

        private bool IsGrounded()
        {
            if (_groundChecker == null) return true;
            return _groundChecker.IsTouchingLayer();
        }

        private void OnDrawGizmos()
        {
            if (debugEnabled)
            {
                Gizmos.color = IsGrounded() ? Color.green : Color.red;
                Gizmos.DrawSphere(transform.position + debugSpherePosition, debugSphereRadius);
            }
        }

        public void AddCoin(int value=1)
        {
            _coins += value;
            Debug.Log($"Money: {_coins}");
        }
    }
}