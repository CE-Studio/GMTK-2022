using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxControler : MonoBehaviour {

    public bool beingHeld = false;
    public GameObject shadow;

    void Start() {
        transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f, Mathf.Floor(transform.localPosition.y) + 0.5f, transform.localPosition.z);
        shadow = transform.GetChild(0).gameObject;
        boxManager.boxes.Add(this);
    }

    void Update() {
        if (beingHeld) {
            transform.localPosition = playerControler.thePlayer.transform.localPosition + new Vector3(0, 0.87f, 0);
            shadow.SetActive(false);
        } else {
            transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f, Mathf.Floor(transform.localPosition.y) + 0.5f, transform.localPosition.z);
            shadow.SetActive(true);
        }
    }
}
