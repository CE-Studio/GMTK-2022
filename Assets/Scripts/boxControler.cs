using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxControler : MonoBehaviour {

    public bool beingHeld = false;

    void Start() {
        transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f, Mathf.Floor(transform.localPosition.y) + 0.5f, transform.localPosition.z);
    }

    void Update() {
        if (beingHeld) {
            transform.localPosition = playerControler.thePlayer.transform.position + new Vector3(0, 0.5f, 0);
        } else {
            transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f, Mathf.Floor(transform.localPosition.y) + 0.5f, transform.localPosition.z);
        }
    }
}
