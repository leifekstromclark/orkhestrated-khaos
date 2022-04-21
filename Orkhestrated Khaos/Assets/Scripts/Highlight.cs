using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{

    private SpriteRenderer sprite_renderer;
    private static Dictionary<string, Color> color_dict = new Dictionary<string, Color>(){
        ["Place"] = Color.blue,
        ["Swap"] = Color.green,
        ["Target"] = Color.red
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void assign_square(string type, int[] pos) {
        sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = Resources.Load<Sprite>("BoardHighlights/" + type + "Highlight" + pos[0].ToString() + pos[1].ToString());
    }

    public void set_color(string type) {
        sprite_renderer.color = color_dict[type];
    }
}