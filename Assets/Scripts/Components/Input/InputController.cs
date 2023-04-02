using Components.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

// Прчина по которой раньше ничегоь не получалось: В PlayerInput компоненте нужно во вкладке Events -> <набор действий> проставлять вручную все функции
// Еще нужно обязательно проставить Behaviour на Invoke Unity Events
// Ок, ради интереса можно и на 

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
        Debug.Log("OnMove" + context.ReadValue<Vector2>());
        _move = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        Debug.Log("OnRotate: " + context.ReadValue<Vector2>());
        _rotate = context.ReadValue<Vector2>();
    }
}
