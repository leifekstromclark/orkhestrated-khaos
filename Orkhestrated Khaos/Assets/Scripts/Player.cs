using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public List<int> deck = new List<int>();
    public List<DeckListEntry> deck_list = new List<DeckListEntry>() {
        new DeckListEntry("Ork", ""),
        new DeckListEntry("Ork", ""),
        new DeckListEntry("Ork", ""),
        new DeckListEntry("Ork", ""),
        new DeckListEntry("Ork", "")
    };

    public List<Unit> hand = new List<Unit>();

    public int max_health = 20;

    public int health;

    public Bar bar;

    
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
}

public class DeckListEntry
{
    public string creature;
    public string weapon;
    
    public DeckListEntry(string creature, string weapon) {
        this.creature = creature;
        this.weapon = weapon;
    }
}