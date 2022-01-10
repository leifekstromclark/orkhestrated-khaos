using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HandManager : MonoBehaviour
{
    public Transform Card_prefab, Orc_prefab;
    private List<int> occupied_positions; float spacing = 2.3F;
    // Start is called before the first frame update
    public GameObject Board_game_object;
    void SpawnCard(){
        Instantiate(Card_prefab, transform);
    }
    void CenterCards(){
        // Centers cards on the middle of your hand.
    }
    void Start()
    {
        // TEST: Spawn 3 blank cards at the start
        for (int i = 0; i < 3; i++) SpawnCard();
        for (int i=0; i<transform.childCount; i++){
            Transform card = this.gameObject.transform.GetChild(i);
            card.Translate(new Vector3(i*spacing, 0F, 0F));
        }
        CenterCards();

    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetMouseButtonDown(0)){
            Ray mouseray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Debug.Log(Input.mousePosition);
            if (Physics.Raycast(mouseray, out hit)){
                //if (hit.transform.name == "in_hand_card"){
                Destroy(hit.collider.gameObject);
                Instantiate(Orc_prefab, Board_game_object.transform);
                
                Debug.Log("This hit at " + hit.point);
            }
        }

        }
    }
