using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    [Header("--- Ranged Enemy Components ---")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    [Header("RANGED ENEMY DAMAGE IN BULLET PREFAB")]
    [Header("--- Ranged Enemy Stats ---")]
    [Range(0.1f, 1)] [SerializeField] float shootRate;
    [Range(10, 45)] [SerializeField] int shootFOV; // fov for shooting/attacking
    [Range(5, 15)] [SerializeField] int stunTime;

    [Header("----- Audio Clips -----")]
    [SerializeField] AudioClip rEnemyAttackSound;
    [SerializeField] AudioClip rebootSFX;

    bool isShooting;
    bool isShutdown;

    // ranged enemy doesn't need the movement based code since they'll be stationary
    protected override void Update()
    {
        if (isShutdown && !getAnimator().GetBool("isShutdown"))
        {
            StopAllCoroutines();
            StartCoroutine(shutdown());
        }
        else if (playerInRange && !isShutdown && !canSeePlayer()) { }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (canSee && !getAnimator().GetBool("isStunned") && !isShutdown)
        {
            faceTarget();

            if (angleToPlayer < shootFOV && !isShooting)
            {
                StartCoroutine(shoot());
            }
        }

        return canSee;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        // shoot sfx
        getAudSource().PlayOneShot(rEnemyAttackSound);

        getAnimator().SetTrigger("Attack");
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator stun()
    {
        getAnimator().SetBool("isStunned", true);

        yield return new WaitForSeconds(stunTime);

        getAudSource().PlayOneShot(rebootSFX, getAudSource().volume);
        getAnimator().SetBool("isStunned", false);
    }

    public override void takeDamage(int amount)
    {
        // ranged enemy shouldnt actually take damage
        if (!getAnimator().GetBool("isStunned") && !getAnimator().GetBool("isShutdown"))
        {
            getAudSource().PlayOneShot(getHurtSFX(), getAudSource().volume);
            StartCoroutine(flashStun());
            StartCoroutine(stun());
        }
    }

    IEnumerator shutdown()
    {
        getAnimator().SetBool("isShutdown", true);

        yield return new WaitForSeconds(1.2f);

        Destroy(gameObject);
    }

    IEnumerator flashStun()
    {
        Color ogColor = getModel().material.color;

        getModel().material.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        getModel().material.color = ogColor;
    }

    public void setShutdown(bool shutdown)
    {
        isShutdown = shutdown;
    }
}
