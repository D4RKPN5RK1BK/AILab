using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components.Controllers
{
    /// <summary>
    /// На самом деле контроллер персонажа которым управляет игрок
    /// </summary>
    public class PlayerController : NavigateComponent
    {
        [Header("Основные настройки персонажа")]
        
        [Tooltip("Скорость движения")]
        public float moveSpeed = 20;
        
        [Tooltip("Скорость вертикального вращения")]
        public float verticalRotation = 100;

        [Tooltip("Скорость горизонтального вращения")]
        public float horisontalRotation = 100;
        
        [Tooltip("Камера привязанная к персонажу (наклоняется вверх вниз)")]
        public GameObject cameraTarget;

        private float _tilt = 0;

        /// <summary>
        /// Двигает персонажа
        /// </summary>
        /// <param name="move"></param>
        public void Move(Vector2 move)
        {
            // Character controller`а нет, так что просто сдвигаем позицию 
            transform.position += (move.y * cameraTarget.transform.forward + move.x * transform.right) * (Time.deltaTime * moveSpeed);
        }

        /// <summary>
        /// Вращение игрока и камеры привязанной к нему
        /// </summary>
        /// <param name="rotate"></param>
        public void Rotate(Vector2 rotation)
        {
            _tilt = Math.Clamp(_tilt -rotation.y * verticalRotation, -60, 60) ; 
            var rotationX = rotation.x * horisontalRotation;

            cameraTarget.transform.localRotation = Quaternion.Euler(_tilt, 0.0f, 0.0f);
            transform.Rotate(Vector3.up, rotationX); 
        }

        /// <summary>
        /// Перезагружает сцену
        /// </summary>
        public void ResetScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}