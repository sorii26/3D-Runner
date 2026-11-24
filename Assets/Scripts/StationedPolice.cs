using UnityEngine;

public class StationedPolice : MonoBehaviour
{
    public float patrolDistance = 3f;
    public float patrolSpeed = 3f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float movement = Mathf.PingPong(
            Time.time * patrolSpeed,
            patrolDistance * 2f
        ) - patrolDistance;

        transform.localPosition = startPosition +
            Vector3.right * movement;
    }
}
