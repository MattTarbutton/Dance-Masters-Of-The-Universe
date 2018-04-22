using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public float duration;
    public float activationTime;
    public float cooldown;
    private bool activating;
    private bool active;
    private bool coolingDown;
    private float elapsedDurationTime;
    private float elapsedActivationTime;
    private float cooldownRemainingTime;

    protected event EventHandler ActivationFinished;
    public event EventHandler SkillFinished;

    public event EventHandler CooldownTimeChanged;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (elapsedDurationTime < duration && active)
        {
            elapsedDurationTime += Time.deltaTime;

            if (elapsedDurationTime >= duration)
                DurationReached();
        }

        if (elapsedActivationTime < activationTime && activating)
        {
            elapsedActivationTime += Time.deltaTime;

            if (elapsedActivationTime >= activationTime)
            {
                OnActivationFinished();
            }
        }

        if (cooldownRemainingTime > 0 && coolingDown)
        {
            cooldownRemainingTime -= Time.deltaTime;
            OnCooldownChanged(this, null);

            coolingDown = cooldownRemainingTime > 0;
        }
    }

    public bool IsAvailable()
    {
        return !active && !activating && !coolingDown;
    }

    public virtual void Activate(Vector2 orientation)
    {
        elapsedActivationTime = 0;
        elapsedDurationTime = 0;
        activating = true;
    }

    protected virtual void Deactivate()
    {

    }

    public float GetCooldownPercentElapsed()
    {
        return cooldownRemainingTime / cooldown;
    }

    public void InterruptSkill()
    {
        if (active)
        {
            elapsedDurationTime = duration;
            Deactivate();
        }
        else if (activating)
        {
            activating = false;
            Deactivate();
        }
    }

    protected virtual void DurationReached()
    {
        active = false;
    }

    protected virtual void OnActivationFinished()
    {
        if (ActivationFinished != null)
            ActivationFinished(this, null);

        activating = false;
        active = true;
    }

    protected virtual void OnSkillFinished(object sender, EventArgs e)
    {
        if (SkillFinished != null)
            SkillFinished(sender, e);

        coolingDown = true;
        cooldownRemainingTime = cooldown;
    }

    protected virtual void OnCooldownChanged(object sender, EventArgs e)
    {
        if (CooldownTimeChanged != null)
            CooldownTimeChanged(sender, e);
    }

}
