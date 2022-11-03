using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickSounds : MonoBehaviour
{
    private Transform player;
    private bool inCooldown = false;
    private string soundName;
    private bool isChick;

    // Start is called before the first frame update
    void Start()
    {
        isChick = gameObject.tag == "Toon Chick";
        player = GameObject.FindGameObjectWithTag("Player").transform;
        soundName = RandomSfxName();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCooldown && !AudioManager.Instance.IsPlaying(soundName) &&
            Vector3.Distance(player.position, transform.position) < 20) {
            
            if (isChick)    AudioManager.Instance.PlayClip("BabyChickSfx");
            else AudioManager.Instance.PlayClip(soundName);

            StartCoroutine(Cooldown());
            soundName = RandomSfxName();
        }
    }

    private IEnumerator Cooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(10);
        inCooldown = false;
    }

    private string RandomSfxName() {
        var rand = Random.Range(0, 3);
        if (rand == 0) return "ChickSfx1";
        else if (rand == 1) return "ChickSfx2";
        else return "ChickSfx3";
    }
}
