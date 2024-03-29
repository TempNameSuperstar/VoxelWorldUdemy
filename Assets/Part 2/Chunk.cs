﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material cubeMaterial;
    public Block[,,] chunkData;

    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
    {
        chunkData = new Block[sizeX, sizeY, sizeZ];

        // Create blocks
        for (int z = 0; z < sizeZ; z++)
            for (int y = 0; y < sizeY; y++)
                for (int x = 0; x < sizeX; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (Random.Range(0,100) < 50)
                        chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, gameObject, cubeMaterial);
                    else
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, gameObject, cubeMaterial);
                }

        // Draw blocks
        for (int z = 0; z < sizeZ; z++)
            for (int y = 0; y < sizeY; y++)
                for (int x = 0; x < sizeX; x++)
                {
                    chunkData[x, y, z].Draw();
                }
        CombineQuads();
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildChunk(16, 16, 16));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CombineQuads()
    {
        // 1. Combine all child meshes
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // 2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        // 3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        // 4. Create a renderer for the parent
        MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = cubeMaterial;

        foreach (Transform quad in transform)
        {
            Destroy(quad.gameObject);
        }
    }


}
