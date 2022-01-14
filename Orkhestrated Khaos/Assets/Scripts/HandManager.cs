using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HandManager : MonoBehaviour
{
    public Transform Card_prefab;
    private Dictionary<Transform, bool> cards = new Dictionary<Transform, bool>(); 
    float spacing = 2.3F;
    private Transform dragged_card; private bool am_dragging = false;
    // Start is called before the first frame update
    public GameObject Board_game_object; BoardManager board_script;
    void SpawnCard(){
        Instantiate(Card_prefab, transform);
    }
    void CenterCards(){
        // Centers cards on the middle of your hand.
        // Run after every update.
        int i = 0;
        foreach(KeyValuePair<Transform, bool> card in cards){
            if (card.Value) {
                card.Key.localPosition = new Vector3(-(float)cards.Count / 2 + spacing * i, 1F, 0F);
            }
            else {
                card.Key.localPosition = new Vector3(-(float)cards.Count / 2 + spacing * i, 0F, 0F);
            }
            i++;
        }
    }
    void Start()
    {
        // TEST: Spawn 3 blank cards at the start
        for (int i = 0; i < 3; i++) SpawnCard();
        for (int i=0; i<transform.childCount; i++){
            Transform card = transform.GetChild(i);
            cards.Add(card, false);
        }
        CenterCards();
        board_script = Board_game_object.GetComponent<BoardManager>();


    }

    // Update is called once per frame
    void Update()
    {   


        if (am_dragging){
            Debug.Log("Dragging...");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            dragged_card.position = (ray.origin); 
            if (Input.GetMouseButtonUp(0)){
                am_dragging = false;
                board_script.spawn_orc(dragged_card);
                cards.Remove(dragged_card);
                Destroy(dragged_card.gameObject);
            }    
        }

        if (Input.GetMouseButtonDown(0)){
            Ray mouseray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log(mouseray); Debug.Log(cards.Count);
            if (Physics.Raycast(mouseray, out hit)){
                // Reset card selection values
                if (cards[hit.collider.gameObject.transform]) {

                    dragged_card = hit.collider.gameObject.transform; am_dragging = true;
                }
                else{
                    foreach (var key in new List<Transform>(cards.Keys)){
                        cards[key] = false;
                    }
                cards[hit.collider.gameObject.transform] = true; // Mark clicked card as selected
                }

                Debug.Log(hit.transform.name);
    
                Debug.Log("This hit at " + hit.point);
            }
            else {
                foreach (var key in new List<Transform>(cards.Keys)){
                    cards[key] = false;
                }
            }

        CenterCards();
        }

        }
    }
