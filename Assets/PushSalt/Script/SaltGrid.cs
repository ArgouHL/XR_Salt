using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SaltGrid : MonoBehaviour
{
    [SerializeField] private GameObject cell;
    [SerializeField] private Vector2Int cellCount;
    [SerializeField] private Vector3 cellStartPos;
    [SerializeField] private float cellSize;
 
    private void Start()
    {
        for(int x=0;x< cellCount.x;x++)
        {
            for (int z = 0; z < cellCount.y; z++)
            {
                Instantiate(cell, new Vector3(x, 0, z) * cellSize + cellStartPos+transform.position, Quaternion.identity,transform).transform.localScale= new Vector3(cellSize,1, cellSize);

            }
        }
    }
}
