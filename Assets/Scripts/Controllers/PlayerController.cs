using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController
{
    public float maxSpeed;
    public SkillController[] skills;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        rb = this.GetComponent<Rigidbody2D>();
        orientation = new Vector2(1, 0);
        state = ActorState.Idle;
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SkillFinished += SkillFinished;
        }

        // Get the material from the sprite object so we can flash when taking damage.
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.tag == "ActorSprite")
            {
                // The actual renderer is on the child of the actor sprite object
                sprite = child.GetComponentInChildren<SpriteRenderer>();
            }
        }
    }

    private void FixedUpdate()
    {
        if (state == ActorState.Idle || state == ActorState.Moving)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            rb.velocity = new Vector2(moveHorizontal * maxSpeed, moveVertical * maxSpeed);

            //moving = rb.velocity.magnitude > 0.01f;
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

    public override void OnTakeDamage(Vector3 damagerPosition, bool activateHitStun)
    {
        if (activateHitStun)
        {
            state = ActorState.HitStun;
            this.GetComponent<HealthComponent>().SetInvulnerable(1.0f);
            Vector3 distance = damagerPosition - this.transform.position;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                rb.velocity = new Vector2(distance.x > 0 ? -maxSpeed / 4f : maxSpeed / 4f, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, distance.y > 0 ? -maxSpeed / 4f : maxSpeed / 4f);
            }

            StartCoroutine(HitStunTimer(0.25f));
            StartCoroutine(Flash(1.0f, 6, Color.white));
        }
        else
        {
            StartCoroutine(Flash(0.1f, 1, Color.gray));
        }
    }

    public override void DestroyByDamage(Vector3 damagerPosition)
    {
        state = ActorState.Dead;
        ActorKilled();

        StartCoroutine(FadeOut(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ActorState.Idle || state == ActorState.Moving)
        {
            if (Input.GetButtonDown("Fire1") && skills[0].IsAvailable())
            {
                //
                skills[0].Activate(orientation);
                state = ActorState.Attacking;
                rb.velocity = new Vector2(0, 0);
            }
            else if (Input.GetButtonDown("Fire2") && skills[1].IsAvailable())
            {
                //
                skills[1].Activate(orientation);
                state = ActorState.Attacking;
                rb.velocity = new Vector2(0, 0);
            }
            else if (Input.GetButtonDown("Fire3") && skills[2].IsAvailable())
            {
                //
                skills[2].Activate(orientation);
                state = ActorState.Attacking;
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    private void SkillFinished(object sender, EventArgs e)
    {
        if (state == ActorState.Attacking)
            state = ActorState.Idle;
    }
}
