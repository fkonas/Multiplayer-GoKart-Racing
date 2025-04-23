using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public int lap = 0;
    public int checkPoint = -1;
    public float timeEntered = 0.0f;
    int checkPointCount;
    int nextCheckPoint;
    public GameObject lastCP;

    void Start() {

        GameObject[] cps = GameObject.FindGameObjectsWithTag("checkpoint");
        checkPointCount = cps.Length;
        foreach (GameObject c in cps) {

            if (c.name == "0") {
                lastCP = c;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "checkpoint") {

            int thisCPNumber = int.Parse(other.gameObject.name);
            if (thisCPNumber == nextCheckPoint) {

                lastCP = other.gameObject;
                checkPoint = thisCPNumber;
                timeEntered = Time.time;
                if (checkPoint == 0) lap++;

                nextCheckPoint++;
                if (nextCheckPoint >= checkPointCount)
                    nextCheckPoint = 0;
            }
        }
    }
}
