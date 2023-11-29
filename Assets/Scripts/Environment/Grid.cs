using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [field: SerializeField] int x_count = 16;
    [field: SerializeField] int y_count = 16;
    [field: SerializeField] int z_count = 16;

    [field: SerializeField] int x_spacing = 0;
    [field: SerializeField] int y_spacing = 0;
    [field: SerializeField] int z_spacing = 0;

    [field: SerializeField] int x_size = 1;
    [field: SerializeField] int y_size = 1;
    [field: SerializeField] int z_size = 1;

    [field: SerializeField] GameObject plot_prefab;

    private GameObject[,,] grid;
    void Start()
    {
        grid = new GameObject[x_count, y_count, z_count];
        Make_Grid();
    }

    private void Make_Grid()
    {
        for (int y = 0; y < y_count; y++)
        {
            for (int x = 0; x < x_count; x++)
            {
                for (int z = 0; z < z_count; z++)
                {
                    Vector3 pos = new Vector3((x * x_size) + x_spacing, (y * y_size)+y_spacing, (z*z_size)+z_spacing);
                    GameObject pref = Instantiate(plot_prefab, transform);
                    pref.transform.localPosition = pos;
                    grid[x, y, z] = pref;
                }
            }
        }
    }
}
