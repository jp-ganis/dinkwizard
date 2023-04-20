using UnityEngine;

public class DinkWizardController : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 0.2f; // The maximum height the object should hover.
    [SerializeField] private float hoverSpeed = 2f; // The speed of the hovering motion.
    [SerializeField] private float jitterAmount = 0.05f; // The amount of jitter to apply left and right.
    [SerializeField] private float jitterSpeed = 4f; // The speed of the jitter motion.

    [SerializeField] PlayerController dink;

    [SerializeField] LineRenderer leftEye;
    [SerializeField] LineRenderer rightEye;

    private Vector3 initialPosition; // The initial position of the object.

    float zap_timer = 0.0f;
    float zap_max_time = 9.4f;

    private void Start()
    {
        initialPosition = transform.position;
        zap_max_time += Random.Range(-3.0f, 6.0f);
    }

    public float GetCurrentZapTimer()
    {
        return zap_timer;
    }

    public float GetMaximumZapTimer()
    {
        return zap_max_time;
    }

    private void Update()
    {
        // Compute the hover motion using a sine wave.
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Compute the jitter motion using a second sine wave.
        float jitterOffset = Mathf.Cos(Time.time * jitterSpeed) * jitterAmount;

        // Combine the hover and jitter offsets to get the final position.
        Vector3 newPosition = initialPosition + Vector3.up * hoverOffset + Vector3.right * jitterOffset;

        // Update the object's position.
        transform.position = newPosition;

        zap_timer += Time.deltaTime;

        if (zap_timer > zap_max_time && dink != null)
        {
            leftEye.positionCount = 2;
            rightEye.positionCount = 2;

            leftEye.SetPosition(0, leftEye.transform.position);
            leftEye.SetPosition(1, dink.shadow.position);

            rightEye.SetPosition(0, rightEye.transform.position);
            rightEye.SetPosition(1, dink.shadow.position);

            if (dink.on_ground)
            {
                dink.DealDamageToPlayer(1.0f);
            }

            zap_timer = 0.0f;
        }
        else if (zap_timer > 1.0f && zap_timer < 2.0f)
        {
            leftEye.positionCount = 0;
            rightEye.positionCount = 0;
        }
    }
}
