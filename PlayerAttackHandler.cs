using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    Animator                animator;
    bool                    canAttack;
    float                   range = .3f;
    UI_Energy               ui_Energy;
    PlayerMovementHandler   playerMovementHandler;
    public float            recoverTime = 2f;

    private void Awake ()
    {
        animator = GetComponent<Animator>();
        ui_Energy = GetComponent<UI_Energy>();
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
        canAttack = true;
    }

    private void Start ()
    {
        InvokeRepeating("IncreaseEnergyPeriodic", recoverTime, recoverTime);
    }

    public void Atack ()
    {
        if (canAttack && !ui_Energy.Empty())
        {
            ui_Energy.DecreaseEnergy (1);

            animator.SetFloat("atack", Random.Range(0.0f, 1.0f));
            animator.SetTrigger("atackSword");
            playerMovementHandler.speed /= 2;
            canAttack = false;
            StartCoroutine(Wait());

            //search for closest enemies 
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D collider2D in colliderArray)
            {
                if (collider2D.TryGetComponent<EnemyBase>(out EnemyBase enemyBase))
                {
                    enemyBase.Damage(1);
                }
            }  
        }
    }

    IEnumerator Wait ()
    {
        yield return new WaitForSeconds(.5f);
        canAttack = true;
        playerMovementHandler.ResetSpeed();
    }

    private void IncreaseEnergyPeriodic ()
    {
        if (canAttack)
            ui_Energy.IncreaseEnergy (1);
    }
}
