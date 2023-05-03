using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] SpriteRenderer hp_bar;
    [SerializeField] public Transform shadow;
    [SerializeField] public GameObject bullet;

    DinkleBrain brain;

    public bool on_ground = false;

    float hp = 1.0f;

    float speed = 10.0f;
    float jump_power = 360.0f;

    float jump_cd_timer = 0.0f;
    float jump_cd = 5.0f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        brain = GetComponent<DinkleBrain>();
    }

    void Update()
    {
        var h_v = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(speed * h_v, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && on_ground)
        {
            Jump();
        }

        if (jump_cd_timer > 0.0f)
        {
            jump_cd_timer -= Time.deltaTime;
            jump_cd_timer = Mathf.Max(0, jump_cd_timer);
        }
    }

    public void Jump()
    {
        if (!on_ground) return;
        if (jump_cd_timer > 0.0f) return;

        jump_cd_timer = jump_cd;
        rb.AddForce(new Vector2(rb.velocity.x, jump_power));
    }

    public void Shoot(float angle)
    {
        // Calculate the direction vector from the firing object
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // Spawn the prefab 1 unit away from the firing object
        Vector3 spawnPos = transform.position + new Vector3(direction.x, direction.y, 0f);
        GameObject newPrefab = Instantiate(bullet, spawnPos, Quaternion.identity);
        Destroy(newPrefab, 1.3f);

        // Set the velocity of the prefab to fire off in the specified direction at speed 40
        Rigidbody2D prefabRigidbody = newPrefab.GetComponent<Rigidbody2D>();
        prefabRigidbody.velocity = direction * 40f;
    }

    public void DealDamageToPlayer(float damage)
    {
        hp = Mathf.Clamp(hp - damage, 0.0f, 1.0f);

        OnHpChanged();
    }

    void OnHpChanged()
    {
        hp_bar.color = GetColorFromValue(hp);
        hp_bar.transform.localScale = new Vector3((float)hp, hp_bar.transform.localScale.y, hp_bar.transform.localScale.z);

        if (hp <= 0.0f)
        {
            Destroy(gameObject);
            var shadow = FindObjectOfType<ShadowController>();
            Destroy(shadow.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            on_ground = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            on_ground = false;
        }

        if (collision.gameObject.CompareTag("Bomb"))
        {
            DealDamageToPlayer(100.0f);
        }
    }

    Color GetColorFromValue(float value)
    {
        if (value <= 0.5f)
        {
            // Interpolate from red to yellow.
            return Color.Lerp(Color.red, Color.yellow, value * 2f);
        }
        else
        {
            // Interpolate from yellow to green.
            return Color.Lerp(Color.yellow, Color.green, (value - 0.5f) * 2f);
        }
    }
}
