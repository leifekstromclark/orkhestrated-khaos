using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Unit host;

    public string equipment_name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void equip(Unit host) {
        this.host = host;
        this.host.equipment = this;
    }

    public virtual List<object> get_stats() {
        return null;
    }

    public AttachmentState get_state() {
        return new AttachmentState(equipment_name, get_stats());
    }
}
