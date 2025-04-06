using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceMonitor : MonoBehaviour {
    public GameObject[] countDownItems;
    CheckpointManager[] carsCPM;
    public static bool racing = false;
    public static int totalLaps = 1;
    public GameObject gameOverPanel;
    public GameObject HUD;

    // Start is called before the first frame update
    void Start() {
        foreach (GameObject g in countDownItems)
            g.SetActive(false);

        StartCoroutine(PlayCountDown());
        gameOverPanel.SetActive(false);

        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        carsCPM = new CheckpointManager[cars.Length];
        for (int i = 0; i < cars.Length; i++)
            carsCPM[i] = cars[i].GetComponent<CheckpointManager>();
    }

    IEnumerator PlayCountDown() {
        yield return new WaitForSeconds(2);
        foreach (GameObject g in countDownItems) {
            g.SetActive(true);
            yield return new WaitForSeconds(1);
            g.SetActive(false);
        }
        racing = true;
    }

    public void RestartLevel() {
        racing = false;
        SceneManager.LoadScene("Track1");
    }


    void LateUpdate() {
        int finishedCount = 0;
        foreach (CheckpointManager cpm in carsCPM) {
            if (cpm.lap == totalLaps + 1)
                finishedCount++;
        }
        if (finishedCount == carsCPM.Length) {
            HUD.SetActive(false);
            gameOverPanel.SetActive(true);
        }
    }
}
