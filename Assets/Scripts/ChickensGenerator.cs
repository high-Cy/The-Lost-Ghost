using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensGenerator : MonoBehaviour
{
    private PerlinNoiseTerrain terrain;
    private Vector3 center;
    private Vector3 housePos;
    private int maxLoop = 100;
    private bool ifFirstChick = true;
    private GameObject firstChick;
    [SerializeField] private Transform parent;
    [SerializeField] private float radius = 5;
    [SerializeField] private int numChicks = 15;
    [SerializeField] private float minSpawnDistance = 1;
    [SerializeField] private GameObject hen;
    [SerializeField] private GameObject chick;

    // Start is called before the first frame update
    void Start()
    {
        terrain = FindObjectOfType<PerlinNoiseTerrain>();
        housePos = FindObjectOfType<HouseGenerator>().GetHousePos();
        SpawnHen();
        SpawnChicks();

    }

    private void SpawnChicks()
    {
        var spawnedPos = new List<Vector3>();

        for (int i = 0; i < numChicks; i++)
        {
            int loopAttempt = 0;
            Vector3 pos;
            do
            {
                loopAttempt++;
                if (loopAttempt > maxLoop) break;

                pos = RandomPointCircle(center, radius);
                pos.y = terrain.GetHeightAt((int)pos.x, (int)pos.z) + 10f;

                if (AnyClosePoints(pos, spawnedPos)) continue;

                var go = Instantiate(chick, pos, RandomYRotation(), parent);
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
                    if (ifFirstChick)
                    {
                        ifFirstChick = false;
                        go.AddComponent<ChickFollow>();
                        go.GetComponent<ChickFollow>().rb = go.GetComponent<Rigidbody>();
                        go.GetComponent<ChickAnimations>().enabled = false;
                        firstChick = go;
                    }
                    spawnedPos.Add(pos);
                    break;
                }
            } while (true);
        }
    }

    private void SpawnHen()
    {
        int loopAttempt = 0;
        do
        {
            loopAttempt++;
            if (loopAttempt > maxLoop) break;
            // Spawn hen in the middle
            center = RandomSpawnAreaCenter();
            center.y += 10f;
            var go = Instantiate(hen, center, RandomYRotation(), parent);
            var size = go.GetComponent<Collider>().bounds.size;
            Vector3 sizeVec = new Vector3(size.x, size.y, size.z);

            bool collided = false;
            Collider[] overlaps = Physics.OverlapBox(center, sizeVec / 2, go.transform.rotation);

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
            else break;

        } while (true);
    }

    private Vector3 RandomPointCircle(Vector3 center, float radius)
    {
        Vector3 point = center + Random.onUnitSphere * radius;
        return new Vector3(Mathf.RoundToInt(point.x), point.y, Mathf.RoundToInt(point.z));

    }

    private Vector3 RandomSpawnAreaCenter()
    {
        var x = Mathf.RoundToInt(housePos.x + RandomOffset());
        var z = Mathf.RoundToInt(housePos.z + RandomOffset());
        var y = terrain.GetHeightAt(x, z);
        return new Vector3(x, y, z);
    }

    private float RandomOffset()
    {
        return Random.Range(30f, 40f);
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

    public GameObject GetFirstChick()
    {
        return firstChick;
    }
}
