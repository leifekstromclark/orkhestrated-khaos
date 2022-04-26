using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{

    public Unit host;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void expunge() {
        // REMOVE THE DEBUFF FROM GAME, UNIT, AND EVENT HANDLER
        host.buffs.Remove(this);
        Destroy(gameObject);
    }
}
