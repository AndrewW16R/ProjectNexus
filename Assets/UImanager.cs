using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{

    PlayerMovementV2 player;
    public GameObject nexus;
    public Text textFieldDashes;

    // Start is called before the first frame update
    void Start()
    {
        //Gets reference to nexus and its PlayerMovementV2 script
        nexus = GameObject.Find("Nexus");
        player = nexus.GetComponent<PlayerMovementV2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textFieldDashes.text = "Dashes Available: " + player.dashesAvailable.ToString();
    }
}
