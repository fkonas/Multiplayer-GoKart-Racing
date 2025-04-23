using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceMonitor : MonoBehaviourPunCallbacks {

    public GameObject[] countdownItems;
    CheckpointManager[] carsCPM;

    public GameObject[] carPrefabs;
    public Transform[] spawnPos;

    public static bool racing = false;
    public static int totalLaps = 1;
    public GameObject gameOverPanel;
    public GameObject HUD;

    public GameObject startRace;
    public GameObject waitingText;

    int playerCar;

    void Start() {

        racing = false;

        foreach (GameObject g in countdownItems) {

            g.SetActive(false);
        }


        gameOverPanel.SetActive(false);

        startRace.SetActive(false);
        waitingText.SetActive(false);

        playerCar = PlayerPrefs.GetInt("PlayerCar");

        int randomStartPos = Random.Range(0, spawnPos.Length);
        Vector3 startPos = spawnPos[randomStartPos].position;
        Quaternion startRot = spawnPos[randomStartPos].rotation;
        GameObject pCar = null;

        if (PhotonNetwork.IsConnected) {

            startPos = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
            startRot = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1].rotation;

            if (NetworkedPlayer.localPlayerInstance == null) {

                pCar = PhotonNetwork.Instantiate(carPrefabs[playerCar].name, startPos, startRot, 0);
            }

            if (PhotonNetwork.IsMasterClient) {

                startRace.SetActive(true);
            } else {

                waitingText.SetActive(true);
            }
        } else {


            pCar = Instantiate(carPrefabs[playerCar]);
            pCar.transform.position = startPos;
            pCar.transform.rotation = startRot;

            foreach (Transform t in spawnPos) {

                if (t == spawnPos[randomStartPos]) continue;
                GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)]);
                car.transform.position = t.position;
                car.transform.rotation = t.rotation;
            }

            StartGame();
        }

        SmoothFollow.playerCar = pCar.gameObject.GetComponent<Drive>().rb.transform;
        pCar.GetComponent<AIController>().enabled = false;
        pCar.GetComponent<Drive>().enabled = true;
        pCar.GetComponent<PlayerController>().enabled = true;


    }

    public void BeginGame() {


        string[] aiNames = { "Adrian", "Lee", "Penny", "Merlin", "Tabytha", "Pauline", "John", "Kia", "Chloe", "Fiona", "Mathew" };

        int numAIPlayers = PhotonNetwork.CurrentRoom.MaxPlayers - PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < PhotonNetwork.CurrentRoom.MaxPlayers; ++i) {

            Vector3 startPos = spawnPos[i].position;
            Quaternion startRot = spawnPos[i].rotation;
            int r = Random.Range(0, carPrefabs.Length);

            object[] instanceData = new object[1];
            instanceData[0] = (string)aiNames[Random.Range(0, aiNames.Length)];

            GameObject AICar = PhotonNetwork.Instantiate(carPrefabs[r].name, startPos, startRot, 0, instanceData);
            AICar.GetComponent<AIController>().enabled = true;
            AICar.GetComponent<Drive>().enabled = true;
            AICar.GetComponent<Drive>().networkName = (string)instanceData[0];
            AICar.GetComponent<PlayerController>().enabled = false;
        }
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("StartGame", RpcTarget.All, null);
        }
        // StartGame();
    }

    [PunRPC]
    public void StartGame() {

        StartCoroutine(PlayCountDown());
        startRace.SetActive(false);
        waitingText.SetActive(false);

        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        carsCPM = new CheckpointManager[cars.Length];

        for (int i = 0; i < cars.Length; ++i) {
            carsCPM[i] = cars[i].GetComponent<CheckpointManager>();

        }

    }

    IEnumerator PlayCountDown() {

        yield return new WaitForSeconds(2);
        foreach (GameObject g in countdownItems) {

            g.SetActive(true);
            yield return new WaitForSeconds(1);
            g.SetActive(false);
        }

        racing = true;
    }

    [PunRPC]
    public void RestartGame() {

        PhotonNetwork.LoadLevel("Track01");
    }

    public void RestartLevel() {

        racing = false;
        if (PhotonNetwork.IsConnected)
            photonView.RPC("RestartGame", RpcTarget.All, null);
        else
            SceneManager.LoadScene("Track01");
    }

    //bool raceOver = false;

    //private void Update() {

    //    if (Input.GetKeyDown(KeyCode.R))
    //        raceOver = true;
    //}

    private void LateUpdate() {

        if (!racing) return;
        int finishedCount = 0;


        foreach (CheckpointManager cpm in carsCPM) {
            // if (cpm == null) continue;
            if (cpm.lap == totalLaps + 1) {

                finishedCount++;
            }
        }

        if (finishedCount == carsCPM.Length) { // || raceOver) {
            HUD.SetActive(false);
            gameOverPanel.SetActive(true);
        }
    }
}
