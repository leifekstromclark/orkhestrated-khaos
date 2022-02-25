using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Counter health_counter;
    private Counter power_counter;

    //This STUFF IS MOST LIKELY TEMPORARY
    private GameObject character;
    private SpriteRenderer character_renderer;


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
        character = transform.GetChild(0).gameObject;
        character_renderer = character.GetComponent<SpriteRenderer>();
        health_counter = (Instantiate(Resources.Load("Health"), transform) as GameObject).GetComponent<Counter>();
        power_counter = (Instantiate(Resources.Load("Power"), transform) as GameObject).GetComponent<Counter>();
        health_counter.set_value(health);
        power_counter.set_value(power);
        allegiance = true;

        if (!allegiance) {
            flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if dragging
        if (is_dragging) {
            drag();
        }
        //if drag is triggered
        if (pressed && !is_dragging && (Input.mousePosition - drag_start).magnitude > drag_tolerance) {
            is_dragging = true;
            begin_drag();
        }
        //if left mouse is down and over my collider
        if (Input.GetMouseButtonDown(0) && box_collider.OverlapPoint(game.mouse_position)) {
            mouse_down();
        }
        // if left mouse is up and over my collider
        if (Input.GetMouseButtonUp(0) && (box_collider.OverlapPoint(game.mouse_position) || is_dragging)) {
            mouse_up();
        }
    }

    //called when left mouse is pressed over unit's collider
    public void mouse_down() {
        //set boolean to signify that I have been pressed
        pressed = true;
        //assign screen coordinates for start of drag
        drag_start = Input.mousePosition;
    }

    //called when a drag is started
    public void begin_drag() {
        //if card is on board (swapping lanes)
        if (in_play) {
            
        }
        //if card is in hand (playing card)
        else {
            //get some valid placement locations for the board
            game.set_valid_locations(get_placement_locations());
            //remove card from hand list
            hand.units.Remove(this);
        }
    }

    //called each frame when dragging
    public void drag() {
        //ADD IN PLAY / NOT IN PLAY CONDITIONALS

        //tell hand to get a to_insert space (to achieve visual skipping / rearranging functionality)
        //returns true if one is needed. otherwise false (to_insert got set to -1)
        bool inserting = hand.set_to_insert(game.mouse_position);

        //if not moused over hand (essentially)
        if (!inserting) {
            //if moused over board
            if (game.poly_collider.OverlapPoint(game.mouse_position)) {
                //get the space that is moused over
                int[] board_pos = game.mouse_to_board_pos(game.mouse_position);
                //if the space is a valid space to place in
                if (game.valid_locations[board_pos[0]][board_pos[1]] != null) {
                    //MAKE THINGS LIGHT UP AND SHIT
                }
            }
        }

        //set transform to follow the mouse
        transform.position = game.mouse_position;
    }

    //called when a drag is finished
    public void end_drag() {
        //if in play (swapping)
        if (in_play) {
            //CALL SWAP FUNCTION HERE
            transform.position = game.board_positions[board_loc[0]][board_loc[1]] + new Vector3(0f, box_collider.size.y * transform.lossyScale.y / 2f, 0f);
        }
        //if not in play (playing from hand)
        else {
            bool placed = false;
            //if moused over hand
            if (hand.box_collider.OverlapPoint(game.mouse_position)) {
                //insert card into hand
                hand.units.Insert(hand.to_insert, this);
                placed = true;
            }
            //if moused over board
            else if (game.poly_collider.OverlapPoint(game.mouse_position)) {
                //get the space that is moused over
                int[] board_pos = game.mouse_to_board_pos(game.mouse_position);
                //if the space is a valid space to place in
                if (game.valid_locations[board_pos[0]][board_pos[1]] != null) {
                    place(board_pos);
                    placed = true;
                }
            }
            //if nothing valid was moused over
            if (!placed) {
                //append card to end of hand
                hand.units.Add(this);
            }
        }
        //clear valid locations
        game.set_valid_locations(null);
    }

    //called when the left mouse is depressed over unit's collider
    public void mouse_up() {
        //if dragging
        if (is_dragging) {
            end_drag();
        }
        //if I was just clicked normally
        else {
            //DISPLAY STATS
        }

        //set input related variables to default states
        is_dragging = false;
        pressed = false;
        hand.to_insert = -1;
    }

    public void receive_event(Event data) {

    }
    
    //gets valid placement (playing out of hand) locations on the board
    public string[][][] get_placement_locations() {
        //instantiate board-sized array of "false"
        string[][][] valid_locations = new string[3][][] {
            new string[7][],
            new string[7][],
            new string[7][]
        };
        //set placement column based on allegiance
        int col;
        if (allegiance) {
            col = 0;
        }
        else {
            col = 6;
        }
        //check if each space in the column is valid
        for (int row = 0; row < 3; row++) {
            if (!game.board[row][col]) {
                valid_locations[row][col] = new string[1]{"Place"};
            }
        }
        return valid_locations;
    }

    public void place(int[] position) {
        //insert card into board
        game.board[position[0]][position[1]] = this;
        //keep track of location on board
        board_loc = position;
        //card is now in play
        in_play = true;
        //set card to corresponding scale
        transform.localScale = new Vector3(game.row_scales[position[0]], game.row_scales[position[0]], 1f);
        character_renderer.sortingOrder = -3 + position[0];
        //move card to corresponding position
        transform.position = game.board_positions[position[0]][position[1]];
    }

    /*

    public string[][] get_swap_locations() {
        
    }

    public void swap(int[] position) {

    }
    */

    public void set_health(int val) {
        health = val;
        health_counter.set_value(val);
    }

    public void set_power(int val) {
        power = val;
        power_counter.set_value(val);
    }

    public void flip() {
        health_counter.transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1f);
        power_counter.transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1f);
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1f);
    }
}
