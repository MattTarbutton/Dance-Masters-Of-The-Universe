using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SkillController
{
    public float velocity;
    public float verticalOffset;
    private ParticleSystem particleEffect;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        particleEffect = this.GetComponent<ParticleSystem>();
        particleEffect.Stop();
        sprite = this.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Activate(Vector2 orientation)
    {
        base.Activate(orientation);

        particleEffect.Play();
        sprite.enabled = true;
        this.transform.localPosition = new Vector3(0, verticalOffset);
        rb.velocity = orientation * velocity;
    }

    protected override void OnActivationFinished()
    {
        base.OnActivationFinished();

        OnSkillFinished(this, null);
    }

    protected override void DurationReached()
    {
        base.DurationReached();

        sprite.enabled = false;
        particleEffect.Stop();
    }
}
