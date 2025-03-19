using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Drive ds;

    void Start()
    {
        ds = this.GetComponent <Drive> ();
    }

    void Update()
    {
        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");

        ds.Go(a, s, b);

        ds.CheckForSkid();
        ds.CalculateEngineSound();
    }
}
