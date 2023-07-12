using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Vector3 _direction;
    
    public void SetDirection(float directionX)
    {
        _direction = new Vector3(Math.Sign(directionX), 0, 0);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = new Vector3(Math.Sign(direction.x), Math.Sign(direction.y), 0);
    }

    private void Update()
    {
        transform.position += _direction * (speed * Time.deltaTime);
    }
}
