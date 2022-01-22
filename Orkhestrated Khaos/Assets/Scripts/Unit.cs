using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Board board;
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
                if (float_pos < 0f) {
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
                if (board.poly_collider.OverlapPoint(mouse_position)) {
                    int[] board_pos = new int[2];
                    float y_pos = (mouse_position.y - board.transform.position.y) / board.transform.lossyScale.y;
                    float width = 7.936f - (7.936f - 5.3f) * (y_pos + 1.505f) / 3.01f;
                    //y: -1.505, -0.265, 0.755, 1.505 x: 3.968 - 2.65 (edge / polygon collider is useful to measure)
                    if (y_pos < -0.265f) {
                        board_pos[0] = 2;
                    }
                    else if (y_pos < 0.755f) {
                        board_pos[0] = 1;
                    }
                    else {
                        board_pos[0] = 0;
                    }
                    float x_pos = (mouse_position.x - board.transform.position.x) / board.transform.lossyScale.x + width / 2f;
                    board_pos[1] = (int)Math.Floor(x_pos / (width / 7f));
                    Debug.Log(board_pos[0]);
                    Debug.Log(board_pos[1]);
                }
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
