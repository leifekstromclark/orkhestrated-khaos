using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupplyBar : MonoBehaviour
{

    private RawImage[] supplies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiate_supplies(int length) {
        supplies = new RawImage[length];
        for (int i = 0; i < supplies.Length; i++) {
            supplies[i] = (Instantiate(Resources.Load("Supply"), transform) as GameObject).GetComponent<RawImage>();
            supplies[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 10 + i * 25);
        }
    }

    public void set_value(int supply, int upkeep, int current) {
        for (int i = 0; i < supplies.Length; i++) {
            if (i < current) {
                supplies[i].gameObject.SetActive(true);
                supplies[i].color = Color.magenta;
            }
            else if (i < supply - upkeep) {
                supplies[i].gameObject.SetActive(true);
                supplies[i].color = Color.black;
            }
            else if (i < supply) {
                supplies[i].gameObject.SetActive(true);
                supplies[i].color = Color.grey;
            }
            else {
                supplies[i].gameObject.SetActive(false);
            }
        }
    }
}
