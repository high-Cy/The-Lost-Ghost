using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManage : MonoBehaviour
{
    public CharacterDB database;
    public int index = 0;
    private Vector3 characterPos = new Vector3(378.42f, 7.53957f, 13.34f);
    private Quaternion characterRot = Quaternion.Euler(new Vector3(0f, 200, 0f));
    [SerializeField] private Transform parent;
    private GameObject character;
    private GameObject characterObj;
    private bool isMainMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartScene") isMainMenu = true;
        else isMainMenu = false;

        if (isMainMenu)
        {
            character = database.GetCharacter(index);
            characterObj = Instantiate(character, characterPos, characterRot, parent);
            characterObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            UpdateCharacter(index);
        }
        else
        {

        }

    }
    public void NextCharacter()
    {
        AudioManager.Instance.PlayClip("SelectSfx");
        index++;
        if (index >= database.numCharacter)
        {
            index = 0;
        }
        database.savedIndex = index;
        UpdateCharacter(index);
    }
    private void UpdateCharacter(int index)
    {
        Destroy(characterObj);
        character = database.GetCharacter(index);
        characterObj = Instantiate(character, characterPos, characterRot, parent);
        characterObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

    }
    public GameObject GetCharacterObj()
    {
        return characterObj;
    }
}
