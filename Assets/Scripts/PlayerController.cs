
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 7f;
    public float maxSpeed = 18f;
    public float speedIncrease = 0.10f;
    public float laneDistance = 3f;
    public float laneSpeed = 10f;
    public float jumpForce = 7f;
    public float gravity = -20f;
    public float turnTolerance = 1.2f;

    private int currentLane = 0;
    private float laneOffset = 0f;
    private float verticalSpeed;

    private CharacterController controller;

    private Transform turnPoint;
    private int requiredDirection;
    private bool turnAvailable = false;
    private bool turnRequested = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Increase difficulty over time
        forwardSpeed = Mathf.Min(
            forwardSpeed + speedIncrease * Time.deltaTime,
            maxSpeed);

        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.aKey.wasPressedThisFrame ||
                keyboard.leftArrowKey.wasPressedThisFrame)
            {
                HandleDirection(-1);
            }

            if (keyboard.dKey.wasPressedThisFrame ||
                keyboard.rightArrowKey.wasPressedThisFrame)
            {
                HandleDirection(1);
            }
        }

        if (Time.timeScale == 0)
            return;

        // Check if the player reached the corner
        if (turnAvailable)
        {
            float distance = Vector3.Dot(
                turnPoint.position - transform.position,
                transform.forward
            );

            // Turn earlier if the correct key was pressed
            if (turnRequested && distance <= turnTolerance)
            {
                ApplyTurn();
            }
            // Game over if the player misses the turn
            else if (distance <= forwardSpeed * Time.deltaTime)
            {
                FindAnyObjectByType<GameManager>().GameOver();
                return;
            }
        }

        // Lane switching
        currentLane = Mathf.Clamp(currentLane, -1, 1);

        float targetOffset = currentLane * laneDistance;

        float nextOffset = Mathf.MoveTowards(
            laneOffset,
            targetOffset,
            laneSpeed * Time.deltaTime
        );

        float sideMovement = nextOffset - laneOffset;

        laneOffset = nextOffset;

        // Jumping
        if (controller.isGrounded)
        {
            verticalSpeed = -2f;

            if (keyboard != null &&
                keyboard.spaceKey.wasPressedThisFrame)
            {
                verticalSpeed = jumpForce;
            }
        }

        verticalSpeed += gravity * Time.deltaTime;

        // Forward movement
        Vector3 movement =
            transform.forward * forwardSpeed
            * Time.deltaTime
            + transform.right * sideMovement
            + Vector3.up * verticalSpeed
            * Time.deltaTime;

        controller.Move(movement);
    }

    // Determine whether to switch lanes or turn
    void HandleDirection(int direction)
    {
        if (turnAvailable)
        {
            if (direction == requiredDirection)
            {
                turnRequested = true;
            }
            else
            {
                Debug.Log("Wrong turn!");

                FindAnyObjectByType<GameManager>().GameOver();
            }
        }
        else
        {
            currentLane += direction;
        }
    }

    // Called when entering a turning zone
    public void PrepareTurn(Transform point, int direction)
    {
        turnPoint = point;
        requiredDirection = direction;

        turnAvailable = true;
        turnRequested = false;

        Debug.Log(direction == -1
            ? "Press LEFT to turn!"
            : "Press RIGHT to turn!");
    }

    // Rotate into the new hallway
    void ApplyTurn()
    {
        controller.enabled = false;

        transform.rotation = turnPoint.rotation;

        Vector3 newPosition = turnPoint.position
            + transform.right * laneOffset;

        newPosition.y = transform.position.y;

        transform.position = newPosition;

        controller.enabled = true;

        turnAvailable = false;
        turnRequested = false;
        turnPoint = null;
    }
}
