using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    Vector3 _laserPos;
    [SerializeField]
    private float _laserSpeed = 10f;
    [SerializeField]
    private float _ceilingBound = 8;
    [SerializeField]
    private float _floorBound = -6;

    private bool _enemyLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyLaser == false)
        {
            PlayerLaser();
        }
        else
        {
            EnemyLaser();
        }
    }

    public void IsEnemyLaser()
    {
        _enemyLaser = true;
    }

    private void PlayerLaser()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        _laserPos = transform.position;

        if (_laserPos.y >= _ceilingBound)
        {
            if (transform.parent.name != "Laser Container") //check for name of parent gameObject, if it's not this name then delete parent
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void EnemyLaser()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        _laserPos = transform.position;

        if (_laserPos.y <= _floorBound)
        {
            if (transform.parent.name != "Laser Container") //check for name of parent gameObject, if it's not this name then delete parent
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }


}
