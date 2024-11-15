using System;
using UnityEngine;

namespace Level.Point
{
    /// <summary>
    /// Данный класс отвечает за триггер, который сообщает, что игрок попустил point
    /// </summary>
    public class PointMissedTrigger : MonoBehaviour
    {
        // Событие используется для оповещения о том, что игрок попустил point
        public event Action PointMissed;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PointMissed?.Invoke();
            }
        }
        
        
        
        
        
    }
}