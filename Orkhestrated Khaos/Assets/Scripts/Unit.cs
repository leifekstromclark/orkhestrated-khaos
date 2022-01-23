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
        if (Input.GetMouseButtonDown(0) && box_collider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
            mouseDown();
        }
        if (Input.GetMouseButtonUp(0) && box_collider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
            mouseUp();
        }

        if (pressed && !is_dragging && (Input.mousePosition - drag_start).magnitude > drag_tolerance) {
            is_dragging = true;
            beginDrag();
        }
        if (is_dragging) {
            drag();
        }
    }

    public void mouseDown() {
        pressed = true;
        drag_start = Input.mousePosition;
    }

    public void beginDrag() {
        if (in_play) {

        }
        else {
            hand.units.Remove(this);
        }
    }

    public void drag() {
        Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool inserting = hand.set_to_insert(mouse_position);
        if (!inserting) {
            if (board.poly_collider.OverlapPoint(mouse_position)) {
                int[] board_pos = board.mouse_to_board_pos(mouse_position);
                //make things light up and shit
            }
        }

        transform.position = mouse_position;
    }

    public void mouseUp() {
        Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (is_dragging) {
            if (in_play) {
                transform.position = board.board_positions[board_loc[0]][board_loc[1]];
            }
            else {
                if (hand.box_collider.OverlapPoint(mouse_position)) {
                    hand.units.Insert(hand.to_insert, this);
                }
                else if (board.poly_collider.OverlapPoint(mouse_position)) {
                    int[] board_pos = board.mouse_to_board_pos(mouse_position);
                    //prlly call place function from here
                    board.board[board_pos[0]][board_pos[1]] = this;
                    board_loc = board_pos;
                    in_play = true;
                    transform.position = board.board_positions[board_pos[0]][board_pos[1]];
                }
                else {
                    hand.units.Add(this);
                }
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
