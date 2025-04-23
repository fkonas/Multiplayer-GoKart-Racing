using UnityEngine;

public class FlipCar : MonoBehaviour {

    Rigidbody rb;
    float lastTimeChecked;

    void Start() {

        rb = this.GetComponent<Rigidbody>();
    }

    void RightCar() {

        this.transform.position += Vector3.up;
        this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
    }

    void Update() {

        if (transform.up.y > 0.5f || rb.linearVelocity.magnitude > 1.0f) {

            lastTimeChecked = Time.time;
        }

        if (Time.time > lastTimeChecked + 3.0f) {

            RightCar();
        }
    }
}
