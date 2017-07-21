using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crosshairController : MonoBehaviour {

    public Image[] crosshairs;
    public PlayerShooterNew playerShooter;

    void Awake () {
        //updateCrosshairs();
        clearCrosshairs();
	}

    void Update()
    {
        //updateCrosshairs();
    }

    public void clearCrosshairs()
    {
        foreach (Image crosshair in crosshairs)
        {
            crosshair.color = Color.clear;
        }
    }

    public void makeCrosshairsVisible()
    {
        foreach (Image crosshair in crosshairs)
        {
            crosshair.color = Color.white;
        }
    }


    public void updateCrosshairs()
    {
        foreach (Image crosshair in crosshairs)
        {
            if (playerShooter.getActiveGun().getCurrentSpread() >= playerShooter.getActiveGun().minSpread)
            {
                if (crosshair.gameObject.tag == "LeftCrosshair")
                {
                    crosshair.transform.position = new Vector3(-Mathf.Abs(playerShooter.getActiveGun().getCurrentSpread() * 600) + 453, 211.5f, 0);
                }

                if (crosshair.gameObject.tag == "RightCrosshair")
                {
                    crosshair.transform.position = new Vector3(Mathf.Abs(playerShooter.getActiveGun().getCurrentSpread() * 600) + 453, 211.5f, 0);
                }

                if (crosshair.gameObject.tag == "TopCrosshair")
                {
                    crosshair.transform.position = new Vector3(453, Mathf.Abs(playerShooter.getActiveGun().getCurrentSpread() * 600) + 211.5f, 0);
                }

                if (crosshair.gameObject.tag == "BottomCrosshair")
                {
                    crosshair.transform.position = new Vector3(453, -Mathf.Abs(playerShooter.getActiveGun().getCurrentSpread() * 600) + 211.5f, 0);
                }
            }
        }

    }
}
