using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            Debug.Log("Player hit: " + gameObject.name);

            GameManager manager =
                FindAnyObjectByType<GameManager>();

            if (manager != null)
                manager.GameOver();
            else
                Debug.LogError("GameManager not found!");
        }
    }
}