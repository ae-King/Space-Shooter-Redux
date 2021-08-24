using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private float _enemySpeed = 7f;
    Vector3 _enemyPos;

    private float _floorBound = -6f;
    private float _ceilingBound = 8f;
    private float _lwallBound = -9.25f;
    private float _rwallBound = 9.25f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); //cache player reference
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
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

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.OnDamage();
                _player.OnKill(1);

            }

            Destroy(this.gameObject);

        }
        if (other.tag == "Laser")
        {
            Destroy(this.gameObject);
            if (_player != null)
            {
                _player.OnKill(1);
            }
            Destroy(other.gameObject);
        }
    }
}
