using System;
using UnityEngine;

namespace Level.Floor
{
    public class Floor : MonoBehaviour
    {
        // Событие, возникающее, когда игрок проходит текущий пол.
        public event Action<Floor> PlayerPassedCurrentFloor;

        private BoxCollider2D _boxCollider;

        private void Awake()
        {
            // Получаем ссылку на компонент BoxCollider2D
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        /// <summary>
        /// Метод получает размер пола.
        /// </summary>
        public Vector2 GetSize()
        {
            // Возвращаем размер коллайдера игрового объекта
            return _boxCollider.size;
        }

        /// <summary>
        /// Метод, вызываемый при столкновении объекта с триггерами.
        /// Проверяет, была ли коллизия с игроком, и при необходимости вызывает событие PlayerPassedCurrentFloor.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PlayerPassedCurrentFloor?.Invoke(this);
            }
        }
        
    }
}