using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Vector3 MoveVector = Vector3.zero;

    private void Update()
    {
        UpdateMove();
    }

    public void ProcessInput(Vector3 moveDirection)
    {

    }

    private void UpdateMove()
    {

    }
}
