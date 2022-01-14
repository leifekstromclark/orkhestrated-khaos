using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Transform Unit_prefab; private List<Transform> units = new List<Transform>();
    // Start is called before the first frame update
    public Dictionary<List<int>, Vector3> array = new Dictionary<List<int>, Vector3>();
    void Start()
    {
        array.Add(new List<int>{0,0}, new Vector3(-4.2f,1.9f,0f)); // 0,0
        array.Add(new List<int>{1,0}, new Vector3(-4.8f,0.4f,0f)); // 1,0
        array.Add(new List<int>{2,0}, new Vector3(-5.4f,-1.5f,0f)); // 2,0
        array.Add(new List<int>{0,1}, new Vector3(-2.9f,2.0f,0f)); // 0,1
        array.Add(new List<int>{1,1}, new Vector3(-3.1f,0.3f,0f)); // 1,1
        array.Add(new List<int>{2,1}, new Vector3(-3.7f,-1.3f,0f)); // 2,1
        array.Add(new List<int>{0,2}, new Vector3(-1.4f,1.9f,0f)); // 0,2
        array.Add(new List<int>{1,2}, new Vector3(-1.6f,0.4f,0f)); // 1,2
        array.Add(new List<int>{2,2}, new Vector3(-1.9f,-1.4f,0f));// 2,2
        array.Add(new List<int>{0,3}, new Vector3(-0.0f,1.8f,-0f));//0,3
        array.Add(new List<int>{1,3}, new Vector3(-0.0f,0.4f,-0f));//1,3
        array.Add(new List<int>{2,3}, new Vector3(-0.0f,-1.4f,0f));//2,3
        array.Add(new List<int>{0,4}, new Vector3(1.3f,2.9f,0f));//0,4
        array.Add(new List<int>{1,4}, new Vector3(1.5f,0.4f,0f));//1,4
        array.Add(new List<int>{2,4}, new Vector3(1.7f,-1.4f,0f));//2,4
        array.Add(new List<int>{0,5}, new Vector3(2.7f,1.9f,0f));//0,5
        array.Add(new List<int>{1,5}, new Vector3(3.1f,0.4f,0f));//1,5
        array.Add(new List<int>{2,5}, new Vector3(3.6f,-1.5f,0f));//2,5
        array.Add(new List<int>{0,6}, new Vector3(4.1f,1.9f,0f));//0,6
        array.Add(new List<int>{1,6}, new Vector3(4.5f,0.5f,0f));//1,6
        array.Add(new List<int>{2,6}, new Vector3(5.3f,-1.4f,0f));//2,6
    }

    Vector3 arr_to_board(List<int> li){
        return array[li];
    }
    List<int> get_closest_arr_slot(Vector3 x){
        float closest_distance = 100000f; List<int> bestpos = new List<int>();
        foreach (KeyValuePair<List<int>, Vector3> pos in array){
            if (Vector3.Distance(pos.Value, x) < closest_distance){
                closest_distance = Vector3.Distance(pos.Value, x);
                bestpos = pos.Key;
            }
        }
        return bestpos;
    }

    public void spawn_orc(Transform card){
        Instantiate(Unit_prefab, transform);
        Transform new_orc = transform.GetChild(transform.childCount - 1);
        units.Add(new_orc);
        List<int> closest_slot = get_closest_arr_slot(card.position);
        new_orc.localPosition = arr_to_board(closest_slot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
