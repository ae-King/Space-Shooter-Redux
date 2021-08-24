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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
}
