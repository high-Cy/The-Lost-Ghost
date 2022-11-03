using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiDisplay : MonoBehaviour
{
    private Transform player;
    private float offsetY = 2.0f;
    private Vector3 emojiPos;
    private GameObject currentEmoji;
    private GameObject mainCamera;
    private bool displayActive = false;
    private bool displaying = false;
    private GameObject emoji;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }
    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        emojiPos = player.position;
        emojiPos.y = emojiPos.y + offsetY;


        if (displayActive == true && displaying == false)
        {
            //Create emoji
            displaying = true;
            emoji = Instantiate(currentEmoji, emojiPos, Quaternion.identity);
            emoji.transform.LookAt(mainCamera.transform.position);
        }

        if (displaying == true)
        {
            //update emoji
            emoji.transform.position = emojiPos;
            emoji.transform.LookAt(mainCamera.transform.position);
        }

        if (displaying == true && displayActive == false)
        {
            //end display
            displaying = false;
            Destroy(emoji.gameObject);
        }
    }

    public void SetDisplay(bool ifDisplay)
    {
        this.displayActive = ifDisplay;
    }

    public void SetEmoji(GameObject emoji)
    {
        this.currentEmoji = emoji;
    }



}