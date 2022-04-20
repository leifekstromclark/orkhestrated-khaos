using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pike : Equipment, ReceivesEvents
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Event receive_event(Event data) {
        if (data is Attack) {
            if ((data as Attack).unit == host) {
                Buff restack;
                foreach (Buff buff in target.buffs) {
                    if (buff is Taunt) { //could be using keywords if want to restrict stacking of a group of buffs
                        restack = buff;
                    }
                }
                if (restack) {
                    restack.turns_remaining = 2;
                }
                else {
                    //add buff to target.buffs (instantiate with proper turns remaining (and probably some gameobjects for visuals))
                }
            }
        }
        return data;
    }
}
