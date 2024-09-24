using UnityEngine;

[ExecuteInEditMode]
public class SceneLightingSetup : MonoBehaviour
{
    void Start()
    {
        // Sprawdź, czy światła już nie istnieją, aby uniknąć wielokrotnego dodawania
        if (transform.Find("Directional Light") == null)
        {
            // 1. Ustawienie Directional Light (symulacja słońca)
            GameObject directionalLight = new GameObject("Directional Light");
            directionalLight.transform.SetParent(this.transform); // Ustawienie jako child
            Light dirLight = directionalLight.AddComponent<Light>();
            dirLight.type = LightType.Directional;
            dirLight.color = new Color(1f, 0.956f, 0.839f); // Ciepłe, słoneczne światło
            dirLight.intensity = 1.2f; // Można dostosować
            directionalLight.transform.rotation = Quaternion.Euler(50, 30, 0); // Rotacja, aby symulować słońce
        }

        // 2. Ustawienie ambient light (oświetlenie otoczenia)
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.5f, 0.6f, 0.7f); // Niebieskawy kolor nieba
        RenderSettings.ambientEquatorColor = new Color(0.4f, 0.5f, 0.6f); // Pośrednie światło
        RenderSettings.ambientGroundColor = new Color(0.2f, 0.25f, 0.3f); // Kolor dla ciemniejszych miejsc

        // 3. Dodanie punktowych świateł (Point Lights) na kluczowych pozycjach
        if (transform.Find("Point Light (Right)") == null)
        {
            CreatePointLight(new Vector3(10, 5, 10), 0.8f, Color.white, "Point Light (Right)"); // Światło z prawej
        }
        if (transform.Find("Point Light (Left)") == null)
        {
            CreatePointLight(new Vector3(-10, 5, 10), 0.8f, Color.white, "Point Light (Left)"); // Światło z lewej
        }
        if (transform.Find("Point Light (Back)") == null)
        {
            CreatePointLight(new Vector3(0, 5, -10), 0.8f, Color.white, "Point Light (Back)"); // Światło od tyłu
        }

        // 4. Włączenie Global Illumination (Realtime GI)
        DynamicGI.UpdateEnvironment();
    }

    // Funkcja do tworzenia punktowego światła
    void CreatePointLight(Vector3 position, float intensity, Color color, string lightName)
    {
        GameObject pointLightObj = new GameObject(lightName);
        pointLightObj.transform.SetParent(this.transform); // Ustawienie jako child
        pointLightObj.transform.position = position;
        Light pointLight = pointLightObj.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.range = 15f; // Zasięg światła
        pointLight.intensity = intensity;
        pointLight.color = color;
        pointLight.shadows = LightShadows.Soft; // Ustawienie miękkich cieni
    }
}
