using UnityEngine;

public class CashCollectible : MonoBehaviour
{
    public int cashValue = 50;
    public float rotationSpeed = 90f;
    public float floatSpeed = 3f;
    public float floatHeight = 0.15f;

    private Vector3 startPosition;
    private bool collected = false;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        transform.Rotate(
            0, rotationSpeed * Time.deltaTime, 0);

        transform.localPosition = startPosition +
            Vector3.up *
            Mathf.Sin(Time.time * floatSpeed) *
            floatHeight;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null && !collected)
        {
            collected = true;

            Debug.Log("Cash collected!");

            GameManager manager =
                FindAnyObjectByType<GameManager>();

            if (manager != null)
                manager.AddScore(cashValue);
            else
                Debug.LogError("GameManager not found!");

            Destroy(gameObject);
        }
    }
}