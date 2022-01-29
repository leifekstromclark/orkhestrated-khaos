using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Unit[][] board = new Unit[3][] {
        new Unit[7],
        new Unit[7],
        new Unit[7]
    };

    public Vector3[][] board_positions =  new Vector3[3][] {
        new Vector3[7],
        new Vector3[7],
        new Vector3[7]
    };

    public Player[] players = new Player[2];

    public PolygonCollider2D poly_collider;

    public bool turn = true;

    public bool[][] valid_locations;

    //THIS IS HERE FOR NOW THOUGH I MAY MOVE IT
    public Vector3 mouse_position = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        poly_collider = GetComponent<PolygonCollider2D>();
        set_board_positions();
    }

    // Update is called once per frame
    void Update()
    {
        //THIS IS HERE FOR NOW THOUGH I MAY MOVE IT
        //convert screen mouse position to world mouse position
        mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //function to set corresponding transform positions for each board space
    public void set_board_positions() {
        for (int row = 0; row < 3; row++) {
            float y;
            if (row == 0) {
                y = 0.755f + (1.505f - 0.755f) / 8f;
            }
            else if (row == 1) {
                y = -0.265f + (0.755f + 0.265f) / 8f;
            }
            else {
                y = -1.505f + (-0.265f + 1.505f) / 8f;
            }
            
            for (int col = 0; col < 7; col++) {
                float width = 7.936f - (7.936f - 5.3f) * (y + 1.505f) / 3.01f;
                float x = width / 7f * (col - 3);
                board_positions[row][col] = transform.position + new Vector3(x * transform.lossyScale.x, y * transform.lossyScale.y, 0f);
            }
        }
    }

    //takes a position of the mouse in the world. returns a corresponding space on the board
    public int[] mouse_to_board_pos(Vector3 mouse_position) {
        //WATCH OUT FOR 7 COL INDICES THIS IS NOT ACCOUNTED FOR AND COULD BE A PROBLEM (I DONT THINK IT IS RN THO)
        int[] board_pos = new int[2];
        float y_pos = (mouse_position.y - transform.position.y) / transform.lossyScale.y;
        float width = 7.936f - (7.936f - 5.3f) * (y_pos + 1.505f) / 3.01f;
        //NOTE y: -1.505, -0.265, 0.755, 1.505 x: 3.968 - 2.65 (edge / polygon collider is useful to measure)
        if (y_pos < -0.265f) {
            board_pos[0] = 2;
        }
        else if (y_pos < 0.755f) {
            board_pos[0] = 1;
        }
        else {
            board_pos[0] = 0;
        }
        float x_pos = (mouse_position.x - transform.position.x) / transform.lossyScale.x + width / 2f;
        board_pos[1] = (int)Math.Floor(x_pos / (width / 7f));
        return board_pos;
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
                if (unit is object && unit.allegiance == turn) {
                    Unit target_unit = null;
                    Player target_player = null;
                    Unit attacker;
                    int moves = unit.speed;
                    int attacks = unit.attacks;
                    int embarked_attacks = 0;
                    if (unit is Vehicle && unit.embarked is object) {
                        embarked_attacks = unit.embarked.attacks;
                    }
                    while (moves > 0 || target_unit is object || target_player is object) {

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
                        if (attacker is object) {
                            for (int r=1; r <= attacker.range; r++) {
                                if (row[i-r] is object) {
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
                        if (target_unit is object) {
                            // Insert code for attacking
                            if (attacker == unit) {
                                attacks -= 1;
                            }
                            else {
                                embarked_attacks -= 1;
                            }
                        }

                        // If found target player then attack
                        if (target_player is object) {
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
