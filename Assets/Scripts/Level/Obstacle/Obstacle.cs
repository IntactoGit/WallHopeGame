using System;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using Random = Unity.Mathematics.Random;

namespace Level.Obstacle
{
    /// <summary>
    /// Класс, представляющий препятствие в игре
    /// </summary>
    public class Obstacle : MonoBehaviour
    {
        // Событие, срабатывающее, когда игрок проходит мимо препятствия
        public event Action PlayerPassedObstacle;

        [SerializeField]
        private float _minMoveY;
        [SerializeField]
        private float _maxMoveY;
        [SerializeField]
        private float _moveDuration;
        // Поле которое представляет вероятность (от 0 до 1), с которой препятствие будет перемещаться в игре.
        [SerializeField]
        private float _probabilityOfMoving = 0.3f;

        private SpriteRenderer _sprite;
        
        [SerializeField]
        private ObstacleMoveTrigger _obstacleMoveTrigger;
        [SerializeField]
        private ObstaclePassedTrigger _obstaclePassedTrigger;

        
        private void Awake()
        {
            // Получаем ссылку на компонент
            _sprite = GetComponent<SpriteRenderer>();
            
            _obstacleMoveTrigger.PlayerEntered += MoveObstacleWithRandomChance;
            _obstaclePassedTrigger.PlayerPassedObstacle += OnPlayerPassedObstacle;
        }

        /// <summary>
        /// Метод вызывается при уничтожении объекта и отписывает методы от события триггеров
        /// </summary>
        private void OnDestroy()
        {
            _obstacleMoveTrigger.PlayerEntered -= MoveObstacleWithRandomChance;
            _obstaclePassedTrigger.PlayerPassedObstacle -= OnPlayerPassedObstacle;
        }

        

        /// <summary>
        /// Метод проверяет коллизии объекта с другими коллайдерами
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Проверяем была ли коллизия с компонентом
            if (collision.gameObject.TryGetComponent(out PlayerController playerController))
            {
                playerController.DestroyPlayer();
            }
        }
        /// <summary>
        /// Перед препятсвием стоит триггер, который вызывает перемещение препятствия по координате по "y" 
        /// </summary>
        
        private void MoveObstacleWithRandomChance()
        {
            // Генерируем случайную вероятность от 0 до 1.
            var randomChance = UnityEngine.Random.value;
           
            // Если вероятность меньше или равна заданной, то перемещаем препятствие
            if (randomChance <= _probabilityOfMoving)
            {
                var randomMoveY = UnityEngine.Random.Range(_minMoveY, _maxMoveY);
                var nextPosition = transform.position.y + randomMoveY;
                transform.DOMoveY(nextPosition, _moveDuration);
            }
        }

        /// <summary>
        /// Метод инициализирует размер препятствия
        /// </summary>
        public void Initialize(float height)
        {
            var newSize = new Vector2(_sprite.size.x, height);
            _sprite.size = newSize;
        }

        protected virtual void OnPlayerPassedObstacle()
        {
            PlayerPassedObstacle?.Invoke();
        }
    }
}