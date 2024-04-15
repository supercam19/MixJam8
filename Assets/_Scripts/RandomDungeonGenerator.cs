using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using NavMeshPlus.Components;

public class RandomDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    private int iterations = 10;
    [SerializeField]
    private int walkLength = 10;
    [SerializeField]
    public bool startRandomlyEachTime = true;

    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;

    public NavMeshSurface navMesh;

    [SerializeField]
    private GameObject enemyPrefab;

    public int enemiesAlive;
    private int floorCount;

    public void RunProceduralGeneration()
    {
        ProceduralGenerationAlgorithms.ClearOrigins();
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        navMesh.BuildNavMesh();
        SpawnEnemies(ProceduralGenerationAlgorithms.GetPathOrigins());
        floorCount++;
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);
            if (startRandomlyEachTime)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }

        }
        return floorPositions;
    }

    private void SpawnEnemies(HashSet<Vector2Int> floorPositions)
    {
        int chance = Mathf.Min(15 + floorCount * 2, 70);
        foreach (var position in floorPositions)
        {
            if (Vector2.Distance(position, startPosition) > 10)
            {
                if (Random.Range(0, 100) < chance)
                {
                    Instantiate(enemyPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity).GetComponent<EnemyAI>().currentFloor = floorCount;
                    enemiesAlive++;
                }
            }
        }
        if (enemiesAlive == 0)
        {
            Vector2Int randomPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            Instantiate(enemyPrefab, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity);
            enemiesAlive++;
        }
    }
}
