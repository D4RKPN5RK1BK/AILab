using Components.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Контроллер для инпутов
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    private PlayerController _playerController;

    private Vector2 _move;
    private Vector2 _rotate;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void Update()
    {
        _playerController.Move(_move);
        _playerController.Rotate(_rotate);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        _rotate = context.ReadValue<Vector2>();
    }

    public void OnActive(InputAction.CallbackContext context)
    {
        _playerController.ResetScene();
    }
}
