using System;
using UnityEngine;

namespace Level.Point
{
    /// <summary>
    /// Класс, представляющий игровой объект, который может быть собран игроком.
    /// За каждый собранный поинт может быть предоставлена награда.
    /// </summary>
    public class Point : MonoBehaviour
    {
        // Событие, возникающее при сборе поинта игроком.
        public event Action<Point> PointCollected;

        // Награда за точку
        public int Reward { get; set; }
        
        // Событие, возникающее, если игрок пропустил поинт.
        public event Action<Point> PointMissed;

        [SerializeField]
        private PointMissedTrigger _pointMissedTrigger;
        /// <summary>
        /// Метод, вызываемый при столкновении объекта с триггерами.
        /// Проверяет, была ли коллизия с игроком, и при необходимости вызывает событие PointCollected.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Проверяем, была ли коллизия с игроком
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PointCollected?.Invoke(this);
            }
        }
        /// <summary>
        /// Метод обрабатывает событие PointMissed
        /// </summary>
        private void OnPointMissed()
        {
            PointMissed?.Invoke(this);
        }
        
        private void Start()
        {
            _pointMissedTrigger.PointMissed += OnPointMissed;
        }

        private void OnDestroy()
        {
            _pointMissedTrigger.PointMissed -= OnPointMissed;
        }
        
        
        
        
        
        
        
    }
}
