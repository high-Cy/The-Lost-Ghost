using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoalGenerator : MonoBehaviour
{
    [SerializeField] private CharacterDB characterDB;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goal;
    [SerializeField] private float minDistance = 100f;

    private PerlinNoiseTerrain terrain;
    private int terrainSize;
    private float offsetPoint = 5f;

    // Start is called before the first frame update
    void Start()
    {
        terrain = FindObjectOfType<PerlinNoiseTerrain>();
        terrainSize = terrain.GetSize();

        InitObjs();

    }

    private void InitObjs()
    {
        Vector3 playerPos, goalPos;
        do
        {
            var playerX = RandomOnTerrain();
            var playerZ = RandomOnTerrain();
            var playerY = terrain.GetHeightAt(playerX, playerZ);
            playerPos = new Vector3(playerX, playerY + 5f, playerZ);

            var goalX = RandomOnTerrain();
            var goalZ = RandomOnTerrain();
            var goalY = terrain.GetHeightAt(goalX, goalZ);
            var goalOffsetY = goal.GetComponent<Renderer>().bounds.size.y / 2;
            goalPos = new Vector3(goalX, goalY, goalZ);

        } while (Vector3.Distance(playerPos, goalPos) <= minDistance);
        player = characterDB.GetCharacter(characterDB.savedIndex);

        Instantiate(player, playerPos, Quaternion.identity, parent);
        Instantiate(goal, goalPos, RandomYRotation(), parent);
    }

    private int RandomOnTerrain()
    {
        return Mathf.RoundToInt(Random.Range(offsetPoint, terrain.GetSize()- offsetPoint));
    }
    private Quaternion RandomYRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }
}
