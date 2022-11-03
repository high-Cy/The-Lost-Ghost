using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadingScene : MonoBehaviour
{
    private bool set = false;
    private bool chick = false;
    public GameObject set1;
    [SerializeField] private GameObject set2;
    [SerializeField] private GameObject set3;
    [SerializeField] private SceneManagement sceneManagement;

    void Start()
    {
        chick = false;
        set = false;
        set1.SetActive(false);
        set2.SetActive(false);
        set3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (set) return;
        
        if (Random.Range(0, 3) == 0)
        {
            set = true;
            chick = true;
        }

        if (sceneManagement.GetIsGrassland())
        {
            set = true;
            if (chick) set3.SetActive(true);
            else set2.SetActive(true);
        }
        else
        {
            set = true;
            if (chick) set3.SetActive(true);
            else set1.SetActive(true);
        }

    }
}
