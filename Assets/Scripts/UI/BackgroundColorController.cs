using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class BackgroundColorController : MonoBehaviour
    {
        [SerializeField]
        private ColorProvider _colorProvider;
        [SerializeField]
        private float _colorChangeDuration; 

        private Camera _camera; 
        
        private void Start()
        {
            // Получаем ссылку на основную камеру, используя статическое свойство Camera.main.
            _camera = Camera.main;
    
            // Устанавливаем начальный цвет фона камеры на основе текущего цвета провайдера.
            Camera.main.backgroundColor = _colorProvider.CurrentColor;
        }
        
        /// <summary>
        /// Метод ChangeColor() запускает анимацию изменения цвета фона камеры.
        /// </summary>
        public void ChangeColor()
        {
            // Получаем следующий случайный цвет, исключая текущий цвет.
            var nextColor = _colorProvider.GetRandomColor(except: _colorProvider.CurrentColor);

            // Запускаем анимацию изменения цвета фона с использованием DoTween.
            _camera.DOColor(nextColor, _colorChangeDuration).SetEase(Ease.OutFlash);

            // Обновляем текущий цвет.
            _colorProvider.CurrentColor = nextColor;
        }
    }
}