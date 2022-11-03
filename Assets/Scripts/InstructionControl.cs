using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionControl : MonoBehaviour
{
    public GameObject Control_panel;
    public GameObject Scene_panel;
    public GameObject Rule_panel;
    public GameObject instruction;

    void Start()
    {
        instruction.SetActive(true);
        Control_panel.SetActive(true);
    }

    // Update is called once per frame
    public void press_Control()
    {
        if (!Control_panel.activeSelf)
        {
            Scene_panel.SetActive(false);
            Rule_panel.SetActive(false);
            Control_panel.SetActive(true);
        }
    }
    public void press_Scene()
    {
        if (!Scene_panel.activeSelf)
        {
            Control_panel.SetActive(false);
            Rule_panel.SetActive(false);
            Scene_panel.SetActive(true);
        }
    }
    public void press_Rule()
    {
        if (!Rule_panel.activeSelf)
        {
            Scene_panel.SetActive(false);
            Control_panel.SetActive(false);
            Rule_panel.SetActive(true);
        }
    }
    public void press_ruturn()
    {
        instruction.SetActive(false);
    }
}
