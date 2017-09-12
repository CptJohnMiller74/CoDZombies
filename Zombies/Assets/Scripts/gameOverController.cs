using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameOverController : MonoBehaviour {

    public Image gameOverImage;
    public Text gameOverText;
    public PlayerHealth playerHealth;

	void Start () {
        gameOverText.color = Color.clear;
        gameOverText.text = "";
	}
	
	void Update () {
		
        if (playerHealth.getIsDead())
        {
            gameOverImage.color = Color.Lerp(gameOverImage.color, Color.black, 2f * Time.deltaTime);
            gameOverText.color = Color.white;
            gameOverText.text = "Press E to restart, \npress Esc to quit";
        }

        if (playerHealth.getIsDead() && Input.GetKeyDown(KeyCode.E))
        {
            gameOverImage.color = Color.clear;
            gameOverText.color = Color.clear;
            Application.LoadLevel(Application.loadedLevel);
        }

        else if (playerHealth.getIsDead() && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
