using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Game game;
    public int power;
    public int health;
    public int speed;
    public int range;
    public int attacks;
    public int cost;
    public int upkeep;
    public int loyalty;
    public bool allegiance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receive_event(Event data) {

    }

    public int[][] get_placement_locations(int position) {

    }

    public int[][] get_swap_locations(int[] position) {
        
    }

    public void place(int[] position) {

    }

    public void swap(int[] position) {

    }
}
