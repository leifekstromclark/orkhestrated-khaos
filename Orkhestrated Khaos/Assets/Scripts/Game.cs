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

    public Player[] players = new Player[2];

    public bool turn = true;

    //might get rid of unity stuff and make it just a normal class. not sure yet (keeping just in case)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reverse_board()
    {
        foreach (Unit[] row in board) {
            Array.Reverse(row);
        }
    }

    public void combat()
    {
        if (turn) {
            reverse_board();
        }

        foreach (Unit[] row in board) {
            for (int i=1; i < row.Length; i++) {
                Unit unit = row[i];
                if (unit is object && unit.allegiance == turn) {
                    Unit target;
                    Unit attacker;
                    int moves = unit.speed;
                    int attacks = unit.attacks;
                    int embarked_attacks = 0;
                    if (unit is Vehicle && unit.embarked is object) {
                        embarked_attacks = unit.embarked.attacks;
                    }
                    while (moves > 0 || target is object) {

                        target = null;
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
                                    target = row[i-r];
                                    break;
                                }
                                else if (i - r == 0) {
                                    target = players[turn ? 1 : 0];
                                    break;
                                }
                            }
                        }

                        // If found target then attack
                        if (target is object) {
                            // Insert code for attacking
                            embarked_attacks -= 1;
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
}
