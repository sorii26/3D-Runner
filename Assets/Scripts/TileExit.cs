using UnityEngine;

public class TileExit : MonoBehaviour
{
    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null
            && !activated)
        {
            activated = true;

            FindAnyObjectByType<GroundSpawner>()
                .PlayerPassedTile();
        }
    }
}