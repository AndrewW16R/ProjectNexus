using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    public float enemyMaxHealth;
    public float enemyCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        if (enemyMaxHealth == 0)
        {
            enemyMaxHealth = 100;
        }
        if (enemyCurrentHealth == 0)
        {
            enemyCurrentHealth = enemyMaxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
