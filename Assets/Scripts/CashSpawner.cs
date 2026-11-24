using UnityEngine;

public class CashSpawner : MonoBehaviour
{
    public GameObject cashPrefab;
    public int cashCount = 4;
    public float spacing = 2.5f;

    [Range(0f, 1f)]
    public float spawnChance = 0.3f;

    void Start()
    {
        // Randomly decide whether to spawn cash
        if (Random.value >= spawnChance)
            return;

        // Choose a random lane
        int lane = Random.Range(-1, 2);

        for (int i = 0; i < cashCount; i++)
        {
            Vector3 position = new Vector3(
                lane * 3f,
                0.8f,
                5f + i * spacing
            );

            Instantiate(
                cashPrefab,
                transform.TransformPoint(position),
                transform.rotation,
                transform
            );
        }
    }
}