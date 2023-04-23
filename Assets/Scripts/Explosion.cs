using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public bool boom;
    ParticleSystem ps;

    public static Explosion instance;

    void Start()
    {
        instance = this;
        ps = GetComponent<ParticleSystem>();
    }

    public static void Explode(Vector3 position)
    {
        instance.transform.position = position;
        instance.ps.Play();
    }

    void Update()
    {
        if (boom)
        {
            boom = false;
            Explode(transform.position);
        }
    }
}
