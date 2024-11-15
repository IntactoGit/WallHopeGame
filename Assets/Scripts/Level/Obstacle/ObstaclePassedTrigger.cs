using System;
using UnityEngine;

namespace Level.Obstacle
{
    /// <summary>
    /// Данный класс отвечает за триггер, который сообщает, что игрок прошел препятсвие
    /// </summary>
    public class ObstaclePassedTrigger : MonoBehaviour
    {
        // Событие используется для оповещения о том, что игрок прошел препятсвие
        public event Action PlayerPassedObstacle;

        /// <summary>
        /// Метод проверяет коллизии объекта с триггерами
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Проверяем была ли коллизия с тегом
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PlayerPassedObstacle?.Invoke();
            }
        }
    }
}