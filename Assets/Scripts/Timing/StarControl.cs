using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControl : MonoBehaviour

{
    [SerializeField] private GameObject image1;
    [SerializeField] private GameObject image2;
    [SerializeField] private GameObject image3;
    [SerializeField] private float time1;
    [SerializeField] private float time2;
    [SerializeField] private float time3;
    private TimeCounting timeCounting;
    private int numStar = 3;

    // Start is called before the first frame update
    void Start()
    {
        timeCounting = FindObjectOfType<TimeCounting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounting.GetTimer() > time1)
        {
            image1.SetActive(false);
            numStar--;
            PlayerData.Time = 2;
        }
        if (timeCounting.GetTimer() > time2)
        {
            image2.SetActive(false);
            numStar--;
            PlayerData.Time = 1;

        }
        if (timeCounting.GetTimer() > time3)
        {
            image3.SetActive(false);
            numStar--;
            PlayerData.Time = 0;

        }
    }
    public int GetNumStar()
    {
        return numStar;
    }
}
