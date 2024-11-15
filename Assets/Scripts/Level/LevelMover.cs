using UnityEngine;

namespace Level
{
    public class LevelMover : MonoBehaviour
    {
        [SerializeField]
        private float _floorSpeed;
        [SerializeField]
        private float _speedIncreaseDelta = 0.15f; 
    
        private void Update()
        {
            // Перемещаем объект влево
            transform.Translate(-Time.deltaTime * _floorSpeed, 0, 0);
        }
        
        /// <summary>
        /// Метод для увеличения скорости перемещения уровня.
        /// </summary>
        public void IncreaseSpeed()
        {
            _floorSpeed += _speedIncreaseDelta;
        }
        
        
    }
}