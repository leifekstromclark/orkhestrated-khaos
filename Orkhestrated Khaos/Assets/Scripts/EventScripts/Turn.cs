using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : Event
{
    public bool turn;

    public Turn(bool turn)
    {
        this.turn = turn;
    }
}
