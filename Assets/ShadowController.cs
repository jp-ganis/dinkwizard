using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask layerMask = 1 << 3;
        var hit = Physics2D.Raycast(playerTransform.position, -transform.up, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            // Set the position of the shadow to the point where the raycast hit the ground.
            transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);

            // Calculate the distance between the player and the ground.
            float distance = Vector3.Distance(playerTransform.position, hit.point);

            // Calculate the scale of the shadow based on the distance to the ground.
            float scale = Mathf.Lerp(0.5f, 0.01f, distance / 10.0f);

            // Set the scale of the shadow.
            transform.localScale = new Vector3(scale, transform.localScale.y, 1f);
        }
    }
}
