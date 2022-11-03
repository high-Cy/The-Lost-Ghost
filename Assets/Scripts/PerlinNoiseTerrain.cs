using UnityEngine;

public class PerlinNoiseTerrain : MonoBehaviour
{
    [SerializeField] private float scale = 10f;
    [SerializeField] private float depth = 10;
    [SerializeField] private int size = 256;
    private Terrain terrain;
    private float offsetX = 20f;
    private float offsetZ = 20f;
    private float noisePow = 1.5f;
    private float minHeight = 999f;
    private float maxHeight = -999f;
    private Vector3 minHeightPoint;

    // Start is called before the first frame update
    void Start()
    {
        offsetX = Random.Range(0, 9999f);
        offsetZ = Random.Range(0, 9999f);
        noisePow = Random.Range(3f, 4f); // higher => lesser 'hills'
        depth = noisePow * 15; // lesser 'hills' => 'hills' can be taller
        scale = Random.Range(200, 250) / depth; // high depth => low scale

        terrain = GetComponent<Terrain>();
        terrain.terrainData = GetTerrainData(terrain.terrainData);
    }

    void Update()
    {
        // terrain.terrainData = GetTerrainData(terrain.terrainData);
    }

    private TerrainData GetTerrainData(TerrainData terrainData)
    {
        terrainData.heightmapResolution = size + 1;
        terrainData.size = new Vector3(size, depth, size);
        terrainData.SetHeights(0, 0, GetHeights());
        return terrainData;
    }

    private float[,] GetHeights()
    {
        float[,] heights = new float[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                var height = GetPerlinNoiseHeight(x, z);
                var heightClamped = Mathf.Clamp(height, 0, depth);
                heights[x, z] = heightClamped;

                if (heightClamped < minHeight)
                {
                    minHeight = heightClamped;
                    minHeightPoint = new Vector3(x, heightClamped, z);
                }
                if (heightClamped > maxHeight) maxHeight = heightClamped;
            }
        }
        return heights;
    }

    private float GetPerlinNoiseHeight(int x, int z)
    {
        float xCoord = (float)x / size * scale + offsetX;
        float zCoord = (float)z / size * scale + offsetZ;
        float noise = Mathf.PerlinNoise(xCoord, zCoord);

        // Raise to power to flatten height
        return Mathf.Pow(noise, noisePow);
    }

    public int GetSize()
    {
        return size;
    }
    public float GetMinHeight()
    {
        return minHeight;
    }
    public Vector3 GetMinHeightPoint()
    {
        return minHeightPoint;
    }
    public float GetMaxHeight()
    {
        return maxHeight;
    }
    public float GetHeightAt(int x, int z)
    {
        return terrain.terrainData.GetHeight(x, z);
    }
    public TerrainData GetTerrainData()
    {
        return terrain.terrainData;
    }

    public int GetQuarterSize()
    {
        return size / 4;
    }

    public int Get3rdQuarterSize()
    {
        return 3 * size / 4;
    }
}
