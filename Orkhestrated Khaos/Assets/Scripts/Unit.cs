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
    public int hand_loc;
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
        }
        if (is_dragging) {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouse_position;
        }
        else {
            position();
        }
    }

    public void position()
    {
        if (in_play) {

        }
        else {
            int hand_length = hand.units.Count;
            float width = box_collider.size.x * transform.lossyScale.x;
            transform.position = hand.transform.position + new Vector3((width + hand.spacing) * hand_loc - ((float)hand_length - 1) * (width + hand.spacing) / 2, 0, 0);
        }
    }

    public void OnMouseDown() {
        pressed = true;
        drag_start = Input.mousePosition;
    }

    public void OnMouseUp() {
        if (is_dragging) {
            //collider2D.OverLapPoint(mouse_position)
        }
        else {
            //Display stats
        }
        is_dragging = false;
        pressed = false;
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
