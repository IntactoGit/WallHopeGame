using System;
using UnityEngine;

namespace Player
{
    
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _jumpForce;
        [SerializeField]
        private int _maxJumpCount = 2;

        // Событие, вызываемое при смерти игрока.
        public event Action PlayerDied;
        
        [SerializeField]
        private GameObject _playerDiedFxPrefab;
        
        private int _jumpCount;
        private Rigidbody2D _rigidbody2D;  
        
        [SerializeField]
        private AudioSource _moveAudio;
        [SerializeField]
        private AudioSource _landingAudio;
        private EchoEffect _echoEffect;
        private bool _isGrounded;
        
        private void Awake()
        {
            // Получаем ссылку на компонент
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _echoEffect = GetComponent<EchoEffect>();
            _jumpCount = 0;
        }
        private void Jump()
        {
            _moveAudio.Play();
            _rigidbody2D.linearVelocity = Vector2.up * _jumpForce * _rigidbody2D.gravityScale;
            _jumpCount--;
        }
        /// <summary>
        /// Метод проверяет, было ли запрошено выполнение прыжка путем нажатия клавиши "пробел" или касания экрана.
        /// </summary>
        private void Update()
        {
            bool isJumpRequested = CheckJumpInput();

            if (CanJump() && isJumpRequested)
            {
                Jump();
                _echoEffect.CanShowEcho(true);
            }
        }
        private bool CheckJumpInput()
        {
            bool isSpaceButton = Input.GetKeyDown(KeyCode.Space);
            bool isTouchInput = Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

            return isSpaceButton || isTouchInput;
        }
        /// <summary>
        /// Метод вызывается при столкновении объекта с другим коллайдером. Проверяет столкновение с землей и обрабатывает его.
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.collider.CompareTag(GlobalConstants.FLOOR_TAG) && !_isGrounded)
            {
                _landingAudio.Play();
                _jumpCount = _maxJumpCount;
                _isGrounded = true;
            }
        }
        /// <summary>
        /// Метод вызывается при завершении столкновения объекта с другим коллайдером.
        /// </summary>
        private void OnCollisionExit2D(Collision2D collision2D)
        {
            if (collision2D.collider.CompareTag(GlobalConstants.FLOOR_TAG))
            {
                _isGrounded = false;
            }
        }
        
        private bool CanJump()
        {
            return _jumpCount > 0;
        }
       
        /// <summary>
        /// Метод уничтожает объект игрока 
        /// </summary>
        public void DestroyPlayer()
        {
            Instantiate(_playerDiedFxPrefab, transform.position, Quaternion.identity);
            PlayerDied?.Invoke();
            
            Destroy(gameObject);
        }
        
        
    }
}