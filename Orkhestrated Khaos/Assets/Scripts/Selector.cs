using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{

    private Icon[] aux_icons = new Icon[2];
    private Icon base_icon;
    private Highlight[] aux_highlights = new Highlight[2];
    private Highlight base_highlight;
    private Game game;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Awake is called on instantiation
    void Awake()
    {
        for (int i = 0; i < 2; i++) {
            aux_icons[i] = transform.GetChild(i).gameObject.GetComponent<Icon>();
        }
        base_icon = transform.GetChild(2).gameObject.GetComponent<Icon>();
        for (int i = 0; i < 2; i++) {
            aux_highlights[i] = transform.GetChild(i+3).gameObject.GetComponent<Highlight>();
        }
        base_highlight = transform.GetChild(5).gameObject.GetComponent<Highlight>();
        game = transform.parent.gameObject.GetComponent<Game>();
        gameObject.SetActive(false);
    }

    public void assign_square(int[] pos) {
        base_highlight.assign_square("Base", pos);

        float y = game.get_y((2 - pos[0]) * 1f/3f + 1f/6f);
        float width = game.get_width(y);
        y -= game.projection_height / 2f;
        float x = width / 7f * (pos[1] - 3);
        base_icon.gameObject.transform.position = new Vector3(x * transform.parent.lossyScale.x, y * transform.parent.lossyScale.y, 0);
        base_icon.gameObject.transform.localScale = new Vector3(width / game.base_width / 3f, width / game.base_width / 3f, 1); //NOTE THAT 3 IS JUST SOME RANDOM CONSTANT I ADDED CUZ IT LOOKED NICE
        
        for (int i = 0; i < 2; i++) {
            string type;
            if (i == 0) {
                type = "Upper";
            }
            else {
                type = "Lower";
            }
            aux_highlights[i].assign_square(type, pos);

            y = game.get_y((2 - pos[0]) * 1f/3f + 1f/12f + i * 1f/6f);
            width = game.get_width(y);
            y -= game.projection_height / 2f;
            x = width / 7f * (pos[1] - 3);
            aux_icons[i].gameObject.transform.position = new Vector3(x * transform.parent.lossyScale.x, y * transform.parent.lossyScale.y, 0);
            aux_icons[i].gameObject.transform.localScale = new Vector3(width / game.base_width / 3f, width / game.base_width / 3f, 1); //NOTE THAT 3 IS JUST SOME RANDOM CONSTANT I ADDED CUZ IT LOOKED NICE
        }
    }

    public void set_status(string[] options) {
        if (options != null) {
            gameObject.SetActive(true);
            bool single;
            if (options.Length == 1) {
                single = true;
                base_icon.set_icon(options[0]);
                base_highlight.set_color(options[0]);
            }
            else {
                single = false;
                for (int i = 0; i < 2; i++) {
                    aux_icons[i].set_icon(options[i]);
                    aux_highlights[i].set_color(options[i]);
                }
            }
            base_icon.gameObject.SetActive(single);
            base_highlight.gameObject.SetActive(single);
            for (int i = 0; i < 2; i++) {
                aux_icons[i].gameObject.SetActive(!single);
                aux_highlights[i].gameObject.SetActive(!single);
            }
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
