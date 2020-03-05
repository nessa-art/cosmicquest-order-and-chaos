﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
    private int assignedPlayer;
    private void Start()
    {
        assignedPlayer = PlayerManager.Instance.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer >= 0)
        {
            name = "Player " + (assignedPlayer + 1) + " UI Control";
            if (GameManager.Instance.CurrentState == GameManager.GameState.Menu)
            {
                MainMenuController.Instance.AssignMultiplayerUIControl(gameObject, assignedPlayer);
            }
            else if (GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
            {
                PreviewMenuController.Instance.AssignMultiplayerUIControl(gameObject);
            }
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }

    public void OnMenuCancel(InputValue value)
    {
        // only player 1 may activate this
        if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.Menu)
        {
            if (!value.isPressed)
            {
                return;
            }
            MainMenuController.Instance.PopMenu();
        }
    }

    public void OnMenuSelect(InputValue value)
    {
        if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
        {
            if (!value.isPressed)
            {
                return;
            }
            PreviewMenuController.Instance.PreviewLevel();
        }
    }

    public void OnMenuNavigate(InputValue value)
    {
        // only player 1 may activate this
        if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
        {
            Vector2 input = value.Get<Vector2>();
            int horizontalInput = input.x > 0 ? 1 : -1;
            int verticalInput = input.y > 0 ? 1 : -1;
            if ((Mathf.Approximately(input.x, 0) && Mathf.Approximately(input.y, 0)))
            {
                return;
            }
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (horizontalInput < 0)
                {
                    // left
                    LevelsController.Instance.NavigateLeft();
                }
                else
                {
                    // right
                    LevelsController.Instance.NavigateRight();
                }
            }
            else
            {
                if (verticalInput < 0)
                {
                    // down
                    LevelsController.Instance.NavigateDown();
                }
                else
                {
                    // up
                    LevelsController.Instance.NavigateUp();
                }
            }
        }
    }
}
