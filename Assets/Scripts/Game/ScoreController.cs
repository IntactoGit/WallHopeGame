using System;
using UnityEngine;

namespace Game
{
    public class ScoreController : MonoBehaviour
    {
        // Событие, срабатывающее при изменении счета.
        public event Action<int> ScoreChanged;

        [SerializeField]
        private AudioSource _scoreChangeAudio;

        private int _score;
        
        /// <summary>
        /// Метод увеличивает счет игрока на заданное количество очков и вызывает событие изменения счета.
        /// </summary>
        public void AddScore(int score)
        {
            _score += score;
            // Вызываем событие для уведомления о изменении счета.
            ScoreChanged?.Invoke(_score);
            // Проигрываем звук изменения счета
            _scoreChangeAudio.Play();
        }
        /// <summary>
        /// Метод вызывается при уничтожении объекта, сохраняет текущий счет в данных игры.
        /// </summary>
        private void OnDestroy()
        {
            // Записываем текущий счет в данные игры.
            PlayerPrefs.SetInt(GlobalConstants.SCORE_PREFS_KEY, _score);
            // Сохраняем изменения в данных.
            PlayerPrefs.Save();
        }
        
        
        
        
        
        
    }
}