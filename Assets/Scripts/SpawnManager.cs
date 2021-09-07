using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    //Start Enemy Characteristics
    [SerializeField]
    GameObject _enemyContainer;
    [SerializeField]
    GameObject _enemyPrefab;
    GameObject _enemy;
    Vector3 _enemyPos;
    //End Enemy Characteristics

    //Start Powerup
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private GameObject[] _powerupList;

    GameObject _powerup;
    Vector3 _powerupPos;
    //End Powerup
    
    //Start Boundaries
    //private float _floorBound = -6f;
    private float _ceilingBound = 8f;
    private float _lwallBound = -9.25f;
    private float _rwallBound = 9.25f;
    //End Boundaries


    [SerializeField]
    GameObject _playerPrefab;
    [SerializeField]
    private bool _spawning = true;
    private float _spawnRate;


    private void Awake()
    {
        StartCoroutine(PlayerSpawn());
    }

    public void OnAsteroidDestroy()
    {
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerUpSpawn());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator PlayerSpawn()
    {
        _playerPrefab.transform.GetComponent<Player>().OnStart();
        StopCoroutine(PlayerSpawn());
        yield return null;

    }

    IEnumerator EnemySpawn()
    {
        while (_spawning == true)
        {
            _spawnRate = Random.Range(2f, 8f);
            yield return new WaitForSeconds(_spawnRate);

            var _enemyrandomBound = Random.Range(_lwallBound, _rwallBound);

            _enemyPos = new Vector3(_enemyrandomBound, _ceilingBound, 0);

            _enemy = Instantiate(_enemyPrefab, _enemyPos, Quaternion.identity); //enemy info

            _enemy.transform.parent = _enemyContainer.transform;
        }
    }

    IEnumerator PowerUpSpawn()
    {
        while (_spawning == true)
        {
            _spawnRate = Random.Range(30f, 45f);
            yield return new WaitForSeconds(_spawnRate);

            var _poweruprandomBound = Random.Range(_lwallBound, _rwallBound);

            _powerupPos = new Vector3(_poweruprandomBound, _ceilingBound, 0);
            _powerup = Instantiate(_powerupList[Random.Range(0, 3)], _powerupPos, Quaternion.identity);
            _powerup.transform.parent = _powerupContainer.transform;

        }
    }

    public void OnPlayerDeath()
    {
        _spawning = false;
        Destroy(_enemy);
        Destroy(_powerup);
        StopAllCoroutines();
    }
}
