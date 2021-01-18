using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>ScoreObject</c> handles aspects of the player's
/// conversational score, including how their response history
/// and response time affect it.
/// </summary>

[CreateAssetMenu]

public class ScoreObject : ScriptableObject
{
    public static float score;
    public static List<ResponseData.Type> responseHistory;
    public static List<float> responseTimes;

    /// <summary>
    /// Function <c>initScoreObject</c> initializes the score
    /// attribute and the lists which are used to contain the
    /// player's response history and response times.
    /// </summary>
    /// <param name="initScore"></param>
    public void initScoreObject()
    {
        score = 0.0f;
        responseHistory = new List<ResponseData.Type>();
        responseTimes = new List<float>();
    }

    /// <summary>
    /// Function <c>calculateScore</c> determines the player's score
    /// based on their response history and response times.
    /// A range-based for loop is used to scan the contents of their 
    /// history, a switch statement is used to check which responses
    /// are POSITIVE or NEGATIVE.
    /// 
    /// POSITIVE responses increment the player's score, where NEGATIVE
    /// responses have the opposite effect. NEUTRAL responses are
    /// ignored as they do not affect the player's score.
    /// 
    /// The quicker the player selects a response, the more they will score.
    /// It is assumed that the number of response times is equal to the number
    /// of responses provided.
    /// 
    /// On each call, the score is set to zero to prevent build-up of points
    /// on each calculation.
    /// </summary>
    public void calculateScore()
    {
        score = 0.0f;
        for (int i = 0; i < responseHistory.Count; i++)
        {
            switch (responseHistory[i])
            {
                case ResponseData.Type.POSITIVE:
                    score += (1 / (1 + responseTimes[i]));
                    break;
                case ResponseData.Type.NEGATIVE:
                    score -= (1 / (1 + responseTimes[i]));
                    break;
                default:
                    break;
            }
        }
        Debug.Log("SCORE: " + score);
    }

    /// <summary>
    /// Function <c>logResponseHistory</c> is invoked by an instance
    /// of <c>Response</c> to register the player's response history
    /// so it can be used in the function <c>calculateScore</c>.
    /// </summary>
    /// <param name="history"></param>
    public void logResponseType(ResponseData.Type type)
    {
        responseHistory.Add(type);
        Debug.Log("ADDED " + type + " RESPONSE");
    }

    /// <summary>
    /// Function <c>logResponseHistory</c> is invoked by an instance
    /// of <c>Response</c> to register the player's response time
    /// so it can be used in the function <c>calculateScore</c>.
    /// </summary>
    /// <param name="time"></param>
    public void logResponseTime(float time)
    {
        responseTimes.Add(time);
        Debug.Log("RESPONDED IN " + time + " SECONDS");
    }

    public float getScore()
    {
        return score;
    }
}
