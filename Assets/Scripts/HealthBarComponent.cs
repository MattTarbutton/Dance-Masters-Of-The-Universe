using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarComponent : MonoBehaviour
{
    public HealthComponent playerHealth;
    public Slider healthSlider;

	// Use this for initialization
	void Start ()
    {
        healthSlider = this.GetComponent<Slider>();
        playerHealth.OnHealthChanged += UpdateHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void UpdateHealth(object sender, System.EventArgs e)
    {
        healthSlider.value = ((float)playerHealth.GetHealth() / (float)playerHealth.maxHealth);
    }
}
