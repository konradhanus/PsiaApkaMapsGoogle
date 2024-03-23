using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;
    public bool isMovingToTop = true;
    private GameObject topEdge;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    private bool goingUp = true;
    public float floatRate;
    private float floatTimer;
   
    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

    public float leftBoundary = -5f;
    public float rightBoundary = 5f;
    public float speed = 2.0f; // Prędkość poruszania się obiektu

 
    // Use this for initialization
    void Start () {
        // Określ pozycję krawędzi górnej ekranu w przestrzeni świata

        topEdge = GameObject.Find("TopEdge");
    }

    // Metoda do zniszczenia obiektu z efektem pękania bańki
    private void DestroyBubble()
    {
        // Tutaj możesz dodać efekt pękania bańki
        // Na przykład animację, efekt dźwiękowy itp.

        // Zniszcz obiekt
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {



        if (isAnimated)
        {
            if (isRotating)
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }

            if (isFloating)
            {
                floatTimer += Time.deltaTime;

                // Ustal wektor ruchu na podstawie aktualnej pozycji obiektu
                Vector3 moveDir = new Vector3(floatSpeed, 0.0f, 0.0f); // Zmiana na osią X
                if (transform.position.y < leftBoundary)
                    moveDir.y = floatSpeed;
                else if (transform.position.y > rightBoundary)
                    moveDir.y = -floatSpeed;

                // Wykonaj translację obiektu
                transform.Translate(moveDir * Time.deltaTime);

                // Sprawdź, czy obiekt powinien zmienić kierunek ruchu
                if (goingUp && floatTimer >= floatRate)
                {
                    goingUp = false;
                    floatTimer = 0;
                    floatSpeed = -floatSpeed;
                }
                else if (!goingUp && floatTimer >= floatRate)
                {
                    goingUp = true;
                    floatTimer = 0;
                    floatSpeed = +floatSpeed;
                }
            }


            if (isMovingToTop)
            {
                // Pobierz lokalną przestrzeń osi Y w oparciu o aktualną rotację obiektu
                Vector3 localUp = transform.TransformDirection(Vector3.forward);

                // Przesuń obiekt wzdłuż lokalnej przestrzeni osi Y
                transform.position += localUp * speed * Time.deltaTime;

                float topEdgePositionY = topEdge.transform.position.y;
                // Sprawdź, czy obiekt przekroczył górną krawędź ekranu
                if (transform.position.y >= topEdgePositionY)
                {
                    // Zniszcz obiekt
                    Destroy(gameObject);
                }
            }

            if (isScaling)
            {
                scaleTimer += Time.deltaTime;

                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else if (!scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }

                if (scaleTimer >= scaleRate)
                {
                    if (scalingUp) { scalingUp = false; }
                    else if (!scalingUp) { scalingUp = true; }
                    scaleTimer = 0;
                }
            }
        }
	}
}
