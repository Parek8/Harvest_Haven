using UnityEngine;

public class EnvironmentInitializer : MonoBehaviour
{
    [field: SerializeField, Range(0.00f, 1.00f)] float TreeDensity = 0.5f;
    [field: SerializeField, Range(0.00f, 1.00f)] float RockDensity = 0.5f;
    [field: SerializeField, Range(0.00f, 1.00f)] float GrassDensity = 0.5f;
    [field: SerializeField] int Width = 128;
    [field: SerializeField] int Depth = 128;
    [field: SerializeField] float Scale = 0.5f;
    [field: SerializeField] TerrainData TerrainData;
    [field: SerializeField] Terrain Terrain;
    [field: SerializeField] GameObject TreePrefab;
    [field: SerializeField] GameObject RockPrefab;
    [field: SerializeField] GameObject GrassPrefab;
    [field: SerializeField] LayerMask GroundLayer;

    private void Start()
    {
        TerrainData.heightmapResolution = Width + 1;
        TerrainData.size = new Vector3(Width, 2, Depth);
        Terrain.transform.position = new Vector3(-transform.position.x - Width/2, 0, -transform.position.y - Depth /2);
        SetHeights();
        FillEnvironment();
    }

    private void FillEnvironment()
    {
        Transform _environmentParent = GameManager.game_manager.environment_parent;

        int _trees = Mathf.RoundToInt(Random.Range(5, 25) * 100 * TreeDensity);
        int _rocks = Mathf.RoundToInt(Random.Range(3, 10) * 100 * RockDensity);
        int _grass = Mathf.RoundToInt(Random.Range(10, 30) * 100 * GrassDensity);
        Quaternion _convertedAngle = Quaternion.Euler(90, 0, 0);
        for (int t = 0; t < _trees; t++)
            Instantiate(TreePrefab, GetRandomPosition(), _convertedAngle, _environmentParent);
        for (int r = 0; r < _rocks; r++)
            Instantiate(RockPrefab, GetRandomPosition(), _convertedAngle, _environmentParent);

        //for (int g = 0; g < _grass; g++)
        //    Instantiate(GrassPrefab, GetRandomPosition(), _convertedAngle, _environmentParent);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 _currentPos = transform.position;
        Vector3 _randomPos = Vector3.zero;
        float _x = Random.Range(0.00f, 1.00f) * Width;
        float _z = Random.Range(0.00f, 1.00f) * Depth;

        RaycastHit hitInfo;
        int i = 0;

        while (!Physics.Raycast(new Vector3(_x, 100, _z), Vector3.down*1000, out hitInfo, 1000, GroundLayer) && i < 50)
        {
            _x = Random.Range(0.00f, 1.00f) * Width;
            _z = Random.Range(0.00f, 1.00f) * Depth;
            i++;
        }

        _randomPos = hitInfo.point + _currentPos;
        _randomPos += new Vector3(Terrain.transform.position.x, 0.7f, Terrain.transform.position.z);
        return _randomPos;
    }

    private void SetHeights()
    {
        float[,] _heights = new float[Width, Depth];

        for (int _x = 0; _x < Width; _x++)
        {
            for (int _z = 0; _z < Depth; _z++)
            {
                _heights[_x, _z] = Mathf.PerlinNoise((float)(_x * Scale) / Width, (float)(_z * Scale) / Depth);
            }
        }

        TerrainData.SetHeights(0, 0, _heights);
    }
}