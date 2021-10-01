using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private Animator _enemyAnimation;
    private BoxCollider2D _enemyCollider;
    private float _deathanimationTime = 2.633f;
    private AudioSource _enemyExplosion;

    [SerializeField]
    private float _enemySpeed = 4f;
    Vector3 _enemyPos;
    private float _onEnemyDeathSpeed = 3.5f;

    [SerializeField] private float _fireRate = 3.0f;
    [SerializeField] private float _canFire = -1;
    [SerializeField] private GameObject _laserPref;
    private Vector3 _laserPos;

    private float _floorBound = -6f;
    private float _ceilingBound = 8f;
    private float _lwallBound = -9.25f;
    private float _rwallBound = 9.25f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); //cache player reference
        _enemyExplosion = GetComponent<AudioSource>();
        _enemyAnimation = GetComponent<Animator>();
        _enemyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        OnFire();
    }

    void OnMove()
    {
        OnBound();
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
    }

    void OnBound()
    {
        _enemyPos = transform.position;
        var _enemyrandomBound = Random.Range(_lwallBound, _rwallBound);
        if (_enemyPos.y <= _floorBound)
        {
            _enemyPos = new Vector3(_enemyrandomBound, _ceilingBound, 0);
            transform.position = _enemyPos;
        }
    }

    private void OnFire()
    {

        if (Time.time > _canFire)
        {
            _laserPos = new Vector3(0, -0.78f, 0);

            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject _enemyParentLaser = Instantiate(_laserPref, _enemyPos + _laserPos, Quaternion.identity);
            Laser[] _enemyChildLasers = _enemyParentLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < _enemyChildLasers.Length; i++)
            {
                _enemyChildLasers[i].IsEnemyLaser();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.OnDamage();
                _player.OnKill(1);
            }

            _enemyAnimation.SetTrigger("OnDestroyed");
            StartCoroutine(OnDeath());
            _enemyExplosion.Play();
            Destroy(_enemyCollider, 0.5f);
            Destroy(this.gameObject, _deathanimationTime);

        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.OnKill(1);
            }

            _enemyAnimation.SetTrigger("OnDestroyed");
            StartCoroutine(OnDeath());
            _enemyExplosion.Play();
            Destroy(_enemyCollider);
            Destroy(this.gameObject, _deathanimationTime);
        }
    }

    IEnumerator OnDeath()
    {
        while (_enemySpeed > 0)
        {
            _enemySpeed -= _onEnemyDeathSpeed * Time.deltaTime;
            yield return null;
        }
        if (_enemySpeed <= 0)
        {
            StopAllCoroutines();
        }
    }
}
