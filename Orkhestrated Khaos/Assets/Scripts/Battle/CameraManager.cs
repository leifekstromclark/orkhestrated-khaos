using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private int camera_location = 1;
    /* 1 - default (looking at board)
    2 - looking down at cards
    3 - looking up at all the cards in your deck (unimplemented)
    4 - looking to the left (board)
    5 - looking to the right (board)
    6 - looking to the left (while looking at cards)
    7 - looking to the right (while looking at cards)
    */

    private Dictionary<int, Vector3> location_to_coords = new Dictionary<int, Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        location_to_coords.Add(1, new Vector3(-10f,0f,0f));
        location_to_coords.Add(2, new Vector3(0f,0f,0f));
        location_to_coords.Add(3, new Vector3(-40f,0f,0f));
        location_to_coords.Add(4, new Vector3(-10f,-23f,5f));
        location_to_coords.Add(5, new Vector3(-10f,23f,-5f));
        location_to_coords.Add(6, new Vector3(0f,-24f,0f));
        location_to_coords.Add(7, new Vector3(0f,24f,0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown){

            
            if (Input.GetAxis("Horizontal") > 0){
                // look to the right of where you are
                if (camera_location == 1) camera_location = 5;
                else if (camera_location == 2) camera_location = 7;
                else if (camera_location == 4) camera_location = 1;
                else if (camera_location == 6) camera_location = 2;
            }
            if (Input.GetAxis("Horizontal") < 0){
                // look to the left of where you are
                if (camera_location == 1) camera_location = 4;
                else if (camera_location == 2) camera_location = 6;
                else if (camera_location == 5) camera_location = 1;
                else if (camera_location == 7) camera_location = 2;
            }
            if (Input.GetAxis("Vertical") > 0){
                // look up
                if (camera_location == 1) camera_location = 3;
                else if (camera_location == 2) camera_location = 1;
            }
            if (Input.GetAxis("Vertical") < 0){
                // look down
                if (camera_location == 3) camera_location = 1;
                else if (camera_location == 1) camera_location = 2;
            }
        transform.eulerAngles = location_to_coords[camera_location];
        // Debug.Log(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        // transform.Rotate(location_to_coords[camera_location] - new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Space.Self);
        }
    }
}
