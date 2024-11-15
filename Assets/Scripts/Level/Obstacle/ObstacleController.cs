using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.Obstacle
{
    /// <summary>
    /// Класс, отвечающий за управление препятствиями на уровне.
    /// </summary>
    public class ObstacleController : MonoBehaviour
    {
       private const int COUNT_INITIAL_OBSTACLES = 10;

        [SerializeField]
        private Obstacle _obstaclePrefab;
        [SerializeField]
        private float _minDistanceBetweenObstaclesX = 4.0f;
        [SerializeField]
        private float _maxDistanceBetweenObstaclesX = 8.0f;
        [SerializeField]
        private float _minObstacleHeight = 1.2f;
        [SerializeField]
        private float _maxObstacleHeight = 1.6f;
        [SerializeField]
        private float _minObstaclePositionY = -0.5f;
        [SerializeField]
        private float _maxObstaclePositionY = -2.5f;

        [SerializeField]
        private float _destroyObstacleDuration = 0.3f;
        
        private readonly Queue<Obstacle> _obstacles = new();
        // Событие, срабатывающее при изменении позиции препятствия
        public event Action<Vector3> ObstacleChangedPosition;
        private void Start()
        {
            // Создаем начальные препятствия при запуске сцены.
            SpawnInitialObstacles();
        }

        /// <summary>
        /// Метод создает начальный пулл препятствий.
        /// </summary>
        private void SpawnInitialObstacles()
        {
            for (int i = 0; i < COUNT_INITIAL_OBSTACLES; i++)
            {
                SpawnObstacle();
            }
        }

        /// <summary>
        /// Метод создает и инициализирует препятствие.
        /// </summary>
        private void SpawnObstacle()
        {
            var obstacle = Instantiate(_obstaclePrefab, transform);
            InitializeObstacle(obstacle);
            SetObstaclePosition(obstacle);
            _obstacles.Enqueue(obstacle);
        }

        /// <summary>
        /// Метод инициализирует препятствие 
        /// </summary>
        private void InitializeObstacle(Obstacle obstacle)
        {
            var obstacleRandomHeight = Random.Range(_minObstacleHeight, _maxObstacleHeight);
            obstacle.Initialize(obstacleRandomHeight);
            obstacle.PlayerPassedObstacle += OnPlayerPassed;
        }

        /// <summary>
        /// Метод устанавливает позицию препятствия на уровне.
        /// </summary>
        private void SetObstaclePosition(Obstacle obstacle)
        {
            var previousObstacle = _obstacles.Count > 0 ? _obstacles.Last() : null;
            var previousPosition = GetPreviousPosition(previousObstacle);
            var nextRandomPosition = GetNextRandomPosition(previousPosition);
            obstacle.transform.position = nextRandomPosition;
            if (_obstacles.Count > 1)
            {
                var positionBetweenObstacle = (previousPosition + obstacle.transform.position) / 2;
                ObstacleChangedPosition?.Invoke(positionBetweenObstacle);
            }
        }

        /// <summary>
        /// Метод вызывается при прохождении игроком препятствия.
        /// </summary>
        private void OnPlayerPassed()
        {
            MoveObstacleToEndTrack();
        }

        /// <summary>
        /// Метод перемещает препятствия в конец уровня при прохождении игроком.
        /// </summary>
        private void MoveObstacleToEndTrack()
        {
            var obstacle = _obstacles.Dequeue();
            SetObstaclePosition(obstacle);
            _obstacles.Enqueue(obstacle);
        }

        /// <summary>
        /// Метод возвращает позицию предыдущего препятствия.
        /// </summary>
        private Vector3 GetPreviousPosition(Obstacle previousObstacle)
        {
            Vector3 previousPosition;
            if (previousObstacle == null)
            {
                var randomPositionX = Random.Range(_minDistanceBetweenObstaclesX, _maxDistanceBetweenObstaclesX);
                var randomPositionY = Random.Range(_minObstaclePositionY, _maxObstaclePositionY);
                previousPosition = new Vector3(randomPositionX, randomPositionY);
            }
            else
            {
                previousPosition = previousObstacle.transform.position;
            }

            return previousPosition;
        }

        /// <summary>
        /// Метод возвращает случайную позицию для следующего препятствия.
        /// </summary>
        private Vector3 GetNextRandomPosition(Vector3 previousPosition)
        {
            var randomSpawnPositionX = Random.Range(_minDistanceBetweenObstaclesX, _maxDistanceBetweenObstaclesX);
            var randomSpawnPositionY = Random.Range(_minObstaclePositionY, _maxObstaclePositionY);
            var spawnPosition = new Vector3(randomSpawnPositionX + previousPosition.x, randomSpawnPositionY,
                previousPosition.z);
            return spawnPosition;
        }

        /// <summary>
        /// Метод отписывает методы от событий
        /// </summary>
        private void OnDestroy()
        {
            foreach (var obstacle in _obstacles)
            {
                obstacle.PlayerPassedObstacle -= OnPlayerPassed;
            }
        }
        /// <summary>
        /// Метод уничтожает все препятствия на уровне с анимацией уменьшения.
        /// </summary>
        public void DestroyAllObstacles()
        {
            while(_obstacles.Count > 0)
            {
                var obstacle = _obstacles.Dequeue();
                obstacle.PlayerPassedObstacle -= OnPlayerPassed;
                obstacle.transform
                    .DOScaleX(0f, _destroyObstacleDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(obstacle.gameObject));
            }
        }
        
        
    }
}