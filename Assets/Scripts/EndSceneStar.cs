using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneStar : MonoBehaviour
{
    [SerializeField] private GameObject image1;
    [SerializeField] private GameObject image2;
    [SerializeField] private GameObject image3;
    int numStar = 3;
    private static StarControl starControl;
    // Start is called before the first frame update
    void Start()
    {
        starControl = FindObjectOfType<StarControl>();
        numStar = PlayerData.Time;
        if (numStar == 3)
        {
            image3.SetActive(true);
            image2.SetActive(true);
            image1.SetActive(true);
        }
        else if (numStar == 2)
        {
            image3.SetActive(false);
        }
        else if (numStar == 1)
        {
            image3.SetActive(false);
            image2.SetActive(false);
        }
        else if (numStar == 0)
        {
            image3.SetActive(false);
            image2.SetActive(false);
            image1.SetActive(false);
        }
    }


}
