using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour
{
    public SkillController skill;
    private Image cooldownImage;

	// Use this for initialization
	void Start ()
    {
        cooldownImage = GetComponent<Image>();
        skill.CooldownTimeChanged += UpdateFillValue;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void UpdateFillValue(object sender, System.EventArgs e)
    {
        cooldownImage.fillAmount = skill.GetCooldownPercentElapsed();
    }
}
