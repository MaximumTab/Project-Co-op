using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITesting : MonoBehaviour
{
    [SerializeField] private float decrementAmount = 0.1f; //smaller the number, the smoother the slider will be
    public Slider cooldownSlider;   //this is the grey overlay

    //--------How to use: modify "value" field in inspector window of slider. increase to make cooldown longer, decrease for shorter -----------

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))    //DEBUGGING placeholder, this will be bound to relevant attack key
        {
            Debug.Log("Attack Activated");
            cooldownSlider.value = cooldownSlider.maxValue; //sets the slider to full to cover sprite 
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        while (cooldownSlider.value != cooldownSlider.minValue)    //while the slider is not "empty"
        {
            cooldownSlider.value-= decrementAmount; //
            yield return null;  //called every frame to create smooth scroll effect

            /* Notes
            * we are decrementing to create the effect of the grey slider getting smaller
            * its a regular image, with a slider with lowered alpha value on top to make it look grey.
            * the slider direction is set to "top to bottom", so we decrement to make it look like its rising
            */
        }
       
    }

   
}
