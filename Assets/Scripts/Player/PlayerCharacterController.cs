using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCharacterController : MonoBehaviour
{
    #region Type Definition

    public enum State
    {
        Normal,
        Dead,
        Paused,
    }

    #endregion
    #region Variables Definition

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public event EventHandler OnJumping;

    [Header("Movement Properties")]
    [SerializeField]
    private float _mouseSensitivity = 1f;
    [SerializeField]
    private float _moveSpeed = 30f;
    [SerializeField]
    private float _jumpSpeed = 50f;
    [SerializeField]
    private float _gravityDownForce = -98f;
    [SerializeField]
    private float _coyoteTime;

    [Header("Player Current State Enum")]
    public State state;

    [Header("UI Related Properties and GOs")]
    /*[SerializeField]
    private GameObject _RifleBulletPrefab;
    [SerializeField]
    private Transform _SpawnBulletPoint;
    [SerializeField]
    private AudioClip _ShotAudioClip;*/
    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private GameObject _playerModel;

    //UI Menu Game Objects
    //[SerializeField] 
    public GameObject pauseMenu;
    [SerializeField] 
    private GameObject winMenu;
    [SerializeField] 
    private GameObject deathMenu;

    //Other Classes
    private PlayerStats _playerStats;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerSound _myPlayerSound;

    private float _cameraVerticalAngle;
    private float _characterVelocityY;

    //Shoot Variables
    private double _shootTimer = 0f;
    private bool _isReloading;

    //Animators
    private Animator _playerCameraAnimator;
    private Animator _playerModelAnimator;

    //Jump Variables
    private float _coyoteTimer;
    bool _justJumped = false;
    int _jumpCount = 0;

    //Other Variables
    private bool _isMoving;
    private string _otherTag;
    [HideInInspector] public bool isRecoiling;
    private Vector3 _characterVelocityMomentum;

    //Coroutine for activar y cortar
    IEnumerator _myVibrationCoroutine;

    #endregion

    #region Events and Surface Type Routine
    private void PlayerCharacterController_OnStopMoving(object sender, EventArgs e)
    {
        _playerModelAnimator.SetBool("isWalking", _isMoving);
        _playerCameraAnimator.SetBool("isMoving", _isMoving);
        _myPlayerSound.PlaySound(PlayerSound.Actions.Stop, _otherTag);
    }

    private void PlayerCharacterController_OnStartMoving(object sender, EventArgs e)
    {
        _playerModelAnimator.SetBool("isWalking", _isMoving);
        _playerCameraAnimator.SetBool("isMoving", _isMoving);

        _myPlayerSound.PlaySound(PlayerSound.Actions.Run, _otherTag);
    }
    private void PlayerCharacterController_OnJumping(object sender, EventArgs e)
    {
        _playerCameraAnimator.SetBool("isJumping", _isMoving);

        _myPlayerSound.PlaySound(PlayerSound.Actions.Jump, _otherTag);
    }

    public void GetSurfaceStepped(string othersTag)
    {
        if (othersTag != _otherTag && _isMoving)
        {
            _otherTag = othersTag;
            _myPlayerSound.PlaySound(PlayerSound.Actions.Run, _otherTag);
        }
    }
    #endregion


    #region Awake y Update

    private void Start()
    {
        //Coroutine for activar y cortar
        _myVibrationCoroutine = VibrationEffect();

        //Trap cursor in window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Get the model animator as well as the camera animator
        _playerModelAnimator = _playerModel.GetComponent<Animator>();
        _playerCameraAnimator = _playerCamera.GetComponent<Animator>();

        //Initial state and reference PlayerStats class
        state = State.Normal;
        _isMoving = false;
        _playerStats = PlayerStats.myInstance;

        //Associate methods to the events
        OnStartMoving += PlayerCharacterController_OnStartMoving;
        OnStopMoving += PlayerCharacterController_OnStopMoving;
        OnJumping += PlayerCharacterController_OnJumping;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                HandleCharacterLook();
                HandleCharacterMovement();
                HandleShooting();
                HandleInteract();
                HandlePausing();
                HandleWeaponChange();
                //Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.None;
                break;
            case State.Dead:
#if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.None;
#endif
#if UNITY_STANDALONE_WIN
                Cursor.lockState = CursorLockMode.Confined;
#endif
                Cursor.visible = true;
                ActivateDeathMenu(deathMenu);
                break;
            case State.Paused:
                HandlePausing();
                break;
        }
    }
    #endregion


    #region Main Routine / Input Handler Methods

    private void HandleCharacterLook()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        // Rotate the transform with the input speed around its local Y axis
        transform.Rotate(new Vector3(0f, lookX * _mouseSensitivity, 0f), Space.Self);

        if (!isRecoiling)
        {
            // Add vertical inputs to the camera's vertical angle
            _cameraVerticalAngle -= lookY * _mouseSensitivity;

        // Limit the camera's vertical angle to min/max
        
            _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);
        // Apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
        _playerCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
        }
    }

    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        _playerModelAnimator.SetFloat("moveX", moveX);
        _playerModelAnimator.SetFloat("moveZ", moveZ);

        Vector3 lastPosition = transform.position;
        Vector3 characterVelocity = (transform.right * moveX * _moveSpeed + transform.forward * moveZ * _moveSpeed);

        //Jump Logic

        //If the player is grounded, or he isn't but coyote time is in effect or has double jump
        if (_characterController.isGrounded || !_characterController.isGrounded && _coyoteTimer < _coyoteTime || _playerStats.hasDoubleJump)
        {
            //Always Reset Gravity and CT after Grounding
            if (_characterController.isGrounded)
            {
                ResetGravityEffect();
                _coyoteTimer = 0f;
                _jumpCount = 0;
                _justJumped = false;
            }
            else
            {
                _coyoteTimer += Time.deltaTime;
                //Si esta en el aire, pero ya paso el coyote time, cuento como que perdio un salto (para el doble salto)
                if (!_justJumped && _coyoteTimer > _coyoteTime)
                {
                    _justJumped = true;
                    _jumpCount++;
                }
            }


            if (TestInputJump() && _jumpCount == 0)
            {
                _justJumped = true;
                _characterVelocityY = _jumpSpeed;
                _jumpCount++;
            }
            else if (_playerStats.hasDoubleJump && TestInputJump() && _jumpCount == 1)
            {
                _characterVelocityY = _jumpSpeed;
                _jumpCount++;
            }

            if (_justJumped)
                OnJumping?.Invoke(this, EventArgs.Empty);
        }

        // Apply gravity to the velocity
        _characterVelocityY += _gravityDownForce * Time.deltaTime;

        // Apply Y velocity to move vector
        characterVelocity.y = _characterVelocityY;

        // Apply momentum
        characterVelocity += _characterVelocityMomentum;

        // Move Character Controller
        _characterController.Move(characterVelocity * Time.deltaTime);

        // Dampen momentum
        if (_characterVelocityMomentum.magnitude > 0f)
        {
            float momentumDrag = 3f;
            _characterVelocityMomentum -= _characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (_characterVelocityMomentum.magnitude < .0f)
            {
                _characterVelocityMomentum = Vector3.zero;
            }
        }

        Vector3 newPosition = transform.position;

        //Usando isGrounded el sonido se trababa al frame que el player estaba en el aire (slopes y microcaidas)
        // Ahora usando !JustJumped es como aplicarle un coyote time a la caminata
        if (newPosition != lastPosition && !_justJumped)
        {
            // Moved
            if (!_isMoving)
            {
                // Wasn't moving
                _isMoving = true;
                OnStartMoving?.Invoke(this, EventArgs.Empty);
            }
        }
        else if (newPosition == lastPosition && !_justJumped)
        {
            // Didn't move
            if (_isMoving)
            {
                // Was moving
                _isMoving = false;
                OnStopMoving?.Invoke(this, EventArgs.Empty);
            }
        }
        else //Si estoy saltando blanqueo esta variable
            _isMoving = false;

    }

    private void HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRadius = 5f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRadius);
            foreach (Collider collider in colliderArray)
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    // If interactable object is in range
                    interactable.Interact();
                }
            }
        }
    }

    private void HandlePausing()
    {
        if (Input.GetButtonDown("Cancel")) //Cancel is the default for Escape
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                state = PlayerCharacterController.State.Paused;
                ActivateMenu(pauseMenu);
#if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.None;
#endif
#if UNITY_STANDALONE_WIN
                Cursor.lockState = CursorLockMode.Confined;
#endif
                Cursor.visible = true;
            }
            else
            {
                state = PlayerCharacterController.State.Normal;
                DeactivateMenu(pauseMenu);
#if UNITY_EDITOR
                //Cursor.lockState = CursorLockMode.Locked;
#endif
#if UNITY_STANDALONE_WIN
                Cursor.lockState = CursorLockMode.Confined;
#endif
                Cursor.visible = true;
            }
        }
    }

    private void HandleShooting()
    {
        if (_playerStats.weaponSlot.currentWeapon != null)
        {
            if (Input.GetButtonDown("Reload"))
            {
                Reload();
            }

            if (_playerStats.weaponSlot.currentWeapon.currentAmmo == 0 && 
                _playerStats.weaponSlot.currentWeapon.totalCurrentAmmo != 0)
            {
                Reload();
            }

            //Si aprieto el click, y pasaron al menos .22 y no esta recargando... DISPARO
            //Sino, aumento el timer
            if (Input.GetButton("Fire1") && _shootTimer <= 0 && !_isReloading)
            {
                if (TryShootAmmo())
                {
                    Shoot();
                    _shootTimer = _playerStats.weaponSlot.currentWeapon.shootCD;
                }
            }
            else
            {
                _shootTimer -= Time.deltaTime;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                isRecoiling = false;
            }
        }
    }

    public void HandleWeaponChange()
    {
        if (Input.GetButtonDown("Slot1"))
        {
            WeaponChange(0);
        }

        if (Input.GetButtonDown("Slot2"))
        {
            WeaponChange(1);
        }

        /*
        if (Input.GetButtonDown("Slot3"))
        {
            WeaponChange(2);
        }

        if (Input.GetButtonDown("Slot4"))
        {
            WeaponChange(3);
        }

        if (Input.GetButtonDown("Slot5"))
        {
            WeaponChange(4);
        }
        */
    }

    #endregion

    #region Shooting Methods

    private void Shoot()
    {
        {
            if (_playerStats.weaponSlot.currentWeapon != null)
            {
                _playerStats.weaponSlot.currentWeapon.Shoot();
                _playerStats.weaponSlot.currentWeapon.currentAmmo--;
                _playerCameraAnimator.SetTrigger("isShooting");
            }
        }
    }

    private void Reload()
    {
        _isReloading = true;
        _playerCameraAnimator.SetTrigger("isReloading");
    }

    public void FinishReload()
    {
        _isReloading = false;
    }

    public bool TryShootAmmo()
    {
        if (_playerStats.weaponSlot.currentWeapon != null)
        {
            if (_playerStats.weaponSlot.currentWeapon.currentAmmo > 0)
            {
                return true;
            }
            else
            {
                _myPlayerSound.PlaySound(PlayerSound.Actions.Empty, _otherTag);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void WeaponChange(int weaponIndex)
    {
        _playerStats.weaponSlot.WeaponChange(weaponIndex);
    }

    #endregion
    #region Jump Methods

    private void ResetGravityEffect()
    {
        _characterVelocityY = 0f;
    }

    private bool TestInputJump()
    {
        return Input.GetButtonDown("Jump");
    }

    #endregion
    #region PlayerStats Methods

    //Method Shoot() also calls PlayerStats to substract ammo.

    /*public void Damage(int damageAmount)
    {
        _playerStats.Damage(damageAmount);
    }*/

    public void HealHp(int healAmount)
    {
        _playerStats.HealHp(healAmount);
    }
    public void HealArmor(int healAmount)
    {
        _playerStats.HealArmor(healAmount);
    }

    #endregion
    #region Crumbling Platform Vibration Methods

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("CrumblingPlatform"))
            StartCoroutine(_myVibrationCoroutine);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CrumblingPlatform"))
            StopCoroutine(_myVibrationCoroutine);
    }

    IEnumerator VibrationEffect()
    {
        bool _goingRight = false;
        while (true)
        {
            if (_goingRight)
                transform.position += new Vector3(-0.1f, 0f, 0f);
            else
                transform.position += new Vector3(0.1f, 0f, 0f);

            _goingRight = !_goingRight;

            yield return new WaitForSeconds(.05f);
        }
    }

    #endregion
    #region UI Related Input Methods
    //Most of the code is on HandlePausing called by Update

    public void DeactivateMenuFromButton()
    {
        DeactivateMenu(pauseMenu);
    }

    #region Here lay the remains of what once used to be the class PauseMenu, good night, sweet prince

    private bool isPaused;
    public string menuMain;
    public string[] sceneToLoad; //agregar escena 0 y main

    public void ActivateMenu(GameObject pauseMenu)
    {
        state = State.Paused;
        isPaused = true;

        Time.timeScale = 0;
        AudioListener.pause = true;

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif

#if UNITY_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Confined;
#endif
        Cursor.visible = true;

        pauseMenu.SetActive(true);
    }
    public void DeactivateMenu(GameObject pauseMenu)
    {
        state = State.Normal;
        Time.timeScale = 1;
        AudioListener.pause = false;
        //AudioSource.ignoreListenerPause=true; opcion para evitar la pausa de los sonidos
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void RestartGame()
    {
        state = State.Normal;
        Time.timeScale = 1;
        SceneLoader._myInstance.RestartGame(sceneToLoad);
    }

    public void ActivateDeathMenu(GameObject deathMenu)
    {
        state = State.Dead;
        Time.timeScale = 0;
        deathMenu.SetActive(true);
    }

    public void ActivateWinMenu(GameObject winMenu)
    {
        state = State.Paused;
        AudioListener.pause = true;

#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
#if UNITY_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Confined;
#endif
        Cursor.visible = true;
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitToTitle()
    {
        Time.timeScale = 1;
        SceneLoader._myInstance.ReturnToTitle(menuMain);
    }

    public void ExitToWindows()
    {
        Application.Quit();
    }

    #endregion

    #endregion
}

