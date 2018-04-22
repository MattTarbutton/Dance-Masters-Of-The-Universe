using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComboAttackController : SkillController
{
    public SkillController nextAttack;
    public float maxTimeToCombo;

    private ParticleSystem particleEffect;
    private float comboTimeLeft;

    private event EventHandler ChainSkillFinished;

    // Use this for initialization
    void Start()
    {
        particleEffect = this.GetComponent<ParticleSystem>();
        particleEffect.Stop();

        nextAttack.SkillFinished += ChildSkillFinished;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (comboTimeLeft > 0)
            comboTimeLeft -= Time.deltaTime;
    }

    public override void Activate(Vector2 orientation)
    {
        if (comboTimeLeft > 0)
        {
            nextAttack.Activate(orientation);
        }
        else
        {
            base.Activate(orientation);
            comboTimeLeft = maxTimeToCombo;
            particleEffect.Play();
        }
    }

    protected override void OnActivationFinished()
    {
        base.OnActivationFinished();
        
        OnSkillFinished(this, new EventArgs<Boolean>(true));
    }

    protected override void DurationReached()
    {
        base.DurationReached();
    }

    private void ChildSkillFinished(object sender, EventArgs e)
    {
        comboTimeLeft = maxTimeToCombo;
        OnSkillFinished(this, null);
    }
}
