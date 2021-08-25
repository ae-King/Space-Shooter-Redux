using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Start Player Position Container
    Vector3 _playerPos;
    Vector3 _playerBound;
    Vector3 _playerDirection;
    //End Player Position Container

    //Start Player Boundaries
    private float _lwallBound = -11.28f;
    private float _rwallBound = 11.28f;
    private float _ceilingBound = 0;
    private float _floorBound = -4;
    //End Player Boundaries

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    //Start Player Statistics
    [SerializeField]
    private int _playerMaxHealth = 3;
    [SerializeField]
    private int _currentPlayerHealth;
    [SerializeField]
    private int _maxShieldHealth = 3;
    [SerializeField]
    private int _currentShieldHealth;
    [SerializeField]
    private float _playerDefaultSpeed = 7f;
    [SerializeField]
    private float _playerCurrentSpeed;
    [SerializeField]
    private int _enemiesDestroyed;
    //End Player Statistics

    //Start Weapon Statistics
    private GameObject _laserContainer;
    Vector3 _laserPos;
    private GameObject _laser;

    [SerializeField]
    private GameObject[] _laserType;
    /*
     Laser Types
     * 0 = Basic
     * 1 = Tri
     */

    [SerializeField]
    private bool _laserEnabled;
    [SerializeField]
    private int _laserMaxAmmo = 30;
    [SerializeField]
    private int _currentLaserAmmo;
    //End Weapon Statistics

    //Start Powerup
    [SerializeField]
    private bool _triLaserEnabled;
    [SerializeField]
    private bool _speedBoostEnabled;
    [SerializeField]
    private bool _shieldBoostEnabled;
    [SerializeField]
    private GameObject _shieldVisual;
    //End Powerup

    // Start is called before the first frame update
    void Start()
    {

        _laserContainer = GameObject.Find("Laser Container"); //get laser container
        if (_laserContainer == null)
        {
            Debug.LogError("Laser Container is null");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        OnSpawn();

    }

    public void OnStart()
    {
        _playerPos = new Vector3(0, -0.5f, 0); //set position
        var _player = Instantiate(this.gameObject, _playerPos, Quaternion.identity); //create clone
        _player.name = "Player";    //rename from "Player(Clone)" to "Player"
    }
    void OnSpawn()
    {
        _laserEnabled = true;

        _currentLaserAmmo = _laserMaxAmmo;

        _currentPlayerHealth = _playerMaxHealth;

        _triLaserEnabled = false;

        _shieldBoostEnabled = false;

        _speedBoostEnabled = false;

        _shieldVisual.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
    }
    
    public void playerDirection(InputAction.CallbackContext context)
    {
        Vector2 _playerInput = context.ReadValue<Vector2>();
        _playerDirection = new Vector3(_playerInput.x, _playerInput.y, 0.0f);
    }
    void OnMove()
    {
        OnBound();
        if (_speedBoostEnabled == true)
        {
            _playerCurrentSpeed = _playerDefaultSpeed + 4;
        }
        else
        {
            _playerCurrentSpeed = _playerDefaultSpeed;
        }
        transform.Translate(_playerDirection * _playerCurrentSpeed * Time.deltaTime);
    }
    void OnBound()
    {
        _playerPos = transform.position;

        if (_playerPos.x <= _lwallBound)
        {
            _playerPos.x = _rwallBound;
        }
        else if (_playerPos.x >= _rwallBound)
        {
            _playerPos.x = _lwallBound;
        }

        float _ymovementBound = Mathf.Clamp(_playerPos.y, _floorBound, _ceilingBound);
        _playerBound = new Vector3(_playerPos.x, _ymovementBound, 0);

        transform.position = _playerBound;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        _playerPos = transform.position;
        _laserPos = new Vector3(0, 0.78f, 0);

        if (context.performed && _triLaserEnabled) //tri laser
        {
            _laserEnabled = false;
            _laser = Instantiate(_laserType[1], _playerPos + _laserPos, Quaternion.identity);
            _laser.transform.parent = _laserContainer.transform; //contain laser


        }
        else if (context.performed && _laserEnabled && _currentLaserAmmo > 0) //basic laser
        {
            _laser = Instantiate(_laserType[0], _playerPos + _laserPos, Quaternion.identity);
            _laser.transform.parent = _laserContainer.transform; //contain laser

            _currentLaserAmmo -= 1;
            _laserEnabled = false;
            StartCoroutine(OnCooldown());

            if (_currentLaserAmmo == 0)
            {
                _laserEnabled = false;
                StartCoroutine(OnReload());
            }
        }

    }

    public void OnKill(int _enemyValue) //parameter received from enemy to determine amount of points
    {
        _enemiesDestroyed += _enemyValue;
        _uiManager.OnScoreUpdate(_enemiesDestroyed);
    }

    public void OnDamage()
    {
        if (_shieldBoostEnabled == true)
        {
            _currentShieldHealth -= 1;
            StartCoroutine(OnShieldDamage());
            if (_currentShieldHealth <= 0)
            {
                StopCoroutine(OnShieldEnabled());
                _shieldBoostEnabled = false;
            }
        }
        else
        {
            _currentPlayerHealth -= 1;
            _uiManager.OnLivesUpdate(_currentPlayerHealth); //update life display

            if (_currentPlayerHealth <= 0)
            {
                OnDeath();
            }
            else if (_currentPlayerHealth > 0 && _currentPlayerHealth < 10)
            {
                StartCoroutine(OnRegen());
            }
        }
    }

    IEnumerator OnRegen()
    {
        yield return new WaitForSeconds(30f);
        _currentPlayerHealth += 1;
        _uiManager.OnLivesUpdate(_currentPlayerHealth); //update life display
    }

    void OnDeath()
    {
        _spawnManager.OnPlayerDeath();
        _uiManager.OnPlayerDeath();
        Destroy(this.gameObject);
    }

    //Basic Laser - Start
    IEnumerator OnCooldown()
    {
        yield return new WaitForSeconds(0.17f);
        _laserEnabled = true;
    }
    IEnumerator OnReload()
    {
        yield return new WaitForSeconds(1f);
        _currentLaserAmmo = _laserMaxAmmo;
        _laserEnabled = true;
    }
    //Basic Laser - End


    //Powerup - Start


    //Tri Laser - Start
    public void OnTriEnable()
    {
        _triLaserEnabled = true;
        StartCoroutine(OnTriEnabled());
    }
    IEnumerator OnTriEnabled()
    {
        //disable after 15 seconds
        _laserEnabled = false;
        StartCoroutine(OnTriCooldown());
        yield return new WaitForSeconds(15f);
        StopCoroutine(OnTriCooldown());
        _triLaserEnabled = false;
        _laserEnabled = true;
        StopCoroutine(OnTriEnabled());
    }
    IEnumerator OnTriCooldown()
    {
        yield return new WaitForSeconds(0.17f);
        _triLaserEnabled = true;
    }
    //Tri Laser - End


    //Speed - Start
    public void OnSpeedEnable()
    {
        _speedBoostEnabled = true;
        StartCoroutine(OnSpeedEnabled());
    }
    IEnumerator OnSpeedEnabled()
    {
        //disable after 15 seconds
        yield return new WaitForSeconds(15f);
        _speedBoostEnabled = false;
        StopCoroutine(OnSpeedEnabled());
    }
    //Speed - End


    //Shield - Start
    public void OnShieldEnable()
    {
        _shieldBoostEnabled = true;
        StartCoroutine(OnShieldEnabled());
    }
    IEnumerator OnShieldEnabled()
    {
        yield return null;
        _shieldVisual.SetActive(true);
        _currentShieldHealth = _maxShieldHealth;
    }
    IEnumerator OnShieldDamage()
    {
        var _randomFlicker = Random.Range(0.02f, 0.1f);
        yield return new WaitForSeconds(_randomFlicker);
        _shieldVisual.SetActive(false);
        yield return new WaitForSeconds(_randomFlicker);
        _shieldVisual.SetActive(true);
        yield return new WaitForSeconds(_randomFlicker);
        _shieldVisual.SetActive(false);
        yield return new WaitForSeconds(_randomFlicker);
        _shieldVisual.SetActive(true);
        StopCoroutine(OnShieldDamage());
    }
    //Shield - End


    //End Powerup - End



}
