using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingParticlesEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodParticles;
    [SerializeField] private Transform _bleedingPoint;

    public void SpawnBlood()
    {
        var particles = Instantiate(_bloodParticles, _bleedingPoint.position, Quaternion.identity);
        particles.Play();
    }
}
