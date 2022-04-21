using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualStabbaz : Equipment
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void equip(Unit host) {
        this.host = host;
        this.host.attacks += 1;
        this.host.speed += 2;
        this.host.health += 2;
        this.host.max_health += 2;
    }
}