using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventProvider : MonoBehaviour
{
    [SerializeField]Character character;

    public void CharacterEvent(string eventName)
    {
        character.SetCharacterEvent(eventName);
    }
}
