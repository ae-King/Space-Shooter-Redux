using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private float _explosionTime = 2.633f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, _explosionTime);
    }
}
