using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ExplodingEnemyAI : EnemyAI
{
    [Header("--- Exploding Enemy Components ---")]
    [SerializeField] Collider explodeTrigger;
    [SerializeField] GameObject explosion;

    [Header("--- Exploding Enemy Stats ---")]
    [Range(3, 10)] [SerializeField] int explodeDmg;
    [Range(3, 5)] [SerializeField] int explodeTimer;
    [Range(4, 10)] [SerializeField] int sightDistance; // for rotating because melee stopping distance is too small for the enemy to track the player with

    [Header("----- Audio Clips -----")]
    [SerializeField] AudioSource eEnemyAudSource;
    [SerializeField] AudioClip eEnemyAttackSound;
    [Range(0, 1)][SerializeField] float attackSoundVol;

    bool isExploding;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (canSee)
        {
            StopCoroutine(roam());

            // enemy should rotate to face player
            if (getAgent().remainingDistance < sightDistance)
                faceTarget();

            if (explodeTrigger.GetComponent<ExplodeTrigger>().getInRange() && !isExploding)
            {
                // enemy doesnt really need an explodeFOV
                // as long as it can see the player and player was within range
                StartCoroutine(explode());
            }
        }

        return canSee;
    }

    IEnumerator explode()
    {
        isExploding = true;
        StartCoroutine(flashWarning());
        yield return new WaitForSeconds(explodeTimer);

        if(explodeTrigger.GetComponent<ExplodeTrigger>().getDmg() != null)
            explodeTrigger.GetComponent<ExplodeTrigger>().getDmg().takeDamage(explodeDmg);

        Instantiate(explosion, transform.position, transform.rotation);
        eEnemyAudSource.PlayOneShot(eEnemyAttackSound, attackSoundVol);
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<IDamage>().takeDamage(getHP());
    }

    IEnumerator flashWarning()
    {
        Color ogColor = getModel().material.color;

        while (isExploding)
        {
            getModel().material.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            getModel().material.color = ogColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
