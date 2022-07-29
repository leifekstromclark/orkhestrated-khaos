using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public string buff_name;
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

    public virtual List<object> get_stats() {
        return null;
    }

    public AttachmentState get_state() {
        return new AttachmentState(buff_name, get_stats());
    }
}
