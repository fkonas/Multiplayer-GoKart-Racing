using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject explode;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "car")
        {
            GameObject c = Instantiate(explode);
            c.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
