using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public float spacing = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < units.Count; i++) {
            units[i].hand_loc = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
