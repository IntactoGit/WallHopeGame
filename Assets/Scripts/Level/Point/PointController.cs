using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Level.Point
{
    /// <summary>
    /// Класс, отвечающий за управление созданием и уничтожением игровых точек.
    /// </summary>
    public class PointController : MonoBehaviour
    {
        // Событие, возникающее при сборе очков. Передает количество собранных очков.
        public event Action<int> RewardAdded;
        
        [SerializeField]
        private Point _pointPrefab;
        // Высота, на которой создаются точки
        [SerializeField]
        private float _pointPositionY; 
        [SerializeField]
        private int _rewardPerPoint = 1;
       
        private float _destroyPointDuration = 0.3f;
       
        private readonly List<Point> _points = new();
        
        /// <summary>
        /// Создает игровую точку, передавая ей случайное значение награды в заданном диапазоне.
        /// </summary>
        public void SpawnPoint(Vector3 position)
        {
            var pointPosition = new Vector3(position.x, _pointPositionY);
            // Создаем новую игровую точку
            var point = Instantiate(_pointPrefab, pointPosition, Quaternion.identity, transform);
            point.Reward = _rewardPerPoint;
            // Подписываем метод на событие
            point.PointCollected += OnPointCollected;
            point.PointMissed += OnPointMissed;
            _points.Add(point);
            
        }
        /// <summary>
        /// Обработчик события, вызываемого при взаимодействии игрока с точкой.
        /// </summary>
       
        private void OnPointCollected(Point point)
        {
            // Вызываем событие RewardAdded, передавая ему награду, связанную с данной точкой
            RewardAdded?.Invoke(point.Reward);

            point.PointCollected -= OnPointCollected;
            point.PointMissed -= OnPointMissed;

            _points.Remove(point);
            Destroy(point.gameObject);
        }
        /// <summary>
        /// Обработчик события, вызываемог, когда игрок пропустил препятсвие
        /// </summary>
        private void OnPointMissed(Point point)
        {
            point.PointCollected -= OnPointCollected;
            point.PointMissed -= OnPointMissed;
            _points.Remove(point);
            Destroy(point.gameObject);
        }
        
        /// <summary>
        /// Уничтожает все созданные игровые точки с анимацией уменьшения.
        /// </summary>
        public void DestroyAllPoints()
        {
            foreach (var point in _points)
            {
                // Отписываем метод текущей точку от события
                point.PointCollected -= OnPointCollected;
                // Уменьшаем масштаб точки
                point.transform
                    .DOScaleX(0f, _destroyPointDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(point.gameObject));
            }
            _points.Clear();
        }
        
        
        
    }
}