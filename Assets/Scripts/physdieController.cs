using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physdieController : MonoBehaviour {

    public Material move;
    public Material rotate;
    public Material action;

    public Rigidbody rb;
    public MeshRenderer mr;

    public int curmode = 0;

    void Start() {
        roll(1);
    }

    void Update() {
        
    }

    public void roll(int mode) {
        gameObject.SetActive(true);
        curmode = mode;
        switch (mode) {
            default:
                mr.material = move;
                break;
            case 1:
                mr.material = rotate;
                break;
            case 2:
                mr.material = action;
                break;
        }
        transform.localPosition = new Vector3(0, 0, 1);
        rb.AddTorque(new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f)));
        rb.AddForce(new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f)));
    }
}
