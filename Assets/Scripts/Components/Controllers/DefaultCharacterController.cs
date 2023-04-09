using UnityEngine;

namespace Components.Controllers
{
    /// <summary>
    /// Попытка в наиболее отзывчивый контроллер персонажа.
    /// <para>Чтобы можно было просто вызвать Ходи() или Прыгай() и все работало наиболее интуитивно</para> 
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class DefaultCharacterController : NavigateComponent
    {
        [Header("Ходьба")] 
    
        [Tooltip("Скорость ходьбы")]
        public float moveSpeed = 1;

        [Tooltip("Максимальная скорость разгона")]
        public float terminalMoveSpeed = 10;
    
        [Tooltip("Скорость ходьбы")]
        public float rotationSpeed = 1;

        [Space(10)]
        [Header("Прыжок и гравитация")]

        [Tooltip("Сила применяемая к прыжку")] 
        public float jumpForce = 1;

        [Tooltip("Время перед засчитыванием падения (да. тосмое время кайота)")]
        public float fallTimeout = 0.2f;
    
        [Tooltip("Высота прыжка")]
        public float jumpHeight = 1;
    
        [Tooltip("Сила прижимающая персонажа к земле")]
        public float gravity = 1;

        [Tooltip("Количество секунд за которые можно будет регулировать дополнительную высоту прыжка")]
        public float additionalForceTimeout = 0.2f;

        /// <summary>
        /// Говорит о том что персонажу нужно совершить прыжок
        /// </summary>
        private bool CallJumpTrigger { get; set; }
    
        /// <summary>
        /// Говорит о том что персонажу нужно применять дополнительную силу к прыжку 
        /// </summary>
        private bool CallContinueJumpTrigger { get; set; }

        /// <summary>
        /// Время каййота йо
        /// </summary>
        private float CurrentFallTimeout { get; set; }
    
        /// <summary>
        /// Сила гравитации применяемая к перонажу
        /// </summary>
        private float VerticalVelocity { get; set; }
    
        /// <summary>
        /// Направление движения персонажа
        /// </summary>
        private Vector3 MoveDirection { get; set; }
    
        private CharacterController _characterController;

        public void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }
        
        public void Update()
        {
            JumpAndGravityHandler();
            MoveHandler();

            PositionAndRotationHandler();
        }
    
        /// <summary>
        /// Устанавливает направление движения для персонажа
        /// <para>Берет на себя нормализацию и проекцию вводимых значений </para>
        /// </summary>
        /// <param name="pos"></param>
        public void Move(Vector3 pos)
        {
            var projected = Vector3.ProjectOnPlane(pos, Vector3.up);
            var normalized = projected.normalized;
            MoveDirection = normalized;
        }

        /// <summary>
        /// Начать прыжок
        /// </summary>
        public void StartJump()
        {
            if (_characterController.isGrounded)
            {
                CallJumpTrigger = true;
                CallContinueJumpTrigger = true;
            }
        }

        /// <summary>
        /// Прекратить давать дополнительную силу прыжку
        /// </summary>
        public void StopJump()
        {
            CallContinueJumpTrigger = false;
        }

        /// <summary>
        /// Применяем все вертикальные применения силы на пероснажа
        /// <para>Раобчая реализация для большинства игр</para>
        /// </summary>
        private void JumpAndGravityHandler()
        {
            // todo не рабоатет
            // if (_characterController.isGrounded)
            // {
            //     VerticalVelocity = gravity;
            //     CurrentFallTimeout = Time.time + fallTimeout;
            // }
            // else
            // {
            //     // Jump
            //     if ((_characterController.isGrounded || CurrentFallTimeout <= Time.time) && CallJumpTrigger)
            //     {
            //         // the square root of H * -2 * G = how much velocity needed to reach desired height
            //         VerticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //         CallJumpTrigger = false;
            //     }
            //
            //     // jump timeout
            //     if (CurrentFallTimeout >= 0.0f)
            //     {
            //         CurrentFallTimeout -= Time.deltaTime;
            //     }
            // }
            //
            // _characterController.Move(new Vector3(0, VerticalVelocity, 0));
            VerticalVelocity = -gravity;
        }

        /// <summary>
        /// Применяет все горизонтальные движения персонажа
        /// </summary>
        private void MoveHandler()
        {
            // todo разгон/замедление
        }

        /// <summary>
        /// Устанавливает позицию и вращает персонажа
        /// <para>Применяется самым последним, delateTime применяется именно тут</para>
        /// </summary>
        private void PositionAndRotationHandler()
        {
            var movement = new Vector3(MoveDirection.x, VerticalVelocity, MoveDirection.z) * (Time.deltaTime * moveSpeed);
            
            _characterController.Move(movement);
            transform.forward = Vector3.RotateTowards(transform.forward, MoveDirection, Time.deltaTime * rotationSpeed, 0).normalized;
        }
    }
}
