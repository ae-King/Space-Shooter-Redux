using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    Vector3 _powerupPos;
    [SerializeField]
    private int _powerUpID;
    [SerializeField]
    private float _powerupSpeed = 7f;

    [SerializeField]
    private float _floorBound = -6f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        _powerupPos = transform.position;

        if (_powerupPos.y <= _floorBound)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player _player = other.transform.GetComponent<Player>();

        if (other.tag == "Player")
        {
            switch (_powerUpID)
            {
                /*
                 Powerup IDs

                * 0 = Shield
                * 1 = Speed
                * 2 = Tri Laser
                
                */
                case 0:
                    _player.OnShieldEnable();
                    Destroy(this.gameObject);
                    break;
                case 1:
                    _player.OnSpeedEnable();
                    Destroy(this.gameObject);
                    break;
                case 2:
                    _player.OnTriEnable();
                    Destroy(this.gameObject);
                    break;
                default:
                    Debug.Log("Powerup doesn't exist");
                    break;

            }

        }
    }
}
