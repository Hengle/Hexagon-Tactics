using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    public Slider slider;

    public float maxValue;
    public float currentValue;

    // Use this for initialization
    void Awake () {
        slider = GetComponent<Slider>();
        currentValue = 0;
        maxValue = 1;
	}
	
	public void SetValue(float value) {
        float percent = value / maxValue;
        slider.value = percent;
    }

    public void setMaxValue(float value) {
        maxValue = value;
        SetValue(currentValue);
    }
}
