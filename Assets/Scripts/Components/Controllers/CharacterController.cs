using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterController : NavigateComponent
{
    [Header("Основные значения персонажа")] 
    
    [Tooltip("Скорость ходьбы")]
    public float moveSpeed;

    [Tooltip("Сила применяемая к прыжку")] 
    public float jumpForce;

    [Tooltip("Время перед засчитыванием падения (да. тосмое время кайота)")]
    public float fallTimeout;

    private readonly CharacterController _characterController;

    public void Move(Vector2 pos)
    {
        
    }

    public void Jump()
    {
        
    }

}
