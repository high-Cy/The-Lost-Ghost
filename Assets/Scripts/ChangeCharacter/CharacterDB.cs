using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class CharacterDB : ScriptableObject

{
    public GameObject[] characters;
    public int numCharacter = 5;
    [HideInInspector] public int savedIndex = 0;
    public GameObject GetCharacter(int index){
        return characters[index];
    }
    


}
