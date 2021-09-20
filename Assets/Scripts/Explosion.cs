using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _explosionAudio;
    private float _explosionTime = 2.633f;

    // Start is called before the first frame update
    void Start()
    {
        _explosionAudio = GetComponent<AudioSource>();

        _explosionAudio.Play();

        Destroy(this.gameObject, _explosionTime);
    }
}
