using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OceanSpawner : MonoBehaviour
{
    public float width = 1;
    public float height = 1;

    public Material OceanMat;

    public void Start()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = OceanMat;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3 l0 = new Vector3(0, 0, 0); //L0
        Vector3 l1 = new Vector3(0, 1, 0); //L1
        Vector3 r0 = new Vector3(1, 0, 0); //R0
        Vector3 r1 = new Vector3(1, 1, 0); //R1
        List<Vector3> points = new List<Vector3>();
        List<int> tris = new List<int>();
        var triTracker = -4;

        for (var y = 0; y <= 10; y++)
        {
            for (var x = 0; y <= 10; y++)
            {
                triTracker += 4;
                points.Add(l0);
                points.Add(l1);
                points.Add(r0);
                points.Add(r1);
                tris.Add(0+triTracker);
                tris.Add(1+triTracker);
                tris.Add(3+triTracker);
                tris.Add(0+triTracker);
                tris.Add(3+triTracker);
                tris.Add(2+triTracker);
                l0 = r0;
                l1 = r1;
                r0.x++;
                r1.x++;
            }
            // Reset to start of row & go up
            l0.x = 0;
            l1.x = 0;
            r0.x = 1;
            r1.x = 1;

            l0 = l1;
            r0 = r1;
            r1.y++;
            l1.y++;
        }


        mesh.vertices = points.ToArray();

        mesh.triangles = tris.ToArray();

        List<Vector3> normalList = new List<Vector3>();
        List<Vector2> uvList = new List<Vector2>();

        for (var step = 0; step < points.Count / 4; step++)
        {
            normalList.Add(-Vector3.forward);
            normalList.Add(-Vector3.forward);
            normalList.Add(-Vector3.forward);
            normalList.Add(-Vector3.forward);
            uvList.Add(new Vector2(0, 0));
            uvList.Add(new Vector2(1, 0));
            uvList.Add( new Vector2(0, 1));
            uvList.Add(new Vector2(1, 1));
        }
        
        Debug.Log(points.Count);
        Debug.Log(uvList.Count);
        Debug.Log(normalList.Count);

        mesh.normals = normalList.ToArray();
        mesh.uv = uvList.ToArray();

        meshFilter.mesh = mesh;
    }
}