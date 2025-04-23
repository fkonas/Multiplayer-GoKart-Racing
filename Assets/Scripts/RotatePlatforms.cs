using UnityEngine;

public class RotatePlatforms : MonoBehaviour {

    float speed = 50.0f;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

        this.transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
