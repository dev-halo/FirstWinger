using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 MoveVector = Vector3.zero;

    [SerializeField]
    private float Speed;

    private void Update()
    {
        UpdateMove();
    }

    public void ProcessInput(Vector3 moveDirection)
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;
    }

    private void UpdateMove()
    {
        transform.position += MoveVector;
    }
}
