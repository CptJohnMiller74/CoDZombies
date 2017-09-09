using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textConfig : MonoBehaviour {

    public Text bulletCount;
    public Text grenadeCount;
    public Image grenadeImg;
    public Image hitmarker;
    public Text wave;
    public Text score;
    public Text alert;
    public Image[] crosshairs;

    void Start () {
        bulletCount.fontSize = Screen.width / 33;
        grenadeCount.fontSize = Screen.width / 56;
        grenadeImg.rectTransform.sizeDelta = new Vector2(Screen.width / 50, Screen.height / 19);
        hitmarker.rectTransform.sizeDelta = new Vector2(Screen.width / 1.03f, Screen.height / .88f);
        wave.fontSize = Screen.width / 33;
        score.fontSize = Screen.width / 33;
        alert.fontSize = Screen.width / 65;
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
