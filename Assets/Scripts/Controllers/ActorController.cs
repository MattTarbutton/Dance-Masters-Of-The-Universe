using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public enum ActorState
    {
        Idle,
        Moving,
        Attacking,
        HitStun,
        Dead
    }

    public Rigidbody2D rb;
    public ActorState state;
    public Vector2 orientation;
    protected SpriteRenderer sprite;

    public event EventHandler OnActorKilled;

    // Use this for initialization
    protected virtual void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnTakeDamage(Vector3 damagerPosition, bool activateHitStun)
    {
        
    }

    public virtual void DestroyByDamage(Vector3 damagerPosition)
    {

    }

    protected virtual void ActorKilled()
    {
        if (OnActorKilled != null)
            OnActorKilled(this, null);
    }

    protected IEnumerator HitStunTimer(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            yield return null;
        }
        state = ActorState.Idle;
    }

    protected IEnumerator Flash(float duration, int intervals, Color color)
    {
        float startTime = Time.time;
        float intervalStartTime = Time.time;
        float interval = duration / (2 * intervals + 1);
        float t = 0;
        bool increasing = true;

        sprite.material.SetColor("_FlashColor", color);

        while (Time.time < startTime + duration)
        {
            if (Time.time > intervalStartTime + interval)
            {
                intervalStartTime = Time.time;
                increasing = !increasing;
            }

            if (increasing)
            {
                t = (Time.time - intervalStartTime) / interval;
                sprite.material.SetFloat("_FlashAmount", t);
                yield return null;
            }
            else
            {
                t = (Time.time - intervalStartTime) / interval;
                sprite.material.SetFloat("_FlashAmount", 1 - t);
                yield return null;
            }
        }

        sprite.material.SetFloat("_FlashAmount", 0);
    }

    protected IEnumerator FadeIn(float duration)
    {
        float startTime = Time.time;
        float t = 0;
        while (Time.time < startTime + duration)
        {
            t = (Time.time - startTime) / duration;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
    }

    protected IEnumerator FadeOut(float duration)
    {
        float startTime = Time.time;
        float t = 0;
        while (Time.time < startTime + duration)
        {
            t = (Time.time - startTime) / duration;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}
