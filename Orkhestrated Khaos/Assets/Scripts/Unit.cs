using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Game game;
    public int power;
    public int health;
    public int speed;
    public int range;
    public int attacks;
    public int cost;
    public int upkeep;
    public int loyalty;
    public bool allegiance;
    public Unit embarked;

    public BoxCollider2D box_collider;
    private bool pressed;
    private Vector3 drag_start;
    private bool is_dragging;
    private int drag_tolerance = 20;
    
    public bool in_play;
    public int[] board_loc;
    public Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        box_collider = GetComponent<BoxCollider2D>();
        in_play = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed && !is_dragging && (Input.mousePosition - drag_start).magnitude > drag_tolerance) {
            is_dragging = true;
            hand.units.Remove(this);
        }
        if (is_dragging) {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (hand.box_collider.OverlapPoint(mouse_position)) {
                float float_pos = (mouse_position.x - (hand.transform.position.x - ((float)hand.units.Count + 1) * hand.spacing / 2)) / hand.spacing;
                int int_pos;
                if (float_pos < 0) {
                    int_pos = 0;
                }
                else if  (float_pos > hand.units.Count) {
                    int_pos = hand.units.Count;
                }
                else {
                    int_pos = (int)Math.Floor(float_pos);
                }
                hand.to_insert = int_pos;
            }
            else {
                hand.to_insert = -1;
            }
            transform.position = mouse_position;
        }
    }

    public void OnMouseDown() {
        pressed = true;
        drag_start = Input.mousePosition;
    }

    public void OnMouseUp() {
        Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (is_dragging) {
            if (hand.box_collider.OverlapPoint(mouse_position)) {
                hand.units.Insert(hand.to_insert, this);
            }
            else {
                hand.units.Add(this);
            }
        }
        else {
            //Display stats
        }
        is_dragging = false;
        pressed = false;
        hand.to_insert = -1;
    }

    public void receive_event(Event data) {

    }
    /*
    public int[][] get_placement_locations(int position) {

    }

    public int[][] get_swap_locations(int[] position) {
        
    }

    public void place(int[] position) {

    }

    public void swap(int[] position) {

    }
    */
}
