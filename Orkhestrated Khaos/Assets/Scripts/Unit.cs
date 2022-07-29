using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

/*
Unit.cs

- Defines behavior of individual units
Properties:
- Health, Power, In Play, Upkeep, Cost, Attacks

*/

/* To-do: 
    - Add comments to document variables below:
    - Support for neutral units (tentative)
    - action queue?
    - event priority
    - toggle states? goblin cannon, howitzer, low angle, unsieged
    - blob shadows
*/

/* Bugs
card drawing is breaking
make visuals a little nicer
read over taunt / anything that accesses it
read over swapping and placing
read over scaling
read over passing the turn (abdullah got skipped sadge)
*/

/*
funni ideas
units that break initiative / attack on enemy turns (idk how we would do this) (maybe with some sort of initiative order list + events) (habb yzo waits for no one)
unorthodox targetting
unorthodox movement
unorthodox tickers (soul harvesting)
only 10 dwarves left (all named)
jonah colins -> rat killer
voltron units
Mace Onn
abdullah's pins
python comments
portal masta
unit that leaves a clone of itself as it moves
treshtog
pirate expansion
dating minigame
ability that distracts last unit in row
recoil knockback man
goblin launcher
imbalance of information mechanic
kingpin vanessa ork
leroy jenkins
atco
jorker
attribute that saves string (some sort of dynamic keyword ability)
*/

public class Unit : MonoBehaviour
{

    private Counter health_counter;
    private Counter power_counter;


    private SortingGroup sorting_group;
    private Transform flip_container;

    public Game game;
    // Reference to game class -> Handles turn and board properties (tentative)

    public string unit_name;

    // Unit stats
    public int power;
    public int health;
    public int max_health;
    public int speed; // Squares/turn
    public int range;
    public int attacks; // attacks/turn
    public int cost;
    public int upkeep;
    //public int loyalty;
    public bool allegiance;

    public Equipment equipment;
    public List<Buff> buffs = new List<Buff>();

    // Tickers
    public int moves_remaining;
    public int attacks_remaining;
    
    // Unity properties
    public BoxCollider2D box_collider;
    private bool pressed;
    private Vector3 drag_start;
    private bool is_dragging;
    private int drag_tolerance = 20; // # of pixels dragged before drag starts
    

    public int[] board_loc; // 2d int -> [row, column]
    public bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        box_collider = GetComponent<BoxCollider2D>();
        sorting_group = GetComponent<SortingGroup>();

        flip_container = transform.Find("Flip");

        // Load health and power from resources file
        health_counter = (Instantiate(Resources.Load("Health"), flip_container) as GameObject).GetComponent<Counter>();
        power_counter = (Instantiate(Resources.Load("Power"), flip_container) as GameObject).GetComponent<Counter>();
        health_counter.set_value(health);
        power_counter.set_value(power);

        if (!allegiance) {
            flip();
        }
    }

    //TWEAK SORTING DETAILS LATER ONCE WE HAVE ACTUAL GRAPHICS (REMEMBER THAT SORTING GROUPS, CUSTOM PIVOTS AND SPRITE SORT POINTS ARE USEFUL AND THAT YOU CAN MANUALLY SET SORTING LAYER IN CODE)

    //CONSIDER MAKING POSITION UPDATE NOT EVENTBASED

    //DONT FORGET THAT CUSTOM AXIS Y IS ON

    // Update is called once per frame
    void Update()
    {
        //if dragging
        if (is_dragging) {
            drag();
        }
        //if drag is triggered
        if (game.accepting_input && game.turn == allegiance && pressed && !is_dragging && (Input.mousePosition - drag_start).magnitude > drag_tolerance) {
            begin_drag(); 
        }
        //if left mouse is down and over my collider
        if (Input.GetMouseButtonDown(0) && box_collider.OverlapPoint(game.mouse_position)) {
            mouse_down();
        }
        // if left mouse is up and over my collider
        if (Input.GetMouseButtonUp(0) && pressed) {
            mouse_up();
        }
    }

    public virtual List<object> get_stats() {
        return new List<object>(){power, health, max_health, speed, range, attacks, cost, upkeep};
    }

    public virtual void load_stats(List<object> stats) {
        power = (int)stats[0];
        health = (int)stats[1];
        max_health = (int)stats[2];
        speed = (int)stats[3];
        range = (int)stats[4];
        attacks = (int)stats[5];
        cost = (int)stats[6];
        upkeep = (int)stats[7];
    }

    public UnitState get_state() {
        int[] state_loc = null;
        if (board_loc != null) {
            state_loc = new int[2]{board_loc[0], board_loc[1]};
        }

        List<AttachmentState> buff_states = new List<AttachmentState>();

        foreach (Buff buff in buffs) {
            buff_states.Add(buff.get_state());
        }

        return new UnitState(unit_name, allegiance, state_loc, get_stats(), equipment.get_state(), buff_states);
    }

    public void load_state(UnitState state) {
        allegiance = state.allegiance;
        board_loc = null;
        if (state.board_loc != null) {
            board_loc = new int[2] {state.board_loc[0], state.board_loc[1]};
        }
        load_stats(state.stats);

        //load on equipment and buffs. give this whole system some love

    }

    //called when left mouse is pressed over unit's collider
    public void mouse_down() {
        //set boolean to signify that I have been pressed
        pressed = true;
        //assign screen coordinates for start of drag
        drag_start = Input.mousePosition;
    }

    //called when a drag input is detected
    public void begin_drag() {
        string[][][] valid_locations = null;

        //if card is on board (swapping lanes)
        if (board_loc != null) {
            //get some valid placement locations for the board
            valid_locations = get_swap_locations();
        }
        //if card is in hand (playing card)
        else if (game.players[allegiance ? 0 : 1].current_supply >= cost) {
            //get some valid placement locations for the board
            valid_locations = get_placement_locations();
            if (valid_locations != null) {
                //remove card from hand list
                game.players[allegiance ? 0 : 1].hand.Remove(this);
            }
        }
        if (valid_locations != null) {
            //send valid locations to game
            game.set_valid_locations(valid_locations); 
            //update my scale and layer
            transform.localScale = new Vector3(1f, 1f, 1f);
            if (!allegiance) {
                flip();
            }
            //set layer to default
            sorting_group.sortingLayerName = "Default";
            //begin the drag
            is_dragging = true;
        }
    }

    //called each frame when dragging
    public void drag() {
        //if playing a card from the hand
        if (board_loc == null) {
            //tell hand to get a to_insert space (to achieve visual skipping / rearranging functionality)
            game.card_bench.set_to_insert(game.mouse_position);
        }

        //if moused over board
        if (game.poly_collider.OverlapPoint(game.mouse_position)) {
            //get the space that is moused over
            (int[] board_pos, int aux) = game.mouse_to_board_pos(game.mouse_position);
            //if the space is a valid space to place in
            if (game.valid_locations[board_pos[0]][board_pos[1]] != null) {
                //MAKE THINGS LIGHT UP AND SHIT
            }
        }

        //set transform to follow the mouse
        transform.position = game.mouse_position;
    }

    //called when a drag is finished
    public void end_drag() {
        bool success = false;
        //if not in play (playing from hand) and moused over hand
        if (board_loc == null && game.card_bench.box_collider.OverlapPoint(game.mouse_position)) {
            //insert card into hand
            game.players[allegiance ? 0 : 1].hand.Insert(game.card_bench.to_insert, this);
            success = true;
        }
        //if moused over board
        else if (game.poly_collider.OverlapPoint(game.mouse_position)) {
            //get the space that is moused over
            (int[] board_pos, int aux) = game.mouse_to_board_pos(game.mouse_position);
            //if the space is a valid space to place or swap in
            if (game.valid_locations[board_pos[0]][board_pos[1]] != null) {
                string action;
                if (game.valid_locations[board_pos[0]][board_pos[1]].Length == 1) {
                    action = game.valid_locations[board_pos[0]][board_pos[1]][0];
                }
                else {
                    action = game.valid_locations[board_pos[0]][board_pos[1]][aux];
                }
                if (board_loc != null) {
                    swap(board_pos, action);
                }
                else {
                    place(board_pos, action);
                }
                success = true;
            }
        }
        //if nothing valid was moused over
        if (!success) {
            if (board_loc != null) {
                set_position(board_loc);
                //put unit back in the battlefield layer
                sorting_group.sortingLayerName = "Battlefield";
            }
            else {
                //append card to end of hand
                game.players[allegiance ? 0 : 1].hand.Add(this);
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
        game.card_bench.to_insert = -1;
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
        bool empty = true;
        //check if each space in the column is valid
        for (int row = 0; row < 3; row++) {
            if (!game.board[row][col]) {
                valid_locations[row][col] = new string[1]{"Place"};
                empty = false;
            }
            else if (game.board[row][col] is Vehicle && !(game.board[row][col] as Vehicle).embarked) {
                valid_locations[row][col] = new string[2]{"Place", "Embark"};
                empty = false;
            }
        }
        if (empty) {
            valid_locations = null;
        }
        return valid_locations;
    }

    public void place(int[] pos, string action) {
        //increment upkeep
        Player player = game.players[allegiance ? 0 : 1];
        player.set_supply(player.supply, player.upkeep + upkeep, player.current_supply - cost);

        if (action == "Place") {
            //insert card into board
            game.board[pos[0]][pos[1]] = this;
            //keep track of location on board
            board_loc = pos;
            //update transform
            set_position(pos);
            //shove me in the battlefield layer
            sorting_group.sortingLayerName = "Battlefield";
        }
        if (action == "Embark") {

        }
    }

    public string[][][] get_swap_locations() {
        //instantiate board-sized array of "null"
        string[][][] valid_locations = new string[3][][] {
            new string[7][],
            new string[7][],
            new string[7][]
        };

        int num_swaps = 0;
        // If middle row in column is empty or contains allied unit
        if (!game.board[1][board_loc[1]] || game.board[1][board_loc[1]].allegiance == allegiance) {
            //check if each space in the column is valid
            for (int row = 0; row < 3; row++) {
                if (row != board_loc[0] && (!game.board[row][board_loc[1]] || game.board[row][board_loc[1]].allegiance == allegiance)) {
                    if (game.board[row][board_loc[1]] is Vehicle && !(game.board[row][board_loc[1]] as Vehicle).embarked) {
                        valid_locations[row][board_loc[1]] = new string[2]{"Swap", "Embark"};
                    }
                    else {
                        valid_locations[row][board_loc[1]] = new string[1]{"Swap"};
                    }
                    num_swaps++;
                }
            }
        }

        GetSwap swaps = game.handler.trigger("GetSwap", new GetSwap(this, valid_locations, num_swaps)) as GetSwap;
        valid_locations = swaps.locations;
        num_swaps = swaps.num_locs;

        if (num_swaps == 0) {
            valid_locations = null;
        }

        return valid_locations;
    }

    public void swap(int[] pos, string action) {
        if (action == "Swap") {
            Unit other = game.board[pos[0]][pos[1]];
            //update other unit if there is one
            if (other != null) {
                //keep track of location on board
                other.board_loc = board_loc;
                //update transform
                other.set_position(board_loc);
            }
            //swap units
            game.board[board_loc[0]][board_loc[1]] = other;
            game.board[pos[0]][pos[1]] = this;
            //keep track of location on board
            board_loc = pos;
            //update transform
            set_position(pos);
            //shove me in the battlefield layer
            sorting_group.sortingLayerName = "Battlefield";
        }
        if (action == "Embark") {

        }
    }
    
    public int[] get_target() {
        if (attacks_remaining > 0) {
            int reverse = allegiance ? 1 : -1;
            for (int r=1; r <= range; r++) {
                int f_r = r * reverse;
                if ((game.board[board_loc[0]][board_loc[1] + f_r] && game.board[board_loc[0]][board_loc[1] + f_r].allegiance != allegiance) || board_loc[1] + f_r == 6 * (reverse + 1) / 2) {
                    return new int[2]{board_loc[0], board_loc[1] + f_r};
                }
            }
        }
        return null;
    }

    public void attack_unit(Unit target) {
        game.handler.trigger("Attack", new Attack(this, target));
        attacks_remaining--;
        target.set_health(target.health - power);
    }

    public void attack_player(Player player) {
        attacks_remaining--;
        player.set_health(player.health - power);
    }

    public int[] get_move() {
        int reverse = allegiance ? 1 : -1;
        for (int m=1; m <= moves_remaining; m++) {
            int f_m = m * reverse;
            Unit target = game.board[board_loc[0]][board_loc[1] + f_m];
            if (board_loc[1] + f_m == 6 * (reverse + 1) / 2 || (target && target.allegiance != allegiance)) {
                break;
            }
            if (!target) {
                return new int[2]{board_loc[0], board_loc[1] + f_m};
            }
        }
        return null;
    }

    public void move(int[] pos) {
        moves_remaining -= Math.Abs(pos[1] - board_loc[1]);

        game.board[pos[0]][pos[1]] = this;
        game.board[board_loc[0]][board_loc[1]] = null;

        board_loc = pos;

        set_position(pos);
    }

    public void reset_tickers() {
        moves_remaining = speed;
        attacks_remaining = attacks;
    }

    //CHANGE THIS FOR INTERPOLATION (ACCOMODATE INTERMEDIATE SCALES)
    public void set_position(int[] pos) {
        //set card to corresponding scale
        transform.localScale = new Vector3(game.row_scales[pos[0]], game.row_scales[pos[0]], 1f);
        if (!allegiance) {
            flip();
        }
        //move card to corresponding position
        transform.position = game.board_positions[pos[0]][pos[1]];
    }

    public void set_health(int val) {
        health = val;
        health_counter.set_value(val); // Updates counter
        if (health <= 0) {
            game.board[board_loc[0]][board_loc[1]] = null;
            game.players[allegiance ? 0 : 1].upkeep -= upkeep;
            if (equipment) {
                if (equipment is IReceiveEvents) {
                    (equipment as IReceiveEvents).unsubscribe();
                }
                Destroy(equipment);
            }

            while (buffs.Count > 0) {
                buffs[0].expunge();
            }
            
            Destroy(gameObject);
        }
    }

    public void set_power(int val) {
        power = val;
        power_counter.set_value(val); // Updates counter
    }

    public void flip() {
        // Flips unit
        health_counter.transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1f);
        power_counter.transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1f);
        flip_container.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1f);
    }
}
