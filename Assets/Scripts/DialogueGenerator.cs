using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueGenerator : MonoBehaviour
{
    private HouseGenerator houseGenerator;
    private Transform target;
    private Transform goal;
    public Text dialogueHouse;
    private TimeCounting timeCounting;
    private ChickFollow chickFollow;
    private PlayerCollision playerCollision;
    private float meetChick = 1f;
    private bool metChick = false;
    private Vector3 view1;
    private Vector3 view2;
    private Vector3 view3;
    private Vector3 view4;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        houseGenerator = FindObjectOfType<HouseGenerator>();
        timeCounting = FindObjectOfType<TimeCounting>();
        chickFollow = FindObjectOfType<ChickFollow>();
        playerCollision = FindObjectOfType<PlayerCollision>();

        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GrasslandScene" || sceneName == "DesertScene")
        {
            view1 = GameObject.FindGameObjectWithTag("Special 1").GetComponent<ObjectsGenerator>().GetCenter();
            view2 = GameObject.FindGameObjectWithTag("Special 2").GetComponent<ObjectsGenerator>().GetCenter();
        }
        if (sceneName == "DesertScene")
        {
            view3 = GameObject.FindGameObjectWithTag("Special 3").GetComponent<ObjectsGenerator>().GetCenter();
            //view4 = GameObject.FindGameObjectWithTag("Special 4").GetComponent<ObjectsGenerator>().GetCenter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float playerHouseDistance = Vector3.Distance(target.position, houseGenerator.GetHousePos());
        float playerGoalDistance = Vector3.Distance(goal.position, target.position);
        float playerViewDistance1 = Vector3.Distance(target.position, view1);
        float playerViewDistance2 = Vector3.Distance(target.position, view2);
        float playerViewDistance3 = 999f;
        float playerViewDistance4 = 999f;
        if (sceneName == "DesertScene")
        {
            playerViewDistance3 = Vector3.Distance(target.position, view3);
            playerViewDistance4 = Vector3.Distance(target.position, view4);
        }

        if (chickFollow.startFollow && meetChick != 0f)
        {
            if (!metChick)
            {
                metChick = true;
                meetChick = timeCounting.GetTimer();
            }
            if ((timeCounting.GetTimer() - meetChick) < 4 && metChick == true)
            {
                dialogueHouse.text = "So nice to meet my old friends!";
            }
            else if ((timeCounting.GetTimer() - meetChick) < 8 && metChick == true)
            {
                dialogueHouse.text = "Hopefully someone'll feed them...";
            }
            else
            {
                meetChick = 0f;
            }
        }
        else if (metChick == false && playerGoalDistance <= 15)
        {
            dialogueHouse.text = "No, I haven't found my old friend...";
        }
        // player being attacked
        else if (playerCollision.GetAttacked())
        {
            dialogueHouse.text = "OOOOOOOUCH~";
        }

        else if (playerCollision.GetHitWall()){
            dialogueHouse.text = "Something's blocking me..";
        }
        else if (((playerViewDistance1 < 20) || (playerViewDistance2 < 20)) && name == "GrasslandScene")
        {
            dialogueHouse.text = "WOW..Spring is coming~";
        }
        else if (((playerViewDistance1 < 20) || (playerViewDistance2 < 20)) && name == "DesertScene")
        {
            dialogueHouse.text = "HORRIBLE!!";
        }
        // survival pack
        else if (sceneName == "DesertScene" && (playerViewDistance3 < 20))
        {
            dialogueHouse.text = "Expedition has been here.";
        }
        else if (sceneName == "GrasslandScene" && playerHouseDistance < 40)
        {
            dialogueHouse.text = "LOOK!! My old home!!";
        }
        else if (sceneName == "GrasslandScene" && playerHouseDistance < 20)
        {
            dialogueHouse.text = "Sigh.. the good ol' days..";
        }
        else if (playerHouseDistance < 40 && sceneName == "DesertScene")
        {
            dialogueHouse.text = "Too bad this isn't my home...";
        }
        else if (timeCounting.GetTimer() > 100 && timeCounting.GetTimer() < 105)
        {
            dialogueHouse.text = "Ugh..So tired..:(";
        }
        else if (timeCounting.GetTimer() > 200 && timeCounting.GetTimer() < 205)
        {
            dialogueHouse.text = "Time to hurry home~";
        }
        else
        {
            dialogueHouse.text = "";
        }

    }
}
