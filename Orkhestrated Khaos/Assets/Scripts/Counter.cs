using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void pre_init() {
        text_mesh = GetComponent<TextMesh>();
    }

    public void set_value(int val) {
        text_mesh.text = val.ToString();
    }
}
