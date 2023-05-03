using System.Collections.Generic;
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

    [SerializeField] GameObject bombPrefab;

    private Vector3 initialPosition; // The initial position of the object.

    List<GameObject> missilePrefabs = new List<GameObject>();

    float zap_timer = 0.0f;
    float zap_max_time = 9.4f;

    float missile_timer = 0.0f;
    float missile_max_time = 6.6f;

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
        missile_timer += Time.deltaTime;

        // zap
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

        // missiles
        if (missile_timer > missile_max_time && dink != null)
        {
            FireMissiles();
            missile_timer = 0.0f;
        }

        // Move the missiles randomly
        float minMissileSpeed = 1;
        float maxMissileSpeed = 15;

        foreach (GameObject prefab in missilePrefabs)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float speed = Random.Range(minMissileSpeed, maxMissileSpeed);
            prefab.transform.Translate(direction * speed * Time.deltaTime);
        }

        // After the delay, make the prefabs follow the target
        if (missile_timer > 3.5)
        {
            foreach (GameObject prefab in missilePrefabs)
            {
                Vector3 directionToTarget = dink.transform.position - prefab.transform.position;
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
                prefab.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                prefab.transform.position = Vector3.MoveTowards(prefab.transform.position, dink.transform.position, maxMissileSpeed * Time.deltaTime);
            }
        }
    }

    void FireMissiles()
    {
        int numPrefabs = 6;
        float spawnRadius = 1.0f;

        missilePrefabs.Clear();

        // Spawn the prefabs in a circle around the character
        for (int i = 0; i < numPrefabs; i++)
        {
            float angle = i * Mathf.PI * 2f / numPrefabs;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * spawnRadius + transform.position;
            GameObject newPrefab = Instantiate(bombPrefab, pos, Quaternion.identity);
            missilePrefabs.Add(newPrefab);
        }
    }


    public List<Vector2> GetBombPositions()
    {
        List<Vector2> bombPositions = new List<Vector2>();

        foreach (var bomb in missilePrefabs)
        {
            bombPositions.Add(bomb.transform.position);
        }

        return bombPositions;
    }

}
