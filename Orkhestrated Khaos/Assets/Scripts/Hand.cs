using System;
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
        //probably change hand to only rearrange at certain points
        arrange();
    }

    //sets a to_insert value based on which part of the hand is moused over during a placement drag
    public static bool set_to_insert(Vector3 mouse_position) {
        if (box_collider.OverlapPoint(mouse_position)) {
            float float_pos = (mouse_position.x - (transform.position.x - ((float)units.Count + 1) * spacing / 2)) / spacing;
            int int_pos;
            if (float_pos < 0f) {
                int_pos = 0;
            }
            else if  (float_pos > units.Count) {
                int_pos = units.Count;
            }
            else {
                int_pos = (int)Math.Floor(float_pos);
            }
            to_insert = int_pos;
            return true;
        }
        else {
            to_insert = -1;
            return false;
        }
    }

    //arranges cards in the hand list, skipping the to_insert space if one exists
    public static void arrange()
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
