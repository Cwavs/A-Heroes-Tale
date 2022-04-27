using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perlinTest : MonoBehaviour
{

    private Color[] pix;
    public Texture2D perlinTex;
    public float xOrg;
    public float yOrg;
    public float scale = 50f;
    public Gradient grad = new Gradient();
    public GradientColorKey[] colourKey;
    public GradientAlphaKey[] alphaKey;
    float oldxOrg;
    float oldyOrg;
    float oldScale;
    List<float> vert;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {


        colourKey = new GradientColorKey[3];
        alphaKey = new GradientAlphaKey[1];

        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        colourKey[0].color = Color.red;
        colourKey[0].time = 0.2f;
        colourKey[1].color = Color.green;
        colourKey[1].time = 0.5f;
        colourKey[2].color = Color.blue;
        colourKey[2].time = 1.0f;
        grad.mode = GradientMode.Blend;

        grad.SetKeys(colourKey, alphaKey);

        mesh = GetComponent<MeshFilter>().mesh;

        int size = mesh.triangles.Length * 3;

        vert = new List<float>();
        perlinTex = new Texture2D(mesh.vertexCount/2, mesh.vertexCount/2);
        pix = new Color[perlinTex.width * perlinTex.height];

        calcNoise();

        GetComponent<Renderer>().material.mainTexture = perlinTex;
    }

    // Update is called once per frame
    void Update()
    {
        if (xOrg - oldxOrg != 0 || yOrg - oldyOrg != 0 || scale - oldScale != 0)
        {
            calcNoise();
            updateMesh();
        }
        oldxOrg = xOrg;
        oldyOrg = yOrg;
        oldScale = scale;
    }

    void calcNoise()
    {
        vert.Clear();
        float y = 0.0f;

        while (y < perlinTex.height)
        {
            float x = 0.0f;
            while (x < perlinTex.width)
            {
                float xCoord = xOrg + x / perlinTex.width * scale;
                float yCoord = yOrg + y / perlinTex.height * scale;
                float perlin = Mathf.PerlinNoise(xCoord, yCoord);
                float perlin2 = Mathf.PerlinNoise(yCoord, xCoord);
                float  finalPerlin = (perlin + perlin2) / 2;
                //Color sampleColor = new Color(finalPerlin, finalPerlin, finalPerlin);
                Color sampleColor = grad.Evaluate(finalPerlin);
                float time = (sampleColor.r + sampleColor.g + sampleColor.b / 3);

                vert.Add(sampleColor.grayscale - 0.5f);

                Debug.Log(finalPerlin.ToString());

                pix[(int)y * perlinTex.width + (int)x] = sampleColor;
                x++;
            }
            y++;
        }
        perlinTex.SetPixels(pix);
        perlinTex.Apply();
    }


    void updateMesh()
    {
        List<Vector3> vec = new List<Vector3>();
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vec.Add(new Vector3(mesh.vertices[i].x, vert[i], mesh.vertices[i].z));
        }
        mesh.SetVertices(vec);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        print("Hi");
    }
    public static float PerlinNoise3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
        return xy * xz * yz * yx * zx * zy;
    }
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }

}