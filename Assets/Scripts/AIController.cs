using UnityEngine;

public class AIController : MonoBehaviour
{

    public Circuit circuit;
    public float brakingSensitivity = 3f;
    Drive ds;
    public float steeringSensitivity = 0.01f;
    public float accelSensitivity = 0.3f;
    Vector3 target;
    Vector3 nextTarget;
    int currentWP = 0;
    float totalDistanceToTarget;


    void Start()
    {
        ds = this.GetComponent<Drive>();
        target = circuit.waypoints[currentWP].transform.position;
        nextTarget = circuit.waypoints[currentWP + 1].transform.position;
        totalDistanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);
    }

    bool isJump = false;

    void Update()
    {
        Vector3 localTarget = ds.rb.gameObject.transform.InverseTransformPoint(target);
        Vector3 nextLocalTarget = ds.rb.gameObject.transform.InverseTransformPoint(nextTarget);
        float distanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float nextTargetAngle = Mathf.Atan2(nextLocalTarget.x, nextLocalTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(ds.currentSpeed);

        float distanceFactor = distanceToTarget / totalDistanceToTarget;
        float speedFactor = ds.currentSpeed / ds.maxSpeed;

        float accel = Mathf.Lerp(accelSensitivity, 1, distanceFactor);
        float brake = Mathf.Lerp((-1 - Mathf.Abs(nextTargetAngle)) * brakingSensitivity, 1 + speedFactor, 1 - distanceFactor);

        if(Mathf.Abs(nextTargetAngle) > 20)
        {
            brake += 0.8f;
            accel -= 0.8f;
        }
        if (isJump)
        {
            accel = 1;
            brake = 0;
            Debug.Log("Jump");
        }

        //Debug.Log("Brake: " + brake + " Accel: " + accel + " Speed: " + ds.rb.angularVelocity.magnitude);

        //if (distanceToTarget < 5) { brake = 0.8f; accel = 0.1f; }

        ds.Go(accel, steer, brake);

        if (distanceToTarget < 4) //threshold, make larger if car starts to circle waypoints
        {
            currentWP++;
            if (currentWP >= circuit.waypoints.Length) 
                currentWP = 0;
            target = circuit.waypoints[currentWP].transform.position;

            if (currentWP >= circuit.waypoints.Length -1)
                nextTarget = circuit.waypoints[0].transform.position;
            else
                nextTarget = circuit.waypoints[currentWP + 1].transform.position;

            totalDistanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);

            if (ds.rb.gameObject.transform.InverseTransformPoint(target).y > 5)
            {
                isJump = true;
            }
            else isJump = false;
        }

        ds.CheckForSkid();
        ds.CalculateEngineSound();
    }
}
