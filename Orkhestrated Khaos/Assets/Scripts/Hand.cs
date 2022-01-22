using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public float spacing;
    public int to_insert;
    public BoxCollider2D box_collider;

    // Start is called before the first frame update
    void Start()
    {
        box_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        arrange();
    }

    public void arrange()
    {
        int hand_length = units.Count;
        if (to_insert >= 0) {
            hand_length += 1;
        }
        int skip = 0;
        for (int i = 0; i < units.Count; i++) {
            if (i == to_insert) {
                skip = 1;
            }
            units[i].transform.position = transform.position + new Vector3(spacing * (i + skip) - ((float)hand_length - 1) * spacing / 2, 0, 0);
        }
    }
}
