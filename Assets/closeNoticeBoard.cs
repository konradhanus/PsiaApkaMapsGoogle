using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeNoticeBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public void HideAllNoticeBoards()
    {
        // Znajdujemy wszystkie obiekty typu GameObject na scenie
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Przechodzimy przez wszystkie obiekty
        foreach (GameObject obj in allObjects)
        {
            Debug.Log("OBJEKT: "+obj.name);
            // Sprawdzamy, czy obiekt jest aktywny i ma nazwę "NoticeBoard"
            if (obj.name == "NoticeBoard")
            {
                // Wyłączamy obiekt
                obj.SetActive(false);
            }
        }
    }
}
