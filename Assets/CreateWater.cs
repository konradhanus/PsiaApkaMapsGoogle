// using UnityEngine;

// public class LowPolyWater : MonoBehaviour
// {
//     public Material waterMaterial; // Materiał wody

//     public int gridSize = 10; // Rozmiar siatki
//     public float waveSpeed = 1.0f; // Prędkość falowania wody
//     public float waveHeight = 0.1f; // Wysokość falowania wody

//     private MeshFilter meshFilter;
//     private MeshRenderer meshRenderer;

//     void Start()
//     {
//         CreateWater();
//     }

//     void Update()
//     {
//         AnimateWater();
//     }

//     void CreateWater()
//     {
//         // Tworzenie siatki wody
//         meshFilter = gameObject.AddComponent<MeshFilter>();
//         meshRenderer = gameObject.AddComponent<MeshRenderer>();

//         // Tworzenie siatki
//         Mesh mesh = new Mesh();
//         mesh.name = "WaterMesh";

//         // Tworzenie wierzchołków
//         Vector3[] vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
//         Vector2[] uv = new Vector2[vertices.Length];

//         for (int i = 0, y = 0; y <= gridSize; y++)
//         {
//             for (int x = 0; x <= gridSize; x++, i++)
//             {
//                 vertices[i] = new Vector3(x, 0, y);
//                 uv[i] = new Vector2((float)x / gridSize, (float)y / gridSize);
//             }
//         }

//         mesh.vertices = vertices;
//         mesh.uv = uv;

//         // Tworzenie trójkątów
//         int[] triangles = new int[gridSize * gridSize * 6];
//         for (int ti = 0, vi = 0, y = 0; y < gridSize; y++, vi++)
//         {
//             for (int x = 0; x < gridSize; x++, ti += 6, vi++)
//             {
//                 triangles[ti] = vi;
//                 triangles[ti + 3] = triangles[ti + 2] = vi + 1;
//                 triangles[ti + 4] = triangles[ti + 1] = vi + gridSize + 1;
//                 triangles[ti + 5] = vi + gridSize + 2;
//             }
//         }

//         mesh.triangles = triangles;
//         mesh.RecalculateNormals();

//         // Ustawienie siatki wody w komponencie MeshFilter
//         meshFilter.mesh = mesh;

//         // Ustawienie materiału
//         if (waterMaterial != null)
//             meshRenderer.material = waterMaterial;
//     }

//     void AnimateWater()
//     {
//         Mesh mesh = meshFilter.mesh;
//         Vector3[] vertices = mesh.vertices;

//         // Animacja falowania wody
//         float time = Time.time * waveSpeed;
//         for (int i = 0; i < vertices.Length; i++)
//         {
//             vertices[i].y = Mathf.Sin(vertices[i].x * 0.5f + time) * waveHeight + Mathf.Sin(vertices[i].z * 0.5f + time) * waveHeight;
//         }

//         mesh.vertices = vertices;
//         mesh.RecalculateNormals();
//     }
// }