using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
    public float outerDiameter = 5f; // Średnica zewnętrzna pierścienia
    public float innerDiameter = 5.1f; // Średnica wewnętrzna pierścienia
    public float lineWidth = 0.1f; // Grubość linii pierścienia

    private bool circleDrawn = false; // Flaga informująca, czy pierścień został już narysowany

    private void Start()
    {
        DrawRing();
        StartCoroutine(AnimateRing());
    }

    private void DrawRing()
    {
        // Tworzenie nowego obiektu pierścienia
        GameObject ring = new GameObject("Ring");
        ring.transform.parent = transform; // Ustawienie pierścienia jako dziecko tego GameObjectu
        ring.transform.localPosition = Vector3.zero; // Ustawienie lokalnej pozycji na (0,0,0)

        // Rysowanie zewnętrznego okręgu
        DrawCircleRing(ring.transform, outerDiameter, lineWidth, Color.white); // Zmiana: Przekazanie koloru jako parametr
    }

    // Metoda do rysowania okręgu jako pierścienia
    private void DrawCircleRing(Transform parent, float diameter, float width, Color color)
    {
        GameObject circle = new GameObject("Circle");
        circle.transform.parent = parent; // Ustawienie pierścienia jako dziecka obiektu nadrzędnego
        circle.transform.localPosition = new Vector3(0f, 0.3f, 0f); 

        LineRenderer lineRenderer = circle.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false; // Ustawienie przestrzeni lokalnej
        lineRenderer.positionCount = 101; // Liczba punktów (segmentów) okręgu

        // Ustawienie punktów okręgu
        for (int i = 0; i <= 100; i++)
        {
            float angle = Mathf.Deg2Rad * (i * 3.6f); // Każdy punkt okręgu ma 3.6 stopnia
            float x = Mathf.Sin(angle) * (diameter / 2f);
            float z = Mathf.Cos(angle) * (diameter / 2f);
            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }

        // Ustawienie właściwości linii
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.material.color = color; // Zmiana: Ustawienie koloru linii
    }

    // Metoda do animacji rozszerzania i zanikania pierścienia w pętli
    private System.Collections.IEnumerator AnimateRing()
    {
        GameObject ring = transform.Find("Ring").gameObject;
        LineRenderer lineRenderer = ring.GetComponentInChildren<LineRenderer>();

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0f); // Rozpoczęcie animacji
        curve.AddKey(7f, outerDiameter / 2f); // Maksymalne rozszerzenie się pierścienia

        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localScale.x", curve);
        clip.SetCurve("", typeof(Transform), "localScale.y", curve);
        clip.SetCurve("", typeof(Transform), "localScale.z", curve);

        Animation anim = ring.AddComponent<Animation>();
        anim.AddClip(clip, "RingAnimation");

        while (true)
        {
            anim.Play("RingAnimation");
            yield return new WaitForSeconds(0.5f); // Dodatkowe opóźnienie przed ponownym uruchomieniem animacji
        }
    }
}
