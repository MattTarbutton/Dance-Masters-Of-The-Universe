using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleAnimation : MonoBehaviour
{
    ActorController controller;
    public bool swap;
    public float angle;
    private int timeMultiplier;
    private float posOffset;
    private Vector3 startingScale;

    // Use this for initialization
    void Start()
    {
        controller = GetComponentInParent<ActorController>();

        timeMultiplier = 15;
        posOffset = 0.03f;
        startingScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.state == ActorController.ActorState.Moving)// moving)
        {
            if (swap)
            {
                //* Bounce forward
                int mult = controller.rb.velocity.x > 0.0f ? 1 : -1;
                this.transform.parent.transform.rotation = Quaternion.Euler(0, 0, mult * angle * Mathf.Sin(Time.time * timeMultiplier));
                this.transform.localPosition = new Vector3(0, 0.05f * Mathf.Cos(Time.time * timeMultiplier));
                this.transform.localScale = startingScale + new Vector3(0.1f * Mathf.Cos(Time.time * 2 * timeMultiplier), 0.1f * Mathf.Sin(Time.time * 2 * timeMultiplier), 0);
                //*/
            }
            else
            {
                //* Crazy up and down bounce
                int mult = controller.rb.velocity.x > 0.0f ? 1 : -1;
                this.transform.parent.transform.rotation = Quaternion.Euler(0, 0, mult * angle * Mathf.Sin(Time.time * timeMultiplier));
                this.transform.localPosition = new Vector3(0, posOffset + 0.5f * posOffset * Mathf.Sin(Time.time * 2 * timeMultiplier));
                this.transform.localScale = startingScale + new Vector3(0.1f * Mathf.Cos(Time.time * 2 * timeMultiplier), 0.1f * Mathf.Sin(Time.time * 2 * timeMultiplier), 0);
                //*/
            }
        }
        else if (this.transform.parent.transform.rotation.z != 0.01f || this.transform.localPosition.y > 0.01f || this.transform.localScale.y < 0.95f || this.transform.localScale.y > 1.05f)
        {
            this.transform.parent.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(this.transform.parent.transform.rotation.z - .01f * Time.deltaTime, 0, 90));
            this.transform.localPosition = new Vector3(0, Mathf.Clamp(this.transform.localPosition.y - 1f * Time.deltaTime, 0, 99));
            this.transform.localScale = startingScale;
        }
    }
}
