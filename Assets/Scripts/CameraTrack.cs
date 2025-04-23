using UnityEngine;

public class CameraTrack : MonoBehaviour {
    public Transform[] target;

    private void Update() {

        transform.LookAt(target[SmoothFollow.index]);
    }

}
