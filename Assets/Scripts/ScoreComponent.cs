using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreComponent : MonoBehaviour
{
    public GameObject scoreManager;
    private Text score;

	// Use this for initialization
	void Start ()
    {
        scoreManager.GetComponent<GameManager>().OnScoreChanged += UpdateScore;
        score = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateScore(object sender, System.EventArgs e)
    {
        score.text = (e as EventArgs<int>).Value.ToString("00000000");
    }
}
