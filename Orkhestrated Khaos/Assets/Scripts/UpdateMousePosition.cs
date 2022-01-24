using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMousePosition : MonoBehaviour
{

    Vector3 mouse_position = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
