using UnityEngine;
using Random = UnityEngine.Random;

namespace AlexzanderCowell
{

    public class CharacterMovementScript : MonoBehaviour
    {
        [Header("Inspector Changes")]
        [Range(-50,50)]
        [SerializeField] private float gravitySlider;
        [Range(0,30)]
        [SerializeField] private float walkSpeed = 6;
        [SerializeField] private float downValue, upValue;
        /*[SerializeField] private LayerMask collisionLayer;*/
        [SerializeField] private float jumpHeight;
        /*[SerializeField] private GameObject[] weaponAndShield;
        [TagSelector]
        [SerializeField] private string thisTag;*/
        [SerializeField] private float _mouseSensitivityY;
        [SerializeField] private float _mouseSensitivityX;
        [SerializeField] private Camera _playerCamera;
        
        [Header("Internal Edits")]
        public static CharacterController _controller;
        private bool _runFaster;
        
        private float _normalWalkSpeed;
        private float _mouseXposition,
            _moveHorizontal,
            _moveVertical,
            _mouseYposition;
        private Vector3 _moveDirection;
        private Animator _playersAnimation;
        private float _timeElapsed = 0;
        private float _duration = 3;
        private float idleTimer = 2;
        private float _idleResetTimer;
        private bool _playerIsJumping;
        private float _running;
        private bool _isRunning;
        private bool _weaponKey;
        private int _weaponButtonCounter;
        private float _randomAttack;
        private bool checkAttackNumber;
        private float storedAttackNumber;
        public static bool _playerIsBlocking;
        public static bool _playerIsAttacking;
        private bool _playerIsInAttackRange;
        private Vector3 _playerRotation;

        private void Awake()
        {
            _playerRotation = transform.rotation.eulerAngles;
            _controller = GetComponent<CharacterController>();
            _playersAnimation = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _running = walkSpeed * 2;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
            Cursor.visible = false;
            _normalWalkSpeed = walkSpeed;
            _runFaster = false;
            _idleResetTimer = idleTimer;
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(thisTag))
            {
                _playerIsInAttackRange = true;
            }
        }*/

        private void FixedUpdate()
        {
            CharacterGravity();
            CharacterMovementBase();
        }

        private void Update()
        {
            /*CombatAnimations();*/
            TooLongIdleState();
            /*WeaponDisplay();*/
            PlayerMovementAnimations();
            /*JumpMovement();*/
            RunningMovement();
        }

        private void CharacterMovementBase()
        {
            transform.rotation = Quaternion.Euler(_playerRotation);
            _mouseXposition += Input.GetAxis("Mouse X") * _mouseSensitivityX;
            _mouseYposition -= Input.GetAxis("Mouse Y") * _mouseSensitivityY;
            _mouseYposition = Mathf.Clamp(_mouseYposition, downValue, upValue);
            
            transform.rotation = Quaternion.Euler(0f, _mouseXposition, 0f);
            _playerCamera.transform.localRotation = Quaternion.Euler(_mouseYposition, 0f, 0f);
            
                _moveHorizontal = Input.GetAxis("Horizontal"); // Gets the horizontal movement of the character.
                _moveVertical = Input.GetAxis("Vertical"); // Gets the vertical movement of the character.
                
            Vector3 movement = new Vector3(_moveHorizontal, 0f, _moveVertical); // Allows the character to move forwards and backwards & left & right.
            movement = transform.TransformDirection(movement) * walkSpeed; // Gives the character movement speed.
            _controller.Move((movement + _moveDirection) * Time.deltaTime); // Gets all the movement variables and moves the character.
        }
        

        private void CharacterGravity()
        {
            _moveDirection.y -= gravitySlider * Time.deltaTime;
            // Move the character controller with gravity
            _controller.Move(_moveDirection * Time.deltaTime);
        }
        
        /*private void JumpMovement() 
        {
            if (_moveDirection.y > 0 && !IsAnimationPlaying("Jumping"))
            {
                _playersAnimation.SetBool("IsJumping", true);
            }
            else if (IsAnimationPlaying("Jumping"))
            {
                _playersAnimation.SetBool("IsJumping", false);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerIsJumping = true;
            }

            if (_playerIsJumping && (_controller.isGrounded)) // If player hits the space bar and the character is touching the ground it will allow the character to jump.)
            {
                _moveDirection.y = Mathf.Sqrt(5f * jumpHeight * gravitySlider);
                _moveDirection.y -= gravitySlider * Time.deltaTime;
                _playerIsJumping = false;
            }
            
        }*/
        
        bool IsAnimationPlaying(string clipName)
        {
            return _playersAnimation.GetCurrentAnimatorStateInfo(0).IsName(clipName);
        }

        private void RunningMovement()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                _isRunning = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                _isRunning = false;
            }

            if (_isRunning)
            {
                walkSpeed = _running;
            }
            else
            {
                walkSpeed = _normalWalkSpeed;
            }
            
        }

        private void PlayerMovementAnimations()
        {
            if (_moveHorizontal > 0 || _moveVertical > 0 || _moveHorizontal < 0 || _moveVertical < 0)
            {
                if (walkSpeed == _normalWalkSpeed)
                {
                    _playersAnimation.SetFloat("Blend", 0.4f, 0.2f, Time.deltaTime);
                }
                else if (walkSpeed == _running)
                {
                    _playersAnimation.SetFloat("Blend", 0.9f, 0.2f, Time.deltaTime);
                }
                else if (_moveVertical < 0)
                {
                    _playersAnimation.SetFloat("Blend", -1, 0.2f, Time.deltaTime);
                }
            }
            else
            {
                _playersAnimation.SetFloat("Blend", 0, 0.2f, Time.deltaTime);
            }

            if (_moveHorizontal > 0 || _moveVertical > 0)
            {
                idleTimer -= 0.5f * Time.deltaTime;
            }
            
            if (idleTimer < 0.2f && _moveHorizontal > 0)
            {
                idleTimer = 0;
                _playersAnimation.SetBool("SleepAnimation", true);
            }
            else
            {
                _playersAnimation.SetBool("SleepAnimation", false);
                idleTimer = _idleResetTimer;
            }
        }

        /*private void WeaponDisplay()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _weaponKey = true;
            }
            if (_weaponKey)
            {
                _weaponButtonCounter += 1;
                _weaponKey = false;
            }
            

            if (_weaponButtonCounter == 1)
            {
                weaponAndShield[0].SetActive(true);
                weaponAndShield[1].SetActive(true);
            }
            else
            {
                weaponAndShield[0].SetActive(false);
                weaponAndShield[1].SetActive(false);
            }
            if (_weaponButtonCounter == 2)
            {
                _weaponButtonCounter = 0;
            }
        }*/

        private void TooLongIdleState()
        {
            /*if (_timeElapsed < _duration)
            {
                float t = _timeElapsed / _duration;
                walkSpeed = Mathf.Lerp(0, 8, t);
                _timeElapsed += Time.deltaTime;
            }
            else
            {
                walkSpeed = 5;
            }*/
            
            /*Debug.Log("Idle Timer " + idleTimer);*/
            
            
            /*Debug.Log("Horizontal " + _moveHorizontal);
            Debug.Log("Vertical " + _moveVertical);*/
        }

        /*private void CombatAnimations()
        {
            _randomAttack = Random.Range(0, 3);
            
            // gets the current animation time and stores it in a variable
            float currentBaseState = _playersAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime;
            
            // uses the currentBaseState and checks if the animation is still playing or not.
           
            if (Input.GetKey(KeyCode.Mouse0) && currentBaseState > 0.5f)
            {
                checkAttackNumber = true;
                
                if (_playerIsInAttackRange)
                {
                    _playerIsAttacking = true;
                }
                else
                {
                    _playerIsAttacking = false;
                }
            }
            else if (currentBaseState < 0.5f)
            {
                storedAttackNumber = 4;
                _playersAnimation.SetInteger("Attack", 0);
                
            }
            
            if (checkAttackNumber)
            {
                storedAttackNumber = _randomAttack;
                checkAttackNumber = false;
            }

            if (_weaponButtonCounter == 1)
            {
                if (storedAttackNumber == 0)
                {
                    _playersAnimation.SetInteger("Attack", 1);
                }
                else if (storedAttackNumber == 1)
                {
                    _playersAnimation.SetInteger("Attack", 2);
                    
                }
                else if (storedAttackNumber == 2)
                {
                    _playersAnimation.SetInteger("Attack", 3);
                   
                }
                else if (storedAttackNumber == 3)
                {
                    _playersAnimation.SetInteger("Attack", 1);
                   
                }
                
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    _playersAnimation.SetBool("ShieldBlock", true);
                    _playerIsBlocking = true;
                }
                else if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    _playersAnimation.SetBool("ShieldBlock", false);
                    _playerIsBlocking = false;
                }
            }
            
            if (_weaponButtonCounter == 0)
            {
                if (storedAttackNumber == 0)
                {
                    _playersAnimation.SetInteger("Attack", 4);
                }
                else if (storedAttackNumber == 1)
                {
                    _playersAnimation.SetInteger("Attack", 5);
                   
                }
                else if (storedAttackNumber == 2)
                {
                    _playersAnimation.SetInteger("Attack", 5);
                }
                else if (storedAttackNumber == 3)
                {
                    _playersAnimation.SetInteger("Attack", 4);
                }
            }
            
            
        }*/
        
        /*private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(thisTag))
            {
                _playerIsInAttackRange = false;
            }
        }*/


    }
}
