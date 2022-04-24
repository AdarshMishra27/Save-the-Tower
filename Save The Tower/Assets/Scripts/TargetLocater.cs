using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocater : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;
    Transform target;
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTraget = null;
        float distance = Mathf.Infinity;

        foreach (Enemy e in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, e.transform.position);
            if (targetDistance < distance)
            {
                closestTraget = e.transform;
                distance = targetDistance;
            }
        }

        target = closestTraget;
    }

    private void AimWeapon()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);
        weapon.LookAt(target);

        if(targetDistance<=range) {
            Attack(true);
        } else {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
