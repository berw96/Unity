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
    private const int max = 5;
    public bool isOver;

    public enum Type
    {
        POSITIVE, NEGATIVE, NEUTRAL, NPC
    }
    public enum Topic
    {
        WEATHER, WORK, ROMANCE, SHOPPING, SCHOOL, POLITICS, OTHERS
    }

    public List<Type> responseHistory;

    public void initResponseDataObject()
    {
        isOver = false;
        responseHistory = new List<Type>();
    }

    public int getMaxHistory()
    {
        return max;
    }
}
