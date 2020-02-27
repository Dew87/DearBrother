using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Regular,
    Quick,
    Interrupt
}

public enum CharacterPerson
{
    Sam,
    Owl,
    TarMan,
    Memory
}

public class DearBrotherCharacter : MonoBehaviour
{
    public CharacterType type;
    public CharacterPerson person;
}
