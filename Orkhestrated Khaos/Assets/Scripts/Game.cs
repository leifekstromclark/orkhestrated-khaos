using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public List<DeckListEntry> player_one_deck = new List<DeckListEntry>() {
        new DeckListEntry("Ork", "Sticka"),
        new DeckListEntry("Ork", "Sticka"),
        new DeckListEntry("Ork", "Sticka"),
        new DeckListEntry("Ork", "VoodooBoosta"),
        new DeckListEntry("Ork", "VoodooBoosta"),
        new DeckListEntry("Ork", "VoodooBoosta"),
        new DeckListEntry("Ork", "VoodooBoosta")
    };
    public List<DeckListEntry> player_two_deck = new List<DeckListEntry>() {
        new DeckListEntry("Goblin", "Sticka"),
        new DeckListEntry("Goblin", "Sticka"),
        new DeckListEntry("Goblin", "Sticka"),
        new DeckListEntry("Goblin", "Sticka"),
        new DeckListEntry("Goblin", "Sticka"),
        new DeckListEntry("Goblin", "Sticka"),
        new DeckListEntry("Goblin", "Sticka")
    };

    //THIS CLASS WILL PROBABLY GET SPLIT INTO MULTIPLE IN THE FUTURE

    public float projection_height = 3.01f;
    public float base_width = 7.936f;
    public float top_width = 6.806f;

    private float[] row_bounds = new float[2];
    private float[] selector_bounds = new float[3];

    public Unit[][] board = new Unit[3][] {
        new Unit[7],
        new Unit[7],
        new Unit[7]
    };

    public Vector3[][] board_positions = new Vector3[3][] {
        new Vector3[7],
        new Vector3[7],
        new Vector3[7]
    };

    public float[] row_scales = new float[3];

    public Selector[][] selectors = new Selector[3][] {
        new Selector[7],
        new Selector[7],
        new Selector[7]
    };

    public int supply_cap = 10;

    public List<Unit> initiative;

    public Player[] players = new Player[2];

    public Hand hand;

    public AbilityHandler handler = new AbilityHandler();

    public PolygonCollider2D poly_collider;

    public bool turn = true;

    public bool accepting_input = true;

    public string[][][] valid_locations;

    //THIS IS HERE FOR NOW THOUGH I MAY MOVE IT
    public Vector3 mouse_position = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        poly_collider = GetComponent<PolygonCollider2D>();
        setup_board();

        foreach (Player player in players) {
            player.supply_bar.instantiate_supplies(supply_cap);
            player.set_supply(2, 0, 2);
        }

        players[0].deck_list = player_one_deck;
        players[1].deck_list = player_two_deck;

        players[0].deck = new List<int>();
        for (int i = 0; i < player_one_deck.Count; i++) {
            players[0].deck.Add(i);
        }
        players[1].deck = new List<int>();
        for (int i = 0; i < player_two_deck.Count; i++) {
            players[1].deck.Add(i);
        }

        for (int i = 0; i < 4; i++) {
            draw_card(true);
            draw_card(false);
        }

        hand.units = players[0].hand;

        foreach (Unit unit in players[1].hand) {
            unit.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //THIS IS HERE FOR NOW THOUGH I MAY MOVE IT
        //convert screen mouse position to world mouse position
        mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    public float get_y(float relative_height){
        return projection_height * relative_height / (1 - (base_width / top_width - 1) * (1 - relative_height));
    }

    public float get_width(float y) {
        return base_width - (base_width - top_width) * y / projection_height;
    }


    //OPTIMIZE THIS ONCE WE KNOW MORE ABOUT HOW THINGS ARE GONNA GET INSTANTIATED

    //precompute various board related data
    public void setup_board() {

        for (int i = 0; i < 2; i++) {
            row_bounds[i] = get_y((2-i)/3f);
        }

        for (int row = 0; row < 3; row++) {

            selector_bounds[row] = get_y((2 - row) * 1f/3f + 1f/6f);

            float y = get_y((2 - row) * 1f/3f + 1f/18f);
            float width = get_width(y);
            row_scales[row] = width / base_width;
            y -= projection_height / 2f;
            
            for (int col = 0; col < 7; col++) {
                float x = width / 7f * (col - 3);
                board_positions[row][col] = transform.position + new Vector3(x * transform.lossyScale.x, y * transform.lossyScale.y, 0);

                selectors[row][col] = (Instantiate(Resources.Load("Selector"), transform) as GameObject).GetComponent<Selector>();
                selectors[row][col].assign_square(new int[2]{row, col});
            }
        }
    }


    //takes a position of the mouse in the world. returns a corresponding space on the board
    public (int[], int) mouse_to_board_pos(Vector3 mouse_position) {
        //WATCH OUT FOR 7 COL INDICES THIS IS NOT ACCOUNTED FOR AND COULD BE A PROBLEM (I DONT THINK IT IS RN THO)
        int[] board_pos = new int[2];
        float y_pos = (mouse_position.y - transform.position.y) / transform.lossyScale.y + projection_height / 2f;
        float width = get_width(y_pos);
        int aux = 0; // represents whether you're moused over the lower or upper half of the square

        if (y_pos > row_bounds[0]) {
            board_pos[0] = 0;
            if (y_pos < selector_bounds[0]) aux = 1;
        }
        else if (y_pos > row_bounds[1]) {
            board_pos[0] = 1;
            if (y_pos < selector_bounds[1]) aux = 1;
        }
        else {
            board_pos[0] = 2;
            if (y_pos < selector_bounds[2]) aux = 1;
        }
        float x_pos = (mouse_position.x - transform.position.x) / transform.lossyScale.x + width / 2f;
        board_pos[1] = (int)Math.Floor(x_pos / (width / 7f));
        return (board_pos, aux);
    }

    public void set_valid_locations(string[][][] locations) {
        //OPTIMIZE THIS
        valid_locations = locations;
        if (valid_locations == null) {
            foreach (Selector[] row in selectors) {
                foreach (Selector selector in row) {
                    selector.set_status(null);
                }
            }
        }
        else {
            for (int row = 0; row < 3; row++) {
                for (int col = 0; col < 7; col++) {
                    selectors[row][col].set_status(valid_locations[row][col]);
                }
            }
        }
    }

    //FIX INDEX ERRORS WHEN DRAWING
    public void draw_card(bool allegiance)
    {
        Player player = players[allegiance ? 0 : 1];
        if (player.deck.Count > 0 && player.hand.Count < 6) {
            int rand = UnityEngine.Random.Range(0, player.deck.Count);
            int index = player.deck[rand];
            Unit unit = (Instantiate(Resources.Load("UnitPrefabs/" + player.deck_list[index].creature)) as GameObject).GetComponent<Unit>();
            unit.allegiance = allegiance;
            unit.game = this;
            unit.hand = hand;
            Equipment equipment = (Instantiate(Resources.Load("EquipmentPrefabs/" + player.deck_list[index].equipment), unit.transform) as GameObject).GetComponent<Equipment>();
            equipment.equip(unit);
            if (equipment is ReceivesEvents) {
                (equipment as ReceivesEvents).subscribe(handler);
            }
            player.hand.Add(unit);
            player.deck.RemoveAt(rand);
        }
    }

    public void pass_turn()
    {
        combat();
        Player player = players[turn ? 0 : 1];
        int new_supply = Math.Min(player.supply + 2, supply_cap);
        player.set_supply(new_supply, player.upkeep, new_supply - player.upkeep);
        handler.trigger("Turn", new Turn(turn));
        turn = !turn;
        foreach (Unit unit in hand.units) {
            unit.gameObject.SetActive(false);
        }

        player = players[turn ? 0 : 1];
        player.set_supply(player.supply, player.upkeep, player.supply - player.upkeep);
        hand.units = player.hand;
        foreach (Unit unit in hand.units) {
            unit.gameObject.SetActive(true);
        }
        draw_card(turn);
    }

    public void combat()
    {
        //DEBUFFS AND STATUS EFFECTS SHOULD BE HANDLED IN HERE OR A SIMILAR GLOBAL LAYER + EVENT HANDLER PRLLY PLAYS SOME ROLE WITH THIS STUFF (Maybe there could be some sort of way to check debuffs or status effects from the unit class hmmm)
        
        initiative = new List<Unit>();

        for (int col = 1; col < 7; col++) {
            for (int row = 0; row < 3; row++) {
                Unit unit = board[row][turn ? 6 - col : col];
                if (unit && unit.allegiance == turn) {
                    initiative.Add(unit);
                }
            }
        }

        foreach (Unit unit in initiative) {
            unit.reset_tickers();
            int[] target_loc = unit.get_target();
            int[] move_loc = unit.get_move();
            while (target_loc != null || move_loc != null) {
                if (target_loc != null) {
                    Unit target = board[target_loc[0]][target_loc[1]];
                    if (target) {
                        unit.attack_unit(target);
                    }
                    else if (target_loc[1] == (unit.allegiance ? 6 : 0)) {
                        unit.attack_player(players[unit.allegiance ? 1 : 0]);
                    }
                    //not sure what happens in edge cases but we will prlly have to account for them
                }
                else {
                    unit.move(move_loc);
                }
                target_loc = unit.get_target();
                move_loc = unit.get_move();
            }
            handler.trigger("Done", new Done(unit));
        }
    }
}
