using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class EchoEffect : MonoBehaviour
    {
        // Количество начальных "эхо" в пуле.
        private const int ECHO_COUNT = 50;

        // Родительский объект, в котором создадутся объекты префаба.
        [SerializeField]
        private Transform _echoParentRoot;
        [SerializeField]
        private GameObject _echoPrefab;
        [SerializeField]
        private float _spawnDelay = 0.1f;
        [SerializeField]
        private float _shrinkDuration = 0.8f;

        private readonly Queue<GameObject> _echoObjectsPool = new();
        private float _nextEchoSpawnTime;
        private bool _isEchoEnabled;
        private Vector3 _initialEchoScale;
    
        /// <summary>
        /// Отображает эхо, если выполнены условия.
        /// </summary>
        private void SpawnInitialEcho()
        {
            for (int i = 0; i < ECHO_COUNT; i++)
            {
                var echo = Instantiate(_echoPrefab, _echoParentRoot);
                echo.SetActive(false);
                _echoObjectsPool.Enqueue(echo);
            }
        }
        /// <summary>
        /// Метод, вызываемый при старте объекта. Здесь подготавливаются начальные условия для работы эффекта "Эхо".
        /// </summary>
        private void Awake()
        {
            _initialEchoScale = _echoPrefab.transform.localScale;
            _isEchoEnabled = true;
            SpawnInitialEcho();
        }
        /// <summary>
        /// Управляет процессом создания эхо.
        /// </summary>
        private void HandleEchoSpawn()
        {
            if (_isEchoEnabled)
            {
                // Вызываем метод отображения эхо
                ShowEcho(); 
            }
        }
        /// <summary>
        /// Отображает эхо, если выполнены условия.
        /// </summary>
        private void ShowEcho()
        {
            _nextEchoSpawnTime -= Time.deltaTime;

            if (_nextEchoSpawnTime <= 0 && !IsPoolEmpty())
            {
                var echo = _echoObjectsPool.Dequeue();
                InitializeEcho(echo);
                AnimateEcho(echo);
            }
        }
        /// <summary>
        /// Проверяет, пуст ли пул эхо.
        /// </summary>
        /// <returns>Возвращает true, если пул пуст, иначе false.</returns>
        private bool IsPoolEmpty()
        {
            return _echoObjectsPool.Count == 0;
        }
        /// <summary>
        /// Устанавливает начальное положение и состояние эхо.
        /// </summary>
        private void InitializeEcho(GameObject echo)
        {
            echo.transform.position = transform.position;
            echo.SetActive(true);
            _nextEchoSpawnTime = _spawnDelay;
        }
        /// <summary>
        /// Анимирует масштаб эхо и возвращает его в пул после завершения.
        /// </summary>
        private void AnimateEcho(GameObject echo)
        {
            echo.transform.localScale = _initialEchoScale;
            echo.transform.DOScale(Vector3.zero, _shrinkDuration).OnComplete(() =>
            {
                echo.SetActive(false);
                _echoObjectsPool.Enqueue(echo);
            });
        }
        /// <summary>
        /// Включает или выключает возможность показывать эхо.
        /// </summary>
        public void CanShowEcho(bool isVisible)
        {
            _isEchoEnabled = isVisible;
        }
        private void Update()
        {
            HandleEchoSpawn();
        }
    }
}
