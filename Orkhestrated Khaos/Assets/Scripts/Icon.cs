using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{

    private SpriteRenderer sprite_renderer;

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
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    public void set_icon(string type) {
        sprite_renderer.sprite = Resources.Load<Sprite>("BoardIcons/" + type);
    }
}