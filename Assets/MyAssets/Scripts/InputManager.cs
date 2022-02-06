using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private PlayerMovement m_PlayerMovement;

    private Vector2 mMovement = Vector2.zero;
    private PlayerInput mPlayerInput;
    private PlayerInput.PlayerActions mPlayerActions;

    private void Awake()
    {
        mPlayerInput = new PlayerInput();
        mPlayerActions = mPlayerInput.Player;

        mPlayerActions.Move.performed += ctx => HandleMove(ctx);
    }

    void HandleMove(InputAction.CallbackContext context)
    {
        mMovement = context.ReadValue<Vector2>();
        m_PlayerMovement.Movement = mMovement;
    }

    private void OnEnable()
    {
        mPlayerInput.Player.Enable();
    }

    private void OnDisable()
    {
        mPlayerInput.Player.Disable();
    }
}
