using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmojiControl : MonoBehaviour
{
    private ChickFollow firstChick;
    private EmojiDisplay emojiDisplay;
    private HouseGenerator houseGenerator;
    private Transform target;
    private TimeCounting timeCounting;
    private float meetChick = -1f;
    private bool metChick = false;
    private PlayerCollision playerCollision;
    private Transform goal;
    private string sceneName;
    private Vector3 view1;
    private Vector3 view2;
    private Vector3 view3;
    [SerializeField] private GameObject chickEmoji;
    [SerializeField] private GameObject houseEmoji;
    [SerializeField] private GameObject attackedEmoji;
    [SerializeField] private GameObject sadEmoji;
    [SerializeField] private GameObject scaredEmoji;
    [SerializeField] private GameObject disappointEmoji;
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void Update()
    {
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        playerCollision = FindObjectOfType<PlayerCollision>();
        firstChick = FindObjectOfType<ChickensGenerator>().GetFirstChick().GetComponent<ChickFollow>();
        emojiDisplay = FindObjectOfType<EmojiDisplay>();
        timeCounting = FindObjectOfType<TimeCounting>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        houseGenerator = FindObjectOfType<HouseGenerator>();
        float playerHouseDistance = Vector3.Distance(target.position, houseGenerator.GetHousePos());
        float playerViewDistance1 = Vector3.Distance(target.position, view1);
        float playerViewDistance2 = Vector3.Distance(target.position, view2);
        float playerViewDistance3 = 999f;
        float playerGoalDistance = Vector3.Distance(goal.position, target.position);
        if (sceneName == "GrasslandScene" || sceneName == "DesertScene")
        {
            view1 = GameObject.FindGameObjectWithTag("Special 1").GetComponent<ObjectsGenerator>().GetCenter();
            view2 = GameObject.FindGameObjectWithTag("Special 2").GetComponent<ObjectsGenerator>().GetCenter();
        }
        if (sceneName == "DesertScene")
        {
            view3 = GameObject.FindGameObjectWithTag("Special 3").GetComponent<ObjectsGenerator>().GetCenter();
            playerViewDistance3 = Vector3.Distance(target.position, view3);
        }



        if (firstChick.GetFollow() && meetChick != 0f)
        {
            if (!metChick)
            {
                metChick = true;
                emojiDisplay.SetDisplay(false);
                meetChick = timeCounting.GetTimer();
            }

            if ((timeCounting.GetTimer() - meetChick) < 4 && metChick == true)
            {

                emojiDisplay.SetEmoji(chickEmoji);
                emojiDisplay.SetDisplay(true);

            }
            else
            {
                meetChick = 0f;
                emojiDisplay.SetDisplay(false);
            }
        }
        else if (sceneName == "GrasslandScene" && playerHouseDistance < 40 && meetChick <= 0)
        {
            emojiDisplay.SetEmoji(houseEmoji);
            emojiDisplay.SetDisplay(true);
        }
        else if (metChick == false && playerGoalDistance <= 15)
        {
            emojiDisplay.SetEmoji(disappointEmoji);
            emojiDisplay.SetDisplay(true);
        }
        else if (playerCollision.GetAttacked())
        {
            emojiDisplay.SetEmoji(attackedEmoji);
            emojiDisplay.SetDisplay(true);
        }
        else if (playerHouseDistance < 40 && sceneName == "DesertScene" && meetChick <= 0)
        {
            emojiDisplay.SetEmoji(sadEmoji);
            emojiDisplay.SetDisplay(true);
        }
        else if (((playerViewDistance1 < 20) || (playerViewDistance2 < 20)) && name == "DesertScene")
        {
            emojiDisplay.SetEmoji(scaredEmoji);
            emojiDisplay.SetDisplay(true);
        }
        else
        {
            emojiDisplay.SetDisplay(false);
        }
    }
}