using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    public void UpdateInput()
    {
        if (SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().CurrentGameState != GameState.Running)
        {
            return;
        }

        UpdateKeyboard();
        UpdateMouse();
    }

    private void UpdateKeyboard()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y = 1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x = 1f;
        }

        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().Hero.ProcessInput(moveDirection);
    }

    private void UpdateMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().Hero.Fire();
        }
    }
}
