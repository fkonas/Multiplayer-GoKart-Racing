using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedPlayer : MonoBehaviourPunCallbacks {

    public static GameObject localPlayerInstance;
    public GameObject playerNamePrefab;
    public Rigidbody rb;
    public Renderer jeepMesh;

    private void Awake() {

        if (photonView.IsMine) {

            localPlayerInstance = gameObject;
        } else {

            GameObject playerName = Instantiate(playerNamePrefab);
            playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;
            string sentName = null;
            if (photonView.InstantiationData != null)
                sentName = (string)photonView.InstantiationData[0];

            if (sentName != null)
                playerName.GetComponent<Text>().text = sentName;
            else
                playerName.GetComponent<Text>().text = photonView.Owner.NickName;

            playerName.GetComponent<NameUIController>().carRend = jeepMesh;
        }
    }
}
