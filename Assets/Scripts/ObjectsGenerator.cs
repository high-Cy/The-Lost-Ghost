using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
    private PerlinNoiseTerrain terrain;
    private Vector3 center;
    private int maxLoop = 100;
    [SerializeField] private Transform parent;
    [SerializeField] private float radius = 10;
    [SerializeField] private int numObjects = 10;
    [SerializeField] private float minSpawnDistance = 1;
    [SerializeField] private List<GameObject> objectTypes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        terrain = FindObjectOfType<PerlinNoiseTerrain>();
        InitObjects();
    }

    private void InitObjects()
    {
        center = RandomSpawnAreaCenter();
        var spawnedPos = new List<Vector3>();

        for (int i = 0; i < numObjects; i++)
        {
            int loopAttempt = 0;
            int rand = Random.Range(0, objectTypes.Count);
            Vector3 pos;
            do
            {
                loopAttempt++;
                if (loopAttempt > maxLoop) break;

                pos = RandomPointCircle(center, radius);
                pos.y = terrain.GetHeightAt((int)pos.x, (int)pos.z) +1f;

                if (AnyClosePoints(pos, spawnedPos)) continue;

                var go = Instantiate(objectTypes[rand], pos, RandomYRotation(), parent);

                var size = go.GetComponent<Collider>().bounds.size;
                Vector3 sizeVec = new Vector3(size.x, size.y, size.z);

                bool collided = false;
                Collider[] overlaps = Physics.OverlapBox(pos, sizeVec / 2, go.transform.rotation);

                foreach (Collider collider in overlaps)
                {
                    // ignore self
                    if (collider.transform == go.transform) continue;
                    // ignore terrain
                    if (collider.tag == "ProceduralTerrain") continue;

                    collided = true;
                    break;
                }

                if (collided) Destroy(go);
                else
                {
                    spawnedPos.Add(pos);
                    break;
                }
            } while (true);
        }
    }

    private Vector3 RandomPointCircle(Vector3 center, float radius)
    {
        Vector3 point = center + Random.onUnitSphere * radius;
        return new Vector3(Mathf.RoundToInt(point.x), point.y, Mathf.RoundToInt(point.z));

    }

    private Vector3 RandomSpawnAreaCenter()
    {
        var x = RandomInt();
        var z = RandomInt();
        var y = terrain.GetHeightAt(x, z);
        return new Vector3(x, y, z);
    }

    private int RandomInt()
    {
        return Mathf.RoundToInt(Random.Range(radius, terrain.GetSize()- radius));
    }

    private Quaternion RandomYRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }

    private bool AnyClosePoints(Vector3 newPos, List<Vector3> spawnedPos)
    {
        foreach (var pos in spawnedPos)
        {
            if ((pos - newPos).magnitude <= minSpawnDistance) return true;
        }
        return false;
    }

    public Vector3 GetCenter()
    {
        return center;
    }
}
