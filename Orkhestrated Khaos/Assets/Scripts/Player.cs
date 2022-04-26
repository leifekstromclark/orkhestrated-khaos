using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public List<int> deck;
    public List<DeckListEntry> deck_list;

    public List<Unit> hand = new List<Unit>();

    public int max_health = 20;

    public int health;

    public int supply;

    public int upkeep;

    public int current_supply;

    public Bar bar;

    public SupplyBar supply_bar;

    
    /* RANDOM CODE I WANTED TO HANG ON TO
    var blockLookup = new Dictionary<string, Func<IBlock>>();
    blockLookup.Add("=", ()=> new BlockAir());
    blockLookup.Add("-", ()=> new BlockDirt());

    mapList[x,y] = blockLookup[symbol]();
    */

    //Resources.Load("PrefabName")

    // Start is called before the first frame update
    void Start()
    {
        set_health(max_health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void set_health(int val) {
        health = val;
        bar.set_value(max_health, health);
    }

    public void set_supply(int supply, int upkeep, int current) {
        this.current_supply = current;
        this.supply = supply;
        this.upkeep = upkeep;
        supply_bar.set_value(this.supply, this.upkeep, this.current_supply);
    }
}

public class DeckListEntry
{
    public string creature;
    public string equipment;
    
    public DeckListEntry(string creature, string equipment) {
        this.creature = creature;
        this.equipment = equipment;
    }
}