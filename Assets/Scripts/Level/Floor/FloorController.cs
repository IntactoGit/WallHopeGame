using UnityEngine;

namespace Level.Floor
{
    /// <summary>
    /// Данный класс отвечает за перемещение пола в игре.
    /// В нашей игре будет два объекта пола, которые мы будем перещать друг за другом.
    /// </summary>
    public class FloorController : MonoBehaviour
    {
        [SerializeField]
        private Floor _floorPrefab;
        [SerializeField]
        private Vector3 _startPosition;

        private Floor _firstFloor;
        private Floor _secondFloor;
       
        private void Awake()
        {
            // Создаем первый пол
            _firstFloor = Instantiate(_floorPrefab, _startPosition, Quaternion.identity, transform);
            _firstFloor.PlayerPassedCurrentFloor += OnPlayerPassedCurrentFloor;
            // Получаем позицию для второго пола
            var nextFloorPosition = GetNextFloorPosition(_firstFloor);

            // Создаем второй пол
            _secondFloor = Instantiate(_floorPrefab, nextFloorPosition, Quaternion.identity, transform);
            _secondFloor.PlayerPassedCurrentFloor += OnPlayerPassedCurrentFloor;
        }
        private Vector3 GetNextFloorPosition(Floor currentFloor)
        {
            var position = currentFloor.transform.position;
            var currentFloorSize = currentFloor.GetSize();
            var nextFloorPositionX = currentFloorSize.x + position.x;
            var nextFloorPosition = new Vector3(nextFloorPositionX, position.y, position.z);
            return nextFloorPosition;
        }
        
        private void OnPlayerPassedCurrentFloor(Floor currentFloor)
        {
            var nextFloorPosition = GetNextFloorPosition(currentFloor);
            if (_firstFloor == currentFloor)
            {
                _secondFloor.transform.position = nextFloorPosition;
            }
            else
            {
                _firstFloor.transform.position = nextFloorPosition;
            }
        }
        
        /// <summary>
        /// Метод вызывается при уничтожении сцены
        /// </summary>
        private void OnDestroy()
        {
            _firstFloor.PlayerPassedCurrentFloor -= OnPlayerPassedCurrentFloor;
            _secondFloor.PlayerPassedCurrentFloor -= OnPlayerPassedCurrentFloor;
        }
        
        
    }
}