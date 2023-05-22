using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{

    public NavMeshSurface[] surfaces;

    // Use this for initialization
    void Awake()
    {
        BakeNavMesh();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            BakeNavMesh();
        }
    }


    public void BakeNavMesh()
    {
        /*for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }*/
    }
}
