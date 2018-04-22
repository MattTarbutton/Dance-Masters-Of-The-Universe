using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : SkillController
{
    public float beamLength;
    public float beamWidth;
    public int beamHitCount;
    public int damagePerHit;
    private LineRenderer line;
    private bool beamActive;
    private float beamTimer;
    private float beamHitInterval;
    private Vector3 orientation;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
        line.enabled = false;
        beamHitInterval = duration / (beamHitCount - 1.5f);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (beamActive)
        {
            if (beamTimer <= 0)
            {
                beamTimer = beamHitInterval;

                //RaycastHit2D[] hits = Physics2D.LinecastAll(line.GetPosition(0), line.GetPosition(1), ~(LayerMask.GetMask("Player")));
                // Check for hits in the middle of the beam and at each edge
                int layerMask = ~(1 << this.gameObject.layer);
                if ((1 << this.gameObject.layer) == LayerMask.GetMask("PlayerAttack"))
                    layerMask = ~LayerMask.GetMask("Player");
                else if ((1 << this.gameObject.layer) == LayerMask.GetMask("EnemyAttack"))
                    layerMask = ~LayerMask.GetMask("Enemy");
                
                RaycastHit2D[] hitsA = Physics2D.LinecastAll(line.GetPosition(0) + orientation * beamWidth / 2.0f, line.GetPosition(1) + orientation * beamWidth / 2.0f, layerMask);
                RaycastHit2D[] hitsB = Physics2D.LinecastAll(line.GetPosition(0), line.GetPosition(1), layerMask);
                RaycastHit2D[] hitsC = Physics2D.LinecastAll(line.GetPosition(0) - orientation * beamWidth / 2.0f, line.GetPosition(1) - orientation * beamWidth / 2.0f, layerMask);
                
                List<GameObject> hits = new List<GameObject>();

                for (int i = 0; i < hitsA.Length; i++)
                {
                    HealthComponent h = hitsA[i].collider.GetComponent<HealthComponent>();
                    if (h != null && !h.invulnerable)
                    {
                        h.Damage(this.transform.parent.gameObject, damagePerHit, false);
                        hits.Add(h.gameObject);
                        audioSource.Play();
                    }
                }

                for (int i = 0; i < hitsB.Length; i++)
                {
                    HealthComponent h = hitsB[i].collider.GetComponent<HealthComponent>();
                    if (h != null && !h.invulnerable && !hits.Contains(h.gameObject))
                    {
                        h.Damage(this.transform.parent.gameObject, damagePerHit, false);
                        hits.Add(h.gameObject);
                        audioSource.Play();
                    }
                }

                for (int i = 0; i < hitsC.Length; i++)
                {
                    HealthComponent h = hitsC[i].collider.GetComponent<HealthComponent>();
                    if (h != null && !h.invulnerable && !hits.Contains(h.gameObject))
                    {
                        h.Damage(this.transform.parent.gameObject, damagePerHit, false);
                        audioSource.Play();
                    }
                }

            }
            else
            {
                beamTimer -= Time.deltaTime;
            }

        }
    }

    public override void Activate(Vector2 orientation)
    {
        base.Activate(orientation);

        this.orientation = orientation;
        // Set the beam material to look like it is moving away from the owner
        if (Mathf.Abs(orientation.x) >= 0.9f)
        {
            line.material.SetFloat("_SpeedU", orientation.x > 0 ? -1 : 1);
        }
        else
        {
            line.material.SetFloat("_SpeedV", orientation.y > 0 ? -1 : 1);
        }

        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, new Vector3(this.transform.position.x + orientation.x * beamLength, this.transform.position.y + orientation.y * beamLength));
    }

    protected override void OnActivationFinished()
    {
        base.OnActivationFinished();

        line.enabled = true;
        beamActive = true;
    }

    protected override void DurationReached()
    {
        base.DurationReached();
        
        line.enabled = false;
        beamActive = false;
        OnSkillFinished(this, null);
    }
}
