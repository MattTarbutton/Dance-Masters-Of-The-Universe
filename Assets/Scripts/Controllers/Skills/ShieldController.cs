using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : SkillController
{
    private ParticleSystem particleEffect;

    // Use this for initialization
    void Start ()
    {
        particleEffect = this.GetComponent<ParticleSystem>();
        particleEffect.Stop();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Activate(Vector2 orientation)
    {
        base.Activate(orientation);
        
    }

    protected override void OnActivationFinished()
    {
        base.OnActivationFinished();

        this.transform.parent.GetComponent<HealthComponent>().SetInvulnerable(duration);

        OnSkillFinished(this, null);
        particleEffect.Play();
    }

    protected override void DurationReached()
    {
        base.DurationReached();

    }
}
