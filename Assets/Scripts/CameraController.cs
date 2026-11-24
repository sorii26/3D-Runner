using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0, 2.8f, -4f);

    public float turnSpeed = 720f;
    public float lookHeight = 1.3f;

    private float cameraAngle;

    void Start()
    {
        cameraAngle = player.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Quickly rotate with the player
        cameraAngle = Mathf.MoveTowardsAngle(
            cameraAngle,
            player.eulerAngles.y,
            turnSpeed * Time.deltaTime
        );

        Quaternion rotation = Quaternion.Euler(
            0, cameraAngle, 0
        );

        // Stay behind the player
        transform.position =
            player.position + rotation * offset;

        transform.LookAt(
            player.position + Vector3.up * lookHeight
        );
    }
}