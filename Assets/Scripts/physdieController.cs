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
    public int curside = 0;

    public bool debug;

    void Start() {
        gameObject.SetActive(false);
        transform.localPosition = new Vector3(0, 0, -1);
        if (debug) {
            roll(0);
        }
    }

    void Update() {
        if (rb.IsSleeping()) {
            findSide();
            Start();
        }
    }

    private void findSide() {

        Vector3[] sidelist = {
                                  transform.forward,
                                  transform.right,
                                  -transform.forward,
                                  -transform.up,
                                  -transform.right,
                                  transform.up,
                                  };

        int h = 0;
        for (int i = 0; i < 6; i++) {
            h = i;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, sidelist[i], out hit, 0.6f)) {
                break;
            }
        }
        h += 1;
        print(h);
        curside = h;
    }

    public void roll(int mode) {
        rb.WakeUp();
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

    public bool isRolling() {
        return (!rb.IsSleeping());
    }
}
