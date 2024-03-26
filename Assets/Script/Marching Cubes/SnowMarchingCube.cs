
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Boundary
{
    public float boundarySize;
    public Vector3 boundaryConner;
    public List<Vector3> posInBoundary;
}

public class SnowMarchingCube : MonoBehaviour
{
    public Vector3Int GridSize = new Vector3Int(30, 10, 30);
    private Vector3Int boundaryCount;
    public float boundarySize;

    public int GridPerUnit = 2;
    public Material material = null;
    private Mesh mesh;
    public float zoom = 1f;
    public Vector3[] ballposs;
    public Boundary[,,] sortingBoundarys;


    public float ballradius = 1f;
    public GameObject go;
    public bool drawGimzo;






    private void Start()
    {
        go = this.gameObject;
        MakeGrid();

        // Noise2D();

        GenerateBoundarys();
        GenerateBalls();
    }

    private void GenerateBoundarys()
    {
        boundaryCount.x = Mathf.FloorToInt(GridSize.x / boundarySize);
        boundaryCount.y = Mathf.FloorToInt(GridSize.y / boundarySize);
        boundaryCount.z = Mathf.FloorToInt(GridSize.z / boundarySize);
     
        sortingBoundarys = new Boundary[boundaryCount.x, boundaryCount.y, boundaryCount.z];

        for (int z = 0; z < boundaryCount.x; z++)
            for (int y = 0; y < boundaryCount.y; y++)
                for (int x = 0; x < boundaryCount.z; x++)
                {
                    sortingBoundarys[x, y, z] = new Boundary();
                    sortingBoundarys[x, y, z].boundarySize = boundarySize;
                    sortingBoundarys[x, y, z].boundaryConner = new Vector3(boundarySize * x, boundarySize * y, boundarySize * z);

                }
    }

    private void GenerateBalls()
    {
        int count = 5;
        ballposs = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            ballposs[i] = new Vector3(UnityEngine.Random.Range(4.1f, GridSize.x / GridPerUnit) - 4.1f, UnityEngine.Random.Range(4.1f, GridSize.y / GridPerUnit - 4.1f), UnityEngine.Random.Range(4.1f, GridSize.z / GridPerUnit) - 4);

        }
    }

    private void Update()
    {
       // SortingPoints();
        GetPoints();

        March();
    }



    private void MakeGrid()
    {
        transform.localScale = Vector3.one / GridPerUnit;
        MarchingCube.grd = new GridPoint[(int)GridSize.x, (int)GridSize.y, (int)GridSize.z];
        for (int z = 0; z < GridSize.z; z++)
            for (int y = 0; y < GridSize.y; y++)
                for (int x = 0; x < GridSize.x; x++)
                {
                    MarchingCube.grd[x, y, z] = new GridPoint();
                    MarchingCube.grd[x, y, z].Position = new Vector3(x, y, z);
                    MarchingCube.grd[x, y, z].On = false;
                }
    }
    private void Noise2D()
    {
        for (int z = 0; z < GridSize.z; z++)
            for (int x = 0; x < GridSize.x; x++)
            {
                float nx = (x / GridSize.x) * zoom;
                float nz = (z / GridSize.z) * zoom;
                float height = Mathf.PerlinNoise(nx, nz) * GridSize.y;
                for (int y = 0; y < GridSize.y; y++)
                {
                    MarchingCube.grd[x, y, z].On = y < height;
                }
            }
    }

    private void SortingPoints()
    {
        Vector3 localPos = transform.position;
        for (int z = 0; z < boundaryCount.x; z++)
            for (int y = 0; y < boundaryCount.y; y++)
                for (int x = 0; x < boundaryCount.z; x++)
                {
                    sortingBoundarys[x, y, z].posInBoundary = new List<Vector3>();

                }

        foreach (var ballpos in ballposs)
        {
            Vector3 pos = ballpos - localPos;
            int xIndex = Mathf.CeilToInt(pos.x / boundarySize);
            int yIndex = Mathf.CeilToInt(pos.y / boundarySize);
            int zIndex = Mathf.CeilToInt(pos.z / boundarySize);
            try
            {
                sortingBoundarys[xIndex, yIndex, zIndex].posInBoundary.Add(ballpos);
            }
            catch
            {
                Debug.Log(new Vector3(xIndex, yIndex, zIndex));
            }

        }
    }

    private void GetPoints()
    {


        //for (int z = 0; z < GridSize.x; z++)
        //{
        //    int searchZ = Mathf.CeilToInt(z / GridPerUnit / boundarySize);
        //    int reletiveZMax = (z / GridPerUnit) % boundarySize > boundarySize / 2 ? 1 : 0;
        //    int reletiveZMin = reletiveZMax - 1;

        //    for (int y = 0; y < GridSize.y; y++)
        //    {
        //        int searchY = Mathf.CeilToInt(z / GridPerUnit / boundarySize);
        //        int reletiveYMax = (y / GridPerUnit) % boundarySize > boundarySize / 2 ? 1 : 0;
        //        int reletiveYMin = reletiveYMax - 1;
        //        for (int x = 0; x < GridSize.z; x++)
        //        {
        //            int searchX = Mathf.CeilToInt(z / GridPerUnit / boundarySize);
        //            int reletiveXMax = (x / GridPerUnit) % boundarySize > boundarySize / 2 ? 1 : 0;
        //            int reletiveXMin = reletiveXMax - 1;

        //            MarchingCube.grd[x, y, z].On = false;

        //            for (int sx = searchX + reletiveXMin; sx <= searchX + reletiveXMax; sx++)
        //            {
        //                if (sx < 0)
        //                    return;
        //                if (MarchingCube.grd[x, y, z].On)
        //                    break;
        //                for (int sy = searchY + reletiveYMin; sy <= searchY + reletiveYMax; sy++)
        //                {
        //                    if (sy < 0)
        //                        return;
        //                    if (MarchingCube.grd[x, y, z].On)
        //                        break;
        //                    for (int sz = searchZ + reletiveZMin; sz <= searchZ + reletiveZMax; sz++)
        //                    {
        //                        if (sz < 0)
        //                            return;
        //                        if (MarchingCube.grd[x, y, z].On)
        //                            break;
        //                        foreach (var pos in sortingBoundarys[sz, sy, sz].posInBoundary)
        //                        {
        //                            if (MarchingCube.grd[x, y, z].On)
        //                                break;
        //                            MarchingCube.grd[x, y, z].On = (new Vector3(x, y, z) / GridPerUnit - pos).sqrMagnitude < ballradius * ballradius;


        //                        }
        //                    }
        //                }
        //            }



        //        }
        //    }

        //}









        for (int z = 0; z < GridSize.z; z++)
        {

            for (int x = 0; x < GridSize.x; x++)
            {

                for (int y = 0; y < GridSize.y; y++)
                {
                    if (MarchingCube.grd[x, y, z].On)
                        return;
                    foreach (var pos in ballposs)
                    {
                        MarchingCube.grd[x, y, z].On = (new Vector3(x, y, z) / GridPerUnit + transform.position - pos).sqrMagnitude < ballradius * ballradius;
                        // MarchingCube.grd[x, y, z].On = (new Vector3(x, y, z) / GridPerUnit + transform.position - pos).magnitude < ballradius;
                    }

                }
            }

        }

    }







    private void March()
    {
        mesh = MarchingCube.GetMesh(ref go, ref material);
        MarchingCube.Clear();
        MarchingCube.MarchCubes();
        MarchingCube.SetMesh(ref mesh);
    }

    private void OnDrawGizmos()
    {
        if (!drawGimzo)
            return;
        foreach (var ballpos in ballposs)
        {
            Gizmos.DrawWireSphere(ballpos, ballradius);
        }
    }



    public void PointsInSphere(Vector3 center, float radius)
    {

        Vector3 pos = transform.position;
        center -= pos;
        for (int x = Mathf.CeilToInt((center.x - radius) * GridPerUnit); x <= Mathf.FloorToInt((center.x + radius) * GridPerUnit); x++)
        {

            for (int z = Mathf.CeilToInt((center.z - radius) * GridPerUnit); z <= Mathf.FloorToInt((center.z + radius) * GridPerUnit); z++)
            {
                for (int y = Mathf.CeilToInt((center.y - radius) * GridPerUnit); y <= Mathf.FloorToInt((center.y + radius) * GridPerUnit); y++)
                {


                    Vector3 gridpoint = new Vector3((float)x / GridPerUnit, (float)y / GridPerUnit, (float)z / GridPerUnit);

                    if ((center - gridpoint).sqrMagnitude <= radius * radius)
                    {
                        try
                        {
                            MarchingCube.grd[x, y, z].On = true;
                        }
                        catch
                        {

                        }
                    }
                }

            }

        }

    }
}
