using UnityEngine;
using System.Collections;

public class AnimationScriptX : MonoBehaviour {

    public bool isAnimated = false;
    public GameObject plusOne;
    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;
    public bool isMovingToTop = true;
    private GameObject topEdge;

    public Vector3 rotationAngle;
    public float rotationSpeed;
    private bool isMoved = false;
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
        plusOne.SetActive(false);
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
        if(!isMoved){
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

                

                    float topEdgePositionZ = topEdge.transform.position.z;
                    // Sprawdź, czy obiekt przekroczył górną krawędź ekranu
                    if (transform.position.z >= topEdgePositionZ)
                    {
                        // Zniszcz obiekt
                        // Destroy(gameObject);
                        isAnimated = false;

                    }else{
                        // Przesuń obiekt wzdłuż lokalnej przestrzeni osi Y
                        transform.position += localUp * speed * Time.deltaTime;
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
            }else{
                ApplyEffect();
            }
        }
	}
    
    private void ApplyEffect()
    {
        plusOne.SetActive(true);
        isAnimated = false;
        
        Vector3 newPosition0 = new Vector3(1.9f, 5.0f, transform.position.z);
        Vector3 newPosition1 = new Vector3(1.9f, 4.2f, transform.position.z);
        Vector3 newPosition2 = new Vector3(1.9f, 3.4f, transform.position.z);
        Vector3 newPosition3 = new Vector3(1.9f, 2.6f, transform.position.z);
        Vector3 newPosition4 = new Vector3(1.9f, 1.8f, transform.position.z);
        Vector3 newPosition5 = new Vector3(1.9f, 1.0f, transform.position.z);
        Vector3 newPosition6 = new Vector3(1.9f, 0.2f, transform.position.z);

        Vector3[] positions = { newPosition0, newPosition1, newPosition2, newPosition3, newPosition4, newPosition5, newPosition6 };

        foreach (Vector3 position in positions)
        {
            if (!IsPositionOccupied(position))
            {
                transform.position = position;
                break;
            }
        }

        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, 0, currentRotation.z);
        isMoved = true;
        Destroy(gameObject, 2f);
    }

    private bool IsPositionOccupied(Vector3 position)
    {
       // Pobierz rodzica obiektu
       Transform parent = transform.parent;
       
       if (parent != null)
       {
            // Pobierz wszystkie dzieci rodzica, które mają tag "Gem"
            GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");

            // Dla każdego znalezionego obiektu "Gem"
            foreach (GameObject gem in gems)
            {
                // Jeśli obiekt "Gem" jest dzieckiem tego samego rodzica co obecny obiekt
                if (gem.transform.parent == parent)
                {
                    // Pobierz pozycję obiektu "Gem" i wyświetl ją
                    Vector3 gemPosition = gem.transform.position;
                    float threshold = 0.001f; // progiem tolerancji

                    if (Mathf.Abs(position.y - gemPosition.y) < threshold)
                    {
                        Debug.Log("Pozycja obiektu Gem: " + gemPosition + " object" + position);
                        return true;
                    }
                     Debug.Log("2Pozycja obiektu Gem: " + gemPosition + " object" + position);
                    
                }
            }
        }
        return false;
    }
    // Obsługa kliknięcia myszką
    private void OnMouseDown()
    {
        ApplyEffect();
    }

    // Obsługa kliknięcia palcem (dotyku)
    private void OnMouseOver()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ApplyEffect();
        }
    }
}
