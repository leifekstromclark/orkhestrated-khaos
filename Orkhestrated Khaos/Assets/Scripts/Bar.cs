using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{

    public Transform pivot;
    public Counter counter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void set_value(int max, int val) {
        counter.set_value(val);
        pivot.localScale = new Vector3(1, (float)val / max, 1);
    }
}
