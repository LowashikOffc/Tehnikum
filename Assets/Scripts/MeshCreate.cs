using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Array
{
    public Transform[] array = new Transform[4];
}
public class MeshCreate : MonoBehaviour
{
    [SerializeField] List<Vector3> vec;

    [SerializeField] private List<Array> myVectorList = new List<Array>();

    public string method = "create";
    public bool isStatic = true;

    public float TextureSizeMultiply = 1;
    public bool debugLines = false;

    public bool up, down, left, right, front, back;

    void Start()
    {
        GameObject obj = new GameObject();
        obj.name = "OBJ";
        if (method == "create")
        {
            Create(obj);
            Destroy(gameObject);
        }
        else if (method == "texturescale")
        {
            gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x * TextureSizeMultiply, transform.localScale.y * TextureSizeMultiply);
        }
        else if (method == "createbyobj")
        {
            CalculateDots();
            if (down) CreatePlane(vec[0], vec[1], vec[2], vec[3], obj);

            if (front) CreatePlane(vec[4], vec[5], vec[0], vec[1], obj);

            if (right) CreatePlane(vec[2], vec[6], vec[0], vec[4], obj);

            if (back) CreatePlane(vec[6], vec[2], vec[7], vec[3], obj);

            if (left) CreatePlane(vec[1], vec[5], vec[3], vec[7], obj);

            if (up) CreatePlane(vec[4], vec[6], vec[5], vec[7], obj);

            Destroy(gameObject);
        }
        else Debug.Log("Incorrect method!");
    }
    void Create(GameObject parent)
    {
        foreach(Array E in myVectorList)
        {
            Debug.Log("Try to create");
            CreatePlane(E.array[0].position, E.array[1].position, E.array[3].position, E.array[2].position, parent);
            Debug.Log("Plane created");
        }
    }

    void AddDots()
    {
        int str = 0;
        for (int i = 8; i > 0; i--)
        {
            vec.Add(transform.Find(str.ToString()).transform.position);
            str++;
        }
    }

    public void CalculateDots()
    {
        Vector3 mid = transform.position;
        Vector3 scal = transform.localScale;
        Vector3 dot1 = mid + new Vector3(scal.x / 2, -scal.y / 2, scal.z / 2);
        vec.Add(dot1);

        Vector3 dot2 = mid + new Vector3(-scal.x / 2, -scal.y / 2, scal.z / 2);
        vec.Add(dot2);
        
        Vector3 dot3 = mid + new Vector3(scal.x / 2, -scal.y / 2, -scal.z / 2);
        vec.Add(dot3);
        
        Vector3 dot4 = mid + new Vector3(-scal.x / 2, -scal.y / 2, -scal.z / 2);
        vec.Add(dot4);
        
        Vector3 dot5 = mid + new Vector3(scal.x / 2, scal.y / 2, scal.z / 2);
        vec.Add(dot5);
        
        Vector3 dot6 = mid + new Vector3(-scal.x / 2, scal.y / 2, scal.z / 2);
        vec.Add(dot6);

        Vector3 dot7 = mid + new Vector3(scal.x / 2, scal.y / 2, -scal.z / 2);
        vec.Add(dot7);
        
        Vector3 dot8 = mid + new Vector3(-scal.x / 2, scal.y / 2, -scal.z / 2);
        vec.Add(dot8);
    }

    private void Update()
    {
        //if (vec.Count != 8) return;
        if (debugLines == false) return;
        Drawlines(vec);
    }

    void Drawlines(List<Vector3> vec)
    {
        Debug.DrawLine(vec[0], vec[1]);
        Debug.DrawLine(vec[0], vec[2]);
        Debug.DrawLine(vec[3], vec[1]);
        Debug.DrawLine(vec[3], vec[2]);

        Debug.DrawLine(vec[4], vec[5]);
        Debug.DrawLine(vec[4], vec[6]);
        Debug.DrawLine(vec[7], vec[5]);
        Debug.DrawLine(vec[7], vec[6]);

        Debug.DrawLine(vec[0], vec[4]);
        Debug.DrawLine(vec[1], vec[5]);
        Debug.DrawLine(vec[2], vec[6]);
        Debug.DrawLine(vec[3], vec[7]);

        Debug.DrawLine(vec[6], vec[5]);
        Debug.DrawLine(vec[1], vec[2]);
        Debug.DrawLine(vec[2], vec[7]);
        Debug.DrawLine(vec[0], vec[6]);
        Debug.DrawLine(vec[0], vec[5]);
        Debug.DrawLine(vec[3], vec[5]);
    }

    void CreatePlane(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, GameObject parent)
    {
        GameObject plane = new GameObject("Plane");
        plane.transform.SetParent(parent.transform);
        if (isStatic == true) plane.isStatic = true;

        MeshFilter filter = plane.AddComponent<MeshFilter>();
        MeshRenderer renderer = plane.AddComponent<MeshRenderer>();
        renderer.material = GetComponent<MeshRenderer>().material;

        Mesh mesh = new Mesh();

        int[] triangles = new int[6]
{
            0, 1, 2,
            1, 3, 2
};

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
        Vector3[] normals = new Vector3[4]
        {
            normal,
            normal,
            normal,
            normal
        };

        Vector3[] vertices = new Vector3[4]
        {
            p0, p1, p2, p3
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.normals = normals;
        mesh.RecalculateTangents();

        MeshCollider collider = plane.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;
        float width = Vector3.Distance(p0, p1);
        float height = Vector3.Distance(p0, p2);

        renderer.material.mainTextureScale = new Vector2(width * TextureSizeMultiply, height * TextureSizeMultiply);

        filter.mesh = mesh;
    }
}
