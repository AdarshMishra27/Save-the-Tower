using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPonints = 5;
    [Tooltip("Add amount to maxHitPoints when enemy dies")]
    [SerializeField] int difficultyRamp = 1;
    int curHitPoints = 0;
    Enemy enemy;
    private void Start() {
        enemy = GetComponent<Enemy>();
    }
    void OnEnable()
    {
        curHitPoints = maxHitPonints;
    }

    void OnParticleCollision(GameObject other) {
        processHit();    
    }

    private void processHit()
    {
        curHitPoints--;

        if(curHitPoints<=0) {
            enemy.RewardGold();
            maxHitPonints+=difficultyRamp;
            gameObject.SetActive(false);
        }
    }
}
