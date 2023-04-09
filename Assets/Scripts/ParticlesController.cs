using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public ParticleSystem _bloodParticles;

    public void SpawnBlood(Damagable damagable)
    {
        var particles = Instantiate(_bloodParticles, damagable.transform.position, Quaternion.identity);
        particles.Play();
    }
}
