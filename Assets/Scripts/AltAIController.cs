using UnityEngine;

public class AltAIController : MonoBehaviour {

    public Circuit circuit;
    Vector3 target;
    int currentWP = 0;
    float speed = 20.0f;
    float accuracy = 1.0f;
    //float rotSpeed = 2.0f;

    void Start() {

        target = circuit.waypoints[currentWP].transform.position;
    }

    // Update is called once per frame
    void Update() {

        float distanceToTarget = Vector3.Distance(target, this.transform.position);
        Vector3 direction = target - this.transform.position;
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
        //    Quaternion.LookRotation(direction),
        //    Time.deltaTime * rotSpeed);

        this.transform.LookAt(target);
        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);

        if (distanceToTarget < accuracy) {

            currentWP++;
            if (currentWP >= circuit.waypoints.Length) {

                currentWP = 0;
            }

            target = circuit.waypoints[currentWP].transform.position;
        }
    }
}
