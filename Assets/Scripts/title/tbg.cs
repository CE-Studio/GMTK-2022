using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tbg : MonoBehaviour {

    private float el;

    void Update() {
        el += Time.deltaTime;
        transform.position = new Vector3(Mathf.Sin(el / 30) * 10, 0, Mathf.Cos(el / 30) * 10);
    }
}
