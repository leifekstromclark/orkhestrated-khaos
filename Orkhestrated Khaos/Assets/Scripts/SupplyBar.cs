using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupplyBar : MonoBehaviour
{

    private int length = 10;

    private RawImage[] supplies;

    // Start is called before the first frame update
    void Start()
    {
        supplies = new RawImage[length];
        for (int i = 0; i < supplies.Length; i++) {
            supplies[i] = (Instantiate(Resources.Load("Supply"), transform) as GameObject).GetComponent<RawImage>();
            supplies[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 10 + i * 25);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void set_value(int total, int upkeep) {
        for (int i = 0; i < supplies.Length; i++) {
            if (i < total - upkeep) {
                supplies[i].gameObject.SetActive(true);
                supplies[i].color = Color.magenta;
            }
            else if (i < total) {
                supplies[i].gameObject.SetActive(true);
                supplies[i].color = Color.grey;
            }
            else {
                supplies[i].gameObject.SetActive(false);
            }
        }
    }
}
