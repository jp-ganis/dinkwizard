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

        if (dinkwizard.GetMaximumZapTimer() - dinkwizard.GetCurrentZapTimer() < 0.5f)
        {
            dinkTriangle.Jump();
        }

        // Missile Code functions

        // List<Vector2> bombPositions = dinkwizard.GetBombPositions();
        // dinkTriangle.Shoot(angle);

        // The first function will get you a list of Vector2 (X,Y coordinates) of the positions of the bombs.
        // The second function will fire a bullet at the desired angle.


    }

}
