using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGenerator : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private LayerMask targetLayer;
    private PerlinNoiseTerrain terrain;
    private float offset = 2f;
    private Vector3 housePos;
    private int maxLoop = 100;

    // Start is called before the first frame update
    void Start()
    {
        terrain = FindObjectOfType<PerlinNoiseTerrain>();
        InitHouse();
    }

    // void LateUpdate()
    // {
    //     Invoke("SetKinematic", 15);
    // }

    private void InitHouse()
    {
        var houseX = RandomOnTerrain();
        var houseZ = RandomOnTerrain();
        housePos = new Vector3(houseX, terrain.GetHeightAt(houseX, houseZ) + offset, houseZ);
        Quaternion houseQuat = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Instantiate(house, housePos, houseQuat, parent);

        GameObject objInstance;
        Vector3 objPos;
        foreach (var obj in objects)
        {
            int loopAttempt = 0;
            do
            {
                loopAttempt++;
                if (loopAttempt > maxLoop) break;

                var objX = Mathf.RoundToInt(housePos.x + RandomOffset());
                var objZ = Mathf.RoundToInt(housePos.z + RandomOffset());
                var objY = terrain.GetHeightAt(objX, objZ) + offset;
                objPos = new Vector3(objX, objY, objZ);

                objInstance = Instantiate(obj, objPos, RandomYRotation(), parent);
                var size = objInstance.GetComponent<Collider>().bounds.size;
                Vector3 sizeVec = new Vector3(size.x, size.y, size.z);

                bool collided = false;
                Collider[] overlaps = Physics.OverlapBox(objPos, sizeVec / 2, objInstance.transform.rotation, targetLayer);

                foreach (Collider collider in overlaps)
                {
                    // ignore self
                    if (collider.transform == objInstance.transform) continue;
                    // ignore terrain
                    if (collider.tag == "ProceduralTerrain") continue;

                    collided = true;
                    break;
                }

                if (collided) Destroy(objInstance);
                else break;

            } while (true);
        }
    }

    private int RandomOnTerrain()
    {
        return Mathf.RoundToInt(Random.Range(150f, terrain.GetSize() - 150f));
    }

    private float RandomOffset()
    {
        bool toAdd = Random.Range(0, 2) == 0;
        var num = Random.Range(15f, 40f);
        return toAdd ? num : -num;
    }

    private Quaternion RandomYRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }

    private void SetKinematic()
    {
        house.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        foreach (var obj in objects)
        {
            if (obj.GetComponent<Rigidbody>() != null)
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        }
    }
    public Vector3 GetHousePos()
    {
        return housePos;
    }
}
