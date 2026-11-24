using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    public Transform turnPoint;
    public int turnDirection = 1;

    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null && !activated)
        {
            activated = true;

            player.PrepareTurn(turnPoint, turnDirection);

            Debug.Log("Turn detected!");
        }
    }
}