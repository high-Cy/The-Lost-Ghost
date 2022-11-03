using UnityEngine;
using UnityEngine.UI;

public class TimeCounting : MonoBehaviour
{
    // Start is called before the first frame update
    private int startTime = 0;
    private float timer = 0;
    public Text timeString;


    void Start()
    {
        timer = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        TimeDisplay(timer);
    }
    void TimeDisplay(float time){
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeString.text = string.Format("{00:00}:{1:00}", minutes, seconds);
        

    }
    public float GetTimer(){
        return timer;
    }
}
