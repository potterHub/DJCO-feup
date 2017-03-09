using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

    public Image healthSlider;
    public Text healthText;

    public void updateBar(float healthPercentage) {
        healthSlider.fillAmount = healthPercentage;
        healthText.text = (healthPercentage * 100) + "%";
    }

}
