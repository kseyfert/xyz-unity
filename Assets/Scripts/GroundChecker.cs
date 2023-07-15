using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Collider2D _collider;
    private bool _isTouchingGround;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _isTouchingGround = _collider.IsTouchingLayers(groundLayer);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _isTouchingGround = _collider.IsTouchingLayers(groundLayer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _isTouchingGround = _collider.IsTouchingLayers(groundLayer);
    }

    public bool IsTouchingGround()
    {
        return _isTouchingGround;
    }
}
