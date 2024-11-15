using System;
using UnityEngine;

namespace Level.Obstacle
{
    /// <summary>
    /// Данный класс отвечает за триггер, с помощью которого вызывается перемещение препятствия
    /// </summary>
    public class ObstacleMoveTrigger : MonoBehaviour
    {
        // Событие используется для оповещения о том, что игрок вошел в область триггера
        public event Action PlayerEntered;
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Проверяем была ли коллизия с тегом
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PlayerEntered?.Invoke();
            }
        }
    }
}