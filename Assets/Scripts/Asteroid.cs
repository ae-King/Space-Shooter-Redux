using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _moveSpeed = 1f;
    private float _rotateSpeed = 6f;
    private Player _player;
    private CircleCollider2D _collider;
    private Animator _explode;
    [SerializeField] private GameObject _explosion;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); //cache player reference
        _collider = GetComponent<CircleCollider2D>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);
            _spawnManager.OnAsteroidDestroy();
        }

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.OnDamage();
            }

            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);
            Destroy(_explosion);
        }
    }
}
