using UnityEngine;

public class PoliceMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float destroyDistance = 8f;

    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.8f;
    public float detectionDistance = 2f;

    public LayerMask laserLayer;

    private Transform player;
    private float groundHeight;
    private float jumpTimer;
    private bool isJumping = false;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>()
            .transform;

        groundHeight = transform.position.y;
    }

    void Update()
    {
        // Run forward toward the robber
        transform.position -= transform.forward *
            moveSpeed * Time.deltaTime;


        // Detect a laser ahead
        Vector3 origin = transform.position +
            Vector3.up * 0.6f;

        if (!isJumping && Physics.Raycast(
            origin,
            -transform.forward,
            detectionDistance,
            laserLayer,
            QueryTriggerInteraction.Collide))
        {
            isJumping = true;
            jumpTimer = 0f;
        }

        // Jump over the laser
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;

            float progress = Mathf.Clamp01(
                jumpTimer / jumpDuration);

            float height = Mathf.Sin(
                progress * Mathf.PI) * jumpHeight;

            Vector3 position = transform.position;
            position.y = groundHeight + height;
            transform.position = position;

            if (progress >= 1f)
            {
                isJumping = false;
            }
        }

        // Destroy after passing the robber
        float distance = Vector3.Dot(
            player.position - transform.position,
            transform.forward);

        if (distance > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}