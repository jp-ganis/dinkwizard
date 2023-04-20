using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinkleBrain : MonoBehaviour
{
    DinkWizardController dinkwizard;
    PlayerController dinkTriangle;

    void Start()
    {
        dinkwizard = FindObjectOfType<DinkWizardController>();
        dinkTriangle = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Dinkle code goes here!

        // You have the following functions that should help you:

        // dinkwizard.GetCurrentZapTimer() This gets the current timer for the zap, counting up
        // dinkwizard.GetMaximumZapTimer() This gets the maximum timer for the zap, i.e. when the zap will occur
        // dinkTriangle.Jump() This will make the Dinktriangle jump


    }

}
