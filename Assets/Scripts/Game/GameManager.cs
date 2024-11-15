using System.Collections;
using System.Collections.Generic;
using Level;
using Level.Obstacle;
using Level.Point;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// Класс GameManager отвечает за координацию и управление основными элементами игры.
    /// Он обрабатывает события и взаимодействия между различными контроллерами игры,
    /// обеспечивая их взаимодействие и корректное функционирование игровых механик.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        [Range(0.1f, 1)]
        private float _pointSpawnProbability = 0.7f;
    
        [SerializeField]
        private PointController _pointController;
        [SerializeField]
        private ObstacleController _obstacleController;
        
        [SerializeField]
        private PlayerController _player;
        [SerializeField]
        private LevelMover _levelMover;
        
        [SerializeField]
        private ScoreController _scoreController;
        [SerializeField]
        private ScoreView _scoreView;
        
        [SerializeField]
        private BackgroundColorController _backgroundColorController;
        [Tooltip("Points required to change background color")]
        [SerializeField]
        private int _colorChangePeriodInPoints = 5;
        
        [SerializeField]
        private int _difficultyIncreasePeriodInPoints = 10;
        
        [SerializeField]
        private float _sceneChangeDelay = 1f;
        
        /// <summary>
        /// Обработчик события изменения позиции препятствий.
        /// </summary>
        private void OnObstacleChangedPosition(Vector3 position)
        {
            // Генерируем случайное число от 0 до 1
            var randomValue = Random.value;
        
            // Проверяем вероятность создания точки
            if (randomValue <= _pointSpawnProbability)
            {
                _pointController.SpawnPoint(position);
            }
        }
        
        /// <summary>
        /// Обработчик события сбора поинтов.
        /// </summary>
        
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            _obstacleController.ObstacleChangedPosition += OnObstacleChangedPosition;
            _pointController.RewardAdded += _scoreController.AddScore;
            _player.PlayerDied += OnPlayerDied;
            
            _scoreController.ScoreChanged += OnScoreChanged;
        }
        
        /// <summary>
        /// Обработчик события изменения позиции препятствий.
        /// </summary>
        private void OnDestroy()
        {
            _obstacleController.ObstacleChangedPosition -= OnObstacleChangedPosition;
            _pointController.RewardAdded -= _scoreController.AddScore;
            
            _player.PlayerDied -= OnPlayerDied;
            _scoreController.ScoreChanged -= OnScoreChanged;
        }
        
        /// <summary>
        /// Обработчик события смерти игрока.
        /// </summary>
        private void OnPlayerDied()
        {   
            // Отключаем движение уровня и уничтожаем объекты
            _levelMover.enabled = false;
            _obstacleController.DestroyAllObstacles();
            _pointController.DestroyAllPoints();
            // Запускаем метод, который представляет собой корутину
            StartCoroutine(LoadGameOverSceneWithDelay());
        }

        /// <summary>
        /// Обработчик события изменения очков
        /// </summary>
        private void OnScoreChanged(int score)
        {
            _scoreView.UpdateScoreLabel(score);
            // При достижении определенного количества очков, меняем цвет фона
            if (score % _colorChangePeriodInPoints == 0)
            {
                _backgroundColorController.ChangeColor();
            }
            // При достижении определенного количества очков, меняем скорость уровня
            if (score % _difficultyIncreasePeriodInPoints == 0)
            {
                _levelMover.IncreaseSpeed();
            }
        }

        /// <summary>
        /// Загружает сцену Game Over с небольшой задержкой после смерти игрока,
        /// чтобы успели проиграться анимация и звук смерти игрока
        /// </summary>
        private IEnumerator LoadGameOverSceneWithDelay()
        {
            // Слово "yield" используется для создания задержки в выполнении кода.
            // Метод "WaitForSeconds" приостанавливает выполнение текущей корутины (или метода) на указанное количество секунд.
            // Это позволяет создавать временные задержки в игре, например, перед переключением сцен.
            // Более подробную информацию о корутинах и "yield" можно найти на курсах или в ресурсах онлайн.
            yield return new WaitForSeconds(_sceneChangeDelay);
            SceneManager.LoadSceneAsync(GlobalConstants.GAME_OVER_SCENE);
        }
        
        
        
    }
    
}