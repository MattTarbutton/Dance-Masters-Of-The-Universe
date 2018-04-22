using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ActorController
{
    public SkillController skill;
    public float maxVelocity;
    public float directionTimeMax;
    public GameObject target;
    public float attackRange;
    public float attackWidthRange;
    public float attackCooldown;
    //protected float radiusToCheck;
    //public float maxTimeToNextCheck;

    private float directionTime;
    private float attackTime;
    //private float timeToNextCheck;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        rb = this.GetComponent<Rigidbody2D>();
        state = ActorState.Idle;
        target = GameObject.FindWithTag("Player");
        directionTime = directionTimeMax;

        // Get the material from the sprite object so we can flash when taking damage.
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.tag == "ActorSprite")
            {
                // The actual renderer is on the child of the actor sprite object
                sprite = child.GetComponentInChildren<SpriteRenderer>();
            }
        }

        skill.SkillFinished += SkillFinished;

        StartCoroutine(FadeIn(directionTimeMax));
    }

    public void Update()
    {
        //if (timeToNextCheck <= 0)
        //{
        //    timeToNextCheck = maxTimeToNextCheck;
        //}
        //else
        //{
        //    timeToNextCheck -= Time.deltaTime;
        //}

        if (state == ActorState.Idle || state == ActorState.Moving)
        {
            if (directionTime <= 0)
            {
                Vector3 distance = this.transform.position - target.transform.position;

                if (((Mathf.Abs(distance.x) < attackRange && Mathf.Abs(distance.y) < attackWidthRange) || (Mathf.Abs(distance.y) < attackRange && Mathf.Abs(distance.x) < attackWidthRange)) && attackTime <= 0)
                {
                    //attack
                    Vector3 distanceVector = this.transform.position - target.transform.position;
                    if (Mathf.Abs(distanceVector.x) > Mathf.Abs(distanceVector.y))
                    {
                        orientation = new Vector2(distanceVector.x > 0 ? -1 : 1, 0);
                    }
                    else
                    {
                        orientation = new Vector2(0, distanceVector.y > 0 ? -1 : 1);
                    }
                    rb.velocity = new Vector2(0, 0);
                    attackTime = attackCooldown;
                    state = ActorState.Attacking;
                    skill.Activate(orientation);
                    StartCoroutine(Flash(skill.activationTime, 1, Color.yellow));
                    directionTime = directionTimeMax;
                }
                else
                {
                    GetNewVelocity();

                    if (rb.velocity.magnitude > 0.01f)
                    {
                        state = ActorState.Moving;
                    }
                    else
                    {
                        state = ActorState.Idle;
                    }

                    if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
                    {
                        orientation = new Vector2(rb.velocity.x > 0 ? 1 : -1, 0);
                    }
                    else if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x))
                    {
                        orientation = new Vector2(0, rb.velocity.y > 0 ? 1 : -1);
                    }
                }
            }
            else
            {
                directionTime -= Time.deltaTime;
                attackTime -= Time.deltaTime;
            }
        }
    }

    public override void OnTakeDamage(Vector3 damagerPosition, bool activateHitStun)
    {
        if (activateHitStun)
        {
            state = ActorState.HitStun;
            this.GetComponent<HealthComponent>().SetInvulnerable(0.4f);
            Vector3 distance = damagerPosition - this.transform.position;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                rb.velocity = new Vector2(distance.x > 0 ? -maxVelocity / 4f : maxVelocity / 4f, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, distance.y > 0 ? -maxVelocity / 4f : maxVelocity / 4f);
            }

            directionTime = 0;
            StartCoroutine(HitStunTimer(0.5f));
            StartCoroutine(Flash(0.5f, 3, Color.white));
        }
        else
        {
            StartCoroutine(Flash(0.1f, 1, Color.gray));
        }
        
    }

    public override void DestroyByDamage(Vector3 damagerPosition)
    {
        state = ActorState.HitStun;
        this.GetComponent<HealthComponent>().SetInvulnerable(0.5f);
        Vector3 distance = damagerPosition - this.transform.position;
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            rb.velocity = new Vector2(distance.x > 0 ? -maxVelocity / 4f : maxVelocity / 4f, 0);
        }
        else
        {
            rb.velocity = new Vector2(0, distance.y > 0 ? -maxVelocity / 4f : maxVelocity / 4f);
        }

        StartCoroutine(Flash(0.3f, 2, Color.white));
        StartCoroutine(FadeOut(0.5f));
        ActorKilled();
    }

    private void GetNewVelocity()
    {
        int roll = Random.Range(0, 100);
        directionTime = directionTimeMax + Random.Range(-.10f, .10f);
        if (roll < 10)
        {
            rb.velocity = new Vector2(maxVelocity, 0);
        }
        else if (roll < 20)
        {
            rb.velocity = new Vector2(-maxVelocity, 0);
        }
        else if (roll < 30)
        {
            rb.velocity = new Vector2(0, maxVelocity);
        }
        else if (roll < 40)
        {
            rb.velocity = new Vector2(0, -maxVelocity);
        }
        else if (roll < 80)
        {
            Vector2 distance = this.transform.position - target.transform.position;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                rb.velocity = new Vector2(distance.x < 0 ? maxVelocity : -maxVelocity, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, distance.y < 0 ? maxVelocity : -maxVelocity);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void SkillFinished(object sender, System.EventArgs e)
    {
        if (state == ActorState.Attacking)
            state = ActorState.Idle;
    }
}
