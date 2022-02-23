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

    public Vector3[][] board_positions = new Vector3[3][] {
        new Vector3[7],
        new Vector3[7],
        new Vector3[7]
    };

    public GameObject[][] board_highlights = new GameObject[3][] {
        new GameObject[7],
        new GameObject[7],
        new GameObject[7]
    };

    public List<Icon> board_icons = new List<Icon>();

    public float[] row_scales = new float[3];

    public Vector3[][] icon_positions = new Vector3[3][] {
        new Vector3[7],
        new Vector3[7],
        new Vector3[7]
    };

    public float[] icon_scales = new float[3];

    public Player[] players = new Player[2];

    public PolygonCollider2D poly_collider;

    public bool turn = true;

    public string[][] valid_locations;

    //THIS IS HERE FOR NOW THOUGH I MAY MOVE IT
    public Vector3 mouse_position = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        poly_collider = GetComponent<PolygonCollider2D>();
        set_positions();
    }

    // Update is called once per frame
    void Update()
    {
        //THIS IS HERE FOR NOW THOUGH I MAY MOVE IT
        //convert screen mouse position to world mouse position
        mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //precompute various board related data
    public void set_positions() {

        //NOTES: CHANGE THIS PROCESS TO BE DERIVED COMPLETELY FROM THE BOX COLLIDER
        //MAKE IT COMPLETELY MATHEMATICAL (NO MORE EYEBALLED Y VALUES) (NO MORE DIVIDING BY 8 CALCULATE EVERYTHING FROM PERSPECTIVE)
        //REMEMBER THAT ICONS ARE CURRENTLY INSTANTIATED (NOT PRELOADED) (THIS IS GOOD IF CHOICE BAD IF NO CHOICE)
        //GIVE THE BOARD LESS PERSPECTIVE (MORE BIRDSEYE)

        float unit_bot_width = 7.936f - (7.936f - 5.3f) * (-1.505f + (-0.265f + 1.505f) / 8f + 1.505f) / 3.01f;
        float icon_bot_width = 7.936f - (7.936f - 5.3f) * (-1.505f + (-0.265f + 1.505f) / 2f + 1.505f) / 3.01f;

        for (int row = 0; row < 3; row++) {
            float unit_y;
            float icon_y;
            if (row == 0) {
                unit_y = 0.755f + (1.505f - 0.755f) / 8f;
                icon_y = 0.755f + (1.505f - 0.755f) / 2f;
            }
            else if (row == 1) {
                unit_y = -0.265f + (0.755f + 0.265f) / 8f;
                icon_y = -0.265f + (0.755f + 0.265f) / 2f;
                
            }
            else {
                unit_y = -1.505f + (-0.265f + 1.505f) / 8f;
                icon_y = -1.505f + (-0.265f + 1.505f) / 2f;
            }

            float unit_width = 7.936f - (7.936f - 5.3f) * (unit_y + 1.505f) / 3.01f;
            float icon_width = 7.936f - (7.936f - 5.3f) * (icon_y + 1.505f) / 3.01f;

            row_scales[row] = unit_width / unit_bot_width;
            icon_scales[row] = icon_width / icon_bot_width;
            
            for (int col = 0; col < 7; col++) {
                float unit_x = unit_width / 7f * (col - 3);
                float icon_x = icon_width / 7f * (col - 3);
                board_positions[row][col] = transform.position + new Vector3(unit_x * transform.lossyScale.x, unit_y * transform.lossyScale.y, 0f);
                icon_positions[row][col] = transform.position + new Vector3(icon_x * transform.lossyScale.x, icon_y * transform.lossyScale.y, 0f);
                board_highlights[row][col] = transform.GetChild(row * 7 + col).gameObject;
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

    public void set_valid_locations(string[][] locations) {
        foreach (Icon icon in board_icons) {
            Destroy(icon.gameObject);
        }
        board_icons.Clear();

        valid_locations = locations;
        if (valid_locations == null) {
            foreach (GameObject[] row in board_highlights) {
                foreach (GameObject highlight in row) {
                    highlight.SetActive(false);
                }
            }
        }
        else {
            for (int row = 0; row < 3; row++) {
                for (int col = 0; col < 7; col++) {
                    if (valid_locations[row][col] != null) {
                        board_highlights[row][col].SetActive(true);
                        Icon icon = (Instantiate(Resources.Load("Icon"), icon_positions[row][col], Quaternion.identity) as GameObject).GetComponent<Icon>();
                        icon.gameObject.transform.localScale = new Vector3(icon_scales[row], icon_scales[row], 1f);
                        icon.pre_init();
                        icon.set_icon(valid_locations[row][col]);
                        board_icons.Add(icon);
                    }
                    else {
                        board_highlights[row][col].SetActive(false);
                    }
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
