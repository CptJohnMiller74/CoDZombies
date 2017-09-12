using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textConfig : MonoBehaviour {

    public RectTransform bulletCount;
    public RectTransform grenadeCount;
    public Image grenadeImg;
    public Image hitmarker;
    public RectTransform wave;
    public RectTransform score;
    public RectTransform alert;
    public RectTransform gameOverText;
    public Image[] crosshairs;

    void Start () {
        bulletCount.sizeDelta = new Vector2(Screen.width / 5.25f, Screen.height / 14f);
        bulletCount.GetComponent<Text>().fontSize = Screen.width / 33;
        bulletCount.position = new Vector3(bulletCount.position.x, Screen.height / 10.5f, bulletCount.position.z);
        grenadeCount.sizeDelta = new Vector2(Screen.width / 5.25f, Screen.height / 14f);
        grenadeCount.GetComponent<Text>().fontSize = Screen.width / 33;
        wave.sizeDelta = new Vector2(Screen.width / 5.25f, Screen.height / 14f);
        wave.GetComponent<Text>().fontSize = Screen.width / 33;
        score.sizeDelta = new Vector2(Screen.width / 5.2f, Screen.height / 13f);
        score.GetComponent<Text>().fontSize = Screen.width / 33;
        score.position = new Vector3(score.position.x, Screen.height / 1.09f, score.position.z);
        alert.sizeDelta = new Vector2(Screen.width / 5.25f, Screen.height / 14f);
        alert.GetComponent<Text>().fontSize = Screen.width / 66;
        gameOverText.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 7f);
        gameOverText.GetComponent<Text>().fontSize = Screen.width / 40;
        grenadeImg.rectTransform.sizeDelta = new Vector2(Screen.width / 25, Screen.height / 5);
        hitmarker.rectTransform.sizeDelta = new Vector2(Screen.width / 1.03f, Screen.height / .88f);

        foreach (Image crosshair in crosshairs)
        {
            if (crosshair.gameObject.tag == "LeftCrosshair" || crosshair.gameObject.tag == "RightCrosshair")
            {
                crosshair.rectTransform.sizeDelta = new Vector2(Screen.width / 75, Screen.height / 211);
            }

            else if (crosshair.gameObject.tag == "TopCrosshair" || crosshair.gameObject.tag == "BottomCrosshair")
            {
                crosshair.rectTransform.sizeDelta = new Vector2(Screen.height / 211, Screen.width / 75);
            }
        }
    }
}
