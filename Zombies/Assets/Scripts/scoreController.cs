using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scoreController : MonoBehaviour {

    public Text scoreText;

    private int score;

	void Start () {
        score = 0;
        scoreText.text = "" + score;
	}

    private void Update()
    {
        scoreText.text = "" + score;
    }

    public int getScore()
    {
        return score;
    }

    public void addScore(int x)
    {
        score += x;
    }

    public void spendScore(int x)
    {
        score = score - x;
    }
}
