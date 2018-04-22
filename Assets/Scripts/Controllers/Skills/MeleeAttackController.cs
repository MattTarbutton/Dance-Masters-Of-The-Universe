using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : SkillController
{
    public int damage;
    private ParticleSystem particleEffect;
    private BoxCollider2D hurtbox;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        particleEffect = this.GetComponent<ParticleSystem>();
        particleEffect.Stop();
        hurtbox = this.GetComponent<BoxCollider2D>();
        hurtbox.enabled = false;
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(this.tag))
        {
            HealthComponent health = collision.GetComponent<HealthComponent>();
            ActorController actor = collision.GetComponent<ActorController>();
            if (health != null && actor != null && !health.invulnerable)
            {
                health.Damage(this.transform.parent.gameObject, damage, true);
                audioSource.Play();
            }
        }
    }

    public override void Activate(Vector2 orientation)
    {
        base.Activate(orientation);

        if (orientation.x > .9f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (orientation.x < -.9f)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (orientation.y > .9f)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else
            transform.rotation = Quaternion.Euler(0, 0, 270);
        
    }

    protected override void OnActivationFinished()
    {
        base.OnActivationFinished();

        hurtbox.enabled = true;
        particleEffect.Play();
    }

    protected override void DurationReached()
    {
        base.DurationReached();

        hurtbox.enabled = false;
        OnSkillFinished(this, null);
    }
}
