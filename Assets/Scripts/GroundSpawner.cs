using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject straightHallway;
    public GameObject leftHallway;
    public GameObject rightHallway;

    public int startingTiles = 7;
    public int minimumStraightTiles = 2;
    public int safeStartingTiles = 2;

    private int straightCount = 0;
    private int totalTiles = 0;

    private Vector3 spawnPosition = Vector3.zero;
    private Quaternion spawnRotation = Quaternion.identity;

    private Queue<GameObject> activeTiles =
        new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < startingTiles; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        GameObject selectedTile = straightHallway;

        // Only allow turns after enough straight hallways
        if (straightCount >= minimumStraightTiles
            && totalTiles >= safeStartingTiles)
        {
            int random = Random.Range(0, 10);

            if (random < 3)
                selectedTile = leftHallway;
            else if (random < 6)
                selectedTile = rightHallway;
        }

        GameObject newTile = Instantiate(
            selectedTile,
            spawnPosition,
            spawnRotation
        );

        // Keep starting hallways and corners empty
        if (totalTiles < safeStartingTiles
            || selectedTile != straightHallway)
        {
            ObstacleSpawner obstacles =
                newTile.GetComponent<ObstacleSpawner>();

            CashSpawner cash =
                newTile.GetComponent<CashSpawner>();

            if (obstacles != null)
                obstacles.enabled = false;

            if (cash != null)
                cash.enabled = false;
        }

        activeTiles.Enqueue(newTile);

        HallwayTile tile =
            newTile.GetComponent<HallwayTile>();

        spawnPosition = tile.endPoint.position;
        spawnRotation = tile.endPoint.rotation;

        if (selectedTile == straightHallway)
            straightCount++;
        else
            straightCount = 0;

        totalTiles++;
    }

    public void PlayerPassedTile()
    {
        SpawnTile();

        if (activeTiles.Count > startingTiles + 2)
        {
            Destroy(activeTiles.Dequeue());
        }
    }
}