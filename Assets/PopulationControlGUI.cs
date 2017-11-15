using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationControlGUI : MonoBehaviour
{

    public List<PopSlider> sliders = new List<PopSlider>();

    

    // Use this for initialization
    void Start()
    {
        //sliders.Add(new PopSlider(0.0f, 10.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        sliders.Add(new PopSlider(0.0f, 10.0f, 0.0f));
    }

    public class PopSlider
    {
        private float minValue;
        private float maxValue;

        //orientiation in deg or radians?
        private float direction;

        public float hSliderValue = 0.0F;

        //initial values
        public PopSlider(float min, float max, float dir)
        {
            minValue = min;
            maxValue = max;
            direction = dir;
            
            hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), 5.0f, min, max);
        }
    }

}
