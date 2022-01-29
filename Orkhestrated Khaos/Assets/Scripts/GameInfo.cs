using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    public Dictionary<string, Type> types = new Dictionary<string, Type>()
    {
        {"ork", typeof(Ork)}
    };
}
