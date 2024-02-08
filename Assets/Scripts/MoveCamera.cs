using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField, Range (0,1)] private float _speed = 0.5f;
    [SerializeField, Range (0,1)] private float _rotationSensetive = 0.25f;

    private Vector3 lastMousePosition;
    private Vector3 rotationDirection = Vector3.zero;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1)) 
            Rotate();
        else
            lastMousePosition = Input.mousePosition;

        transform.Translate(GetDirectionByInputKey());
    }

    private void Rotate()
    {
        Vector3 rotationValues = Input.mousePosition - lastMousePosition;

        rotationDirection = new Vector3(-rotationValues.y, rotationValues.x, 0);
        transform.eulerAngles += rotationDirection * _rotationSensetive;

        lastMousePosition = Input.mousePosition;
    }

    private Vector3 GetDirectionByInputKey()
    {
         Vector3 direction = new Vector3(0, 0, 0);
        
        if (Input.GetKey(KeyCode.W))
            direction.z += _speed;
        else if (Input.GetKey(KeyCode.S))
            direction.z -= _speed;

        if (Input.GetKey(KeyCode.A))
            direction.x -= _speed;
        else if (Input.GetKey(KeyCode.D))
            direction.x += _speed;

        if (Input.GetKey(KeyCode.Space))
            direction.y += _speed;
        else if (Input.GetKey(KeyCode.LeftAlt))
            direction.y -= _speed;

        return direction;
    }
}
