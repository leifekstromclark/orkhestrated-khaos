using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    //THIS CLASS WILL PROBABLY GET SPLIT INTO MULTIPLE IN THE FUTURE

    public float projection_height = 3.01f;
    public float base_width = 7.936f;
    public float top_width = 6.806f;

    private float[] row_bounds = new float[2];

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

    public Player[] players = new Player[2];

    public PolygonCollider2D poly_collider;

    public bool turn = true;

    public string[][][] valid_locations;

    //THIS IS HERE FOR NOW THOUGH I MAY MOVE IT
    public Vector3 mouse_position = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        poly_collider = GetComponent<PolygonCollider2D>();
        setup_board();
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

    //precompute various board related data
    public void setup_board() {

        row_bounds[0] = get_y(1f/3f);
        row_bounds[1] = get_y(2f/3f);

        for (int row = 0; row < 3; row++) {

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
    public int[] mouse_to_board_pos(Vector3 mouse_position) {
        //WATCH OUT FOR 7 COL INDICES THIS IS NOT ACCOUNTED FOR AND COULD BE A PROBLEM (I DONT THINK IT IS RN THO)
        int[] board_pos = new int[2];
        float y_pos = (mouse_position.y - transform.position.y) / transform.lossyScale.y + projection_height / 2f;
        float width = get_width(y_pos);
        if (y_pos < row_bounds[0]) {
            board_pos[0] = 2;
        }
        else if (y_pos < row_bounds[1]) {
            board_pos[0] = 1;
        }
        else {
            board_pos[0] = 0;
        }
        float x_pos = (mouse_position.x - transform.position.x) / transform.lossyScale.x + width / 2f;
        board_pos[1] = (int)Math.Floor(x_pos / (width / 7f));
        return board_pos;
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

    public void reverse_board()
    {
        foreach (Unit[] row in board) {
            Array.Reverse(row);
        }
    }
    /*
    public void combat()
    {
        if (turn) {
            reverse_board();
        }

        foreach (Unit[] row in board) {
            for (int i=1; i < row.Length; i++) {
                Unit unit = row[i];
                if (unit && unit.allegiance == turn) {
                    Unit target_unit = null;
                    Player target_player = null;
                    Unit attacker;
                    int moves = unit.speed;
                    int attacks = unit.attacks;
                    int embarked_attacks = 0;
                    if (unit is Vehicle && unit.embarked) {
                        embarked_attacks = unit.embarked.attacks;
                    }
                    while (moves > 0 || target_unit || target_player) {

                        target_unit = null;
                        target_player = null;
                        attacker = null;

                        // Assign unit as attacker if able
                        if (attacks > 0) {
                            attacker = unit;
                        }

                        // Assign embarked unit as attacker if able
                        else if (embarked_attacks > 0) {
                            attacker = unit.embarked;
                        }

                        // If attacking then get a target
                        if (attacker) {
                            for (int r=1; r <= attacker.range; r++) {
                                if (row[i-r]) {
                                    target_unit = row[i-r];
                                    break;
                                }
                                else if (i - r == 0) {
                                    target_player = players[turn ? 1 : 0];
                                    break;
                                }
                            }
                        }

                        // If found target unit then attack
                        if (target_unit) {
                            // Insert code for attacking
                            if (attacker == unit) {
                                attacks -= 1;
                            }
                            else {
                                embarked_attacks -= 1;
                            }
                        }

                        // If found target player then attack
                        if (target_player) {
                            // Insert code for attacking
                            if (attacker == unit) {
                                attacks -= 1;
                            }
                            else {
                                embarked_attacks -= 1;
                            }
                        }

                        // Otherwise move if able
                        else if (moves > 0) {
                            // Insert code for moving
                            // dont move into enemy spawn zone
                            moves -= 1;
                        }

                    }
                }
            }
        }

        if (turn) {
            reverse_board();
        }
    }
    */
}
