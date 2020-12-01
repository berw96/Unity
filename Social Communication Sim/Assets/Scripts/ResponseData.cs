using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>ResponseData</c> inherits from Unity Class
/// <c>ScriptableObject</c> and contains enums defining
/// the different types of response object which can be
/// spawned, their collective difficulty and the topic
/// of conversation they correspond to.
/// </summary>

[CreateAssetMenu]

public class ResponseData : ScriptableObject
{
    public enum Type
    {
        POSITIVE, NEGATIVE, NEUTRAL, NPC
    }
    public enum Topic
    {
        WEATHER, WORK, ROMANCE, SHOPPING, SCHOOL, POLITICS, OTHERS
    }
}
