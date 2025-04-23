using UnityEngine;

public class SelectCar : MonoBehaviour {

    public GameObject[] cars;
    int currentCar = 0;

    void Start() {

        if (PlayerPrefs.HasKey("PlayerCar")) {

            currentCar = PlayerPrefs.GetInt("PlayerCar");
        }
        this.transform.LookAt(cars[currentCar].transform.position);
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {

            currentCar++;
            if (currentCar > cars.Length - 1) {

                currentCar = 0;
            }
            PlayerPrefs.SetInt("PlayerCar", currentCar);
        }

        Quaternion lookDir = Quaternion.LookRotation(cars[currentCar].transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * 2);
    }
}
