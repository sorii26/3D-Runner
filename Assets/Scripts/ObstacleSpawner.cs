using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject stationaryPolice;
    public GameObject movingPolice;
    public GameObject laserObstacle;

    [Range(0f, 1f)]
    public float spawnChance = 0.9f;

    [Range(0f, 1f)]
    public float secondObstacleChance = 0.5f;

    void Start()
    {
        if (Random.value < spawnChance)
        {
            RandomObstacle(6f);

            if (Random.value < secondObstacleChance)
            {
                RandomObstacle(15f);
            }
        }
    }

    void RandomObstacle(float zPosition)
    {
        int random = Random.Range(0, 3);
        int lane = Random.Range(-1, 2);

        if (random == 0)
            SpawnObstacle(stationaryPolice, 0f, zPosition);

        else if (random == 1)
            SpawnObstacle(movingPolice, lane * 3f, zPosition);

        else if (random == 2)
            SpawnObstacle(laserObstacle, 0f, zPosition);
    }

    void SpawnObstacle(GameObject obstacle, float x, float z)
    {
        Vector3 position = transform.TransformPoint(
            new Vector3(x, 0, z));

        Instantiate(obstacle, position,
            transform.rotation, transform);
    }
}