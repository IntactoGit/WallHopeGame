using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// Класс GameOverScreen отображает экран поражения и обрабатывает взаимодействие с ним.
    /// </summary>
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private ColorProvider _colorProvider;
        [SerializeField]
        private TextMeshProUGUI _currentScoreLabel;
        [SerializeField]
        private TextMeshProUGUI _bestScoreLabel;
        [SerializeField]
        private float _newBestScoreAnimationDuration = 0.3f;
        [SerializeField]
        private AudioSource _bestScoreChangedAudio;


        private void Awake()
        {
            // Получаем ссылку на основную камеру и устанавливаем цвет фона
            Camera.main.backgroundColor = _colorProvider.CurrentColor;

            // Получаем текущий счет и лучший счет из данных
            var currentScore = PlayerPrefs.GetInt(GlobalConstants.SCORE_PREFS_KEY);
            var bestScore = PlayerPrefs.GetInt(GlobalConstants.BEST_SCORE_PREFS_KEY);

            // Если текущий счет превышает лучший счет, обновляем лучший счет и анимируем его
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                ShowNewBestScoreAnimation();
                SaveNewBestScore(bestScore);
            }

            // Обновляем тексты текущего счета и лучшего счета на экране
            _currentScoreLabel.text = currentScore.ToString();
            _bestScoreLabel.text = $"BEST {bestScore.ToString()}";
        }

        /// <summary>
        /// Запускает анимацию нового лучшего счета.
        /// </summary>
        private void ShowNewBestScoreAnimation()
        {
            _bestScoreLabel.transform.DOPunchScale(Vector3.one, _newBestScoreAnimationDuration, 0);
            _bestScoreChangedAudio.Play();
        }

        /// <summary>
        /// Сохраняет новый лучший счет в данных.
        /// </summary>
        private void SaveNewBestScore(int bestScore)
        {
            PlayerPrefs.SetInt(GlobalConstants.BEST_SCORE_PREFS_KEY, bestScore);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// Вызывается при нажатии на кнопку рестарта игры.
        /// </summary>
        [UsedImplicitly]
        public void RestartGame()
        {
            // Обновляем цвет фона
            _colorProvider.CurrentColor = _colorProvider.GetRandomColor(_colorProvider.CurrentColor);
            SceneManager.LoadSceneAsync(GlobalConstants.GAME_SCENE);
        }

        /// <summary>
        /// Метод выходит из игры по нажатию кнопки
        /// </summary>
        [UsedImplicitly]
        public void ExitGame()
        {
            //  блок кода, заключенный в #if UNITY_EDITOR и #endif, содержит комментарий о том,
            // что этот код будет выполняться только в редакторе Unity
#if UNITY_EDITOR
            // Остановка play мода
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            // Выход из приложения
            Application.Quit();
        }
    }
}