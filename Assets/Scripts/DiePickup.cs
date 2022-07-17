using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePickup : MonoBehaviour
{
    GameObject player;
    AudioClip pickupSfx;
    
    void Start()
    {
        player = GameObject.Find("Grid/player");
        pickupSfx = Resources.Load<AudioClip>("Sounds/PickupDie");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2Int.FloorToInt(player.transform.localPosition) == Vector2Int.FloorToInt(transform.localPosition)) {
            player.GetComponent<playerControler>().manager.maxRolls++;
            player.GetComponent<playerControler>().aplay(pickupSfx);
            Destroy(gameObject);
        }
    }
}
