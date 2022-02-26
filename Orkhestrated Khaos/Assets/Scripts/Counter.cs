using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Counter.cs
A script managing text objects that changes their text based on calls.

*/
public class Counter : MonoBehaviour
{

    private TextMesh text_mesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Awake() {
        text_mesh = GetComponent<TextMesh>();
    }

    public void set_value(int val) {
        text_mesh.text = val.ToString();
    }
}