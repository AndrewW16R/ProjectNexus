using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySprite : MonoBehaviour
{

    public AIPath aiPath; //component added to enemey gameobject to control it

    Animator animator;
    EnemySight enemySight;
    EnemyAI enemyAI;

    private string currentAnim;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        enemySight = GetComponentInParent<EnemySight>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        if (enemySight.playerInSight == true && enemyAI.isMoving == true)
        {
            SetAnimationState("Enemy_Dummy_Walk");
        }
        else
        {
            SetAnimationState("Enemy_Dummy_Idle");
        }
    }

    void SetAnimationState(string newState)
    {
        if (newState == currentAnim)
        {
            return;
        }
        else
        {
            currentAnim = newState;
            animator.Play(newState);
        }

    }
}
