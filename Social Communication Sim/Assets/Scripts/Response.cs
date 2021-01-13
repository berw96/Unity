using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>Response</c> contains all of the data for each
/// response instance. The member variable <c>type</c> is
/// serialized so that it is accessible via the Unity IDE.
/// This enables each response's type to be set individually.
/// 
/// Once instantiated, the type of the response determines its
/// <c>text</c> field, which is accessed via
/// <c>GetComponentInChildren<Text>().text</c>. The <c>text</c> field's
/// string value also depends on each response's set <c>topic</c>
/// and the responses provided in previous parts of the conversation.
/// Each repsonse instance knows the conversation history.
/// </summary>

public class Response : MonoBehaviour
{
    [SerializeField] ResponseData.Type type;
    private static ResponseData.Topic topic;
    private static List<ResponseData.Type> history;
    private static float timer;
    

    /// <summary>
    /// Function <c>Awake</c> is called by the Unity engine on startup.
    /// Here it sets the topic of each response and initializes the
    /// list of responses. At this stage <c>history</c> has a Count
    /// of 0, corresponding to 'no history'. Once the player selects a
    /// response button the <c>onSelected</c> function is invoked, which
    /// adds the selected response's type to the list, via the inbuilt
    /// <c>Add</c> function which increments the Count of the list
    /// by +1. The player may not select NPC responses.
    /// </summary>
    public void Awake()
    {
        if (this.type == ResponseData.Type.NPC)
        {
            GetComponent<Image>().color = new Color(200.0f, 0.0f, 0.0f, 0.392f);
        }
        topic = ResponseData.Topic.WEATHER;
        history = new List<ResponseData.Type>();
        Debug.Log("history.Count = " + history.Count);
    }

    /// <summary>
    /// Function <c>Update</c> is called by the Unity engine every tick/frame
    /// of the computer's CPU. Here it is used to constantly update the contents
    /// of each response based on the response history established throughout
    /// the conversation between the player and the NPC.
    /// </summary>
    public void Update()
    {
        for (int i = 0; i < history.Count; i++)
        {
            Debug.Log("Response " + (i + 1) + ": " + history[i]);
        }
        setContents();
    }

    /// <summary>
    /// Function <c>setContents</c> sets the contents of each response
    /// based on the history of responses provided, the <c>topic</c> of the
    /// response and its <c>type</c>. Response contents are stored in a
    /// <c>Text</c> component which is accessible via 
    /// <c>GetComponenet<Text>().text</c>.
    /// </summary>
    public void setContents()
    {
        switch (history.Count)
        {
            case 0:
                Debug.Log("Setting initial responses");
                switch (topic)
                {
                    case ResponseData.Topic.WEATHER:
                        switch (type)
                        {
                            case ResponseData.Type.NPC:
                                GetComponentInChildren<Text>().text = "Hello! Lovely weather today, isn\'t it?";
                                break;
                            case ResponseData.Type.POSITIVE:
                                GetComponentInChildren<Text>().text = "Yes, it certainly is!";
                                break;
                            case ResponseData.Type.NEGATIVE:
                                GetComponentInChildren<Text>().text = "...";
                                break;
                            case ResponseData.Type.NEUTRAL:
                                GetComponentInChildren<Text>().text = "I don\'t really care much about the weather.";
                                break;
                        }
                        Debug.Log("Response Type: " + this.type);
                        break;
                }
                break;
            case 1:
                Debug.Log("Setting responses at stage 2");
                switch (history[history.Count - 1])
                {
                    case ResponseData.Type.POSITIVE:
                        switch (topic)
                        {
                            case ResponseData.Topic.WEATHER:
                                switch (type)
                                {
                                    case ResponseData.Type.NPC:
                                        GetComponentInChildren<Text>().text = "Although, it is too hot for my taste.";
                                        break;
                                    case ResponseData.Type.POSITIVE:
                                        GetComponentInChildren<Text>().text = "It can be TOO hot, can\'t it?";
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        GetComponentInChildren<Text>().text = "Sorry, I would like to be left alone.";
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        GetComponentInChildren<Text>().text = "...";
                                        break;
                                }
                                Debug.Log("Response Type: " + this.type);
                                break;
                        }
                        break;
                    case ResponseData.Type.NEGATIVE:
                        switch (topic)
                        {
                            case ResponseData.Topic.WEATHER:
                                switch (type)
                                {
                                    case ResponseData.Type.NPC:
                                        GetComponentInChildren<Text>().text = "...";
                                        break;
                                    default:
                                        GetComponentInChildren<Text>().text = "";
                                        break;
                                }
                                Debug.Log("Response Type: " + this.type);
                                break;
                        }
                        break;
                    case ResponseData.Type.NEUTRAL:
                        switch (topic)
                        {
                            case ResponseData.Topic.WEATHER:
                                switch (type)
                                {
                                    case ResponseData.Type.NPC:
                                        GetComponentInChildren<Text>().text = "Why not?";
                                        break;
                                    case ResponseData.Type.POSITIVE:
                                        GetComponentInChildren<Text>().text = "I don\'t mind what the weather is really.";
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        GetComponentInChildren<Text>().text = "...";
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        GetComponentInChildren<Text>().text = "I prefer to stay indoors myself.";
                                        break;
                                }
                                Debug.Log("Response Type: " + this.type);
                                break;
                            case ResponseData.Topic.WORK:
                                Debug.Log("This topic is currently unavailable.");
                                break;
                        }
                        break;
                }
                break;
            case 2:
                Debug.Log("Setting responses at stage 3");
                switch (history[history.Count - 2])
                {
                    case ResponseData.Type.POSITIVE:
                        switch (history[history.Count - 1])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (topic)
                                {
                                    case ResponseData.Topic.WEATHER:
                                        switch (type)
                                        {
                                            case ResponseData.Type.NPC:
                                                GetComponentInChildren<Text>().text = "Yeah.";
                                                break;
                                            case ResponseData.Type.POSITIVE:
                                                GetComponentInChildren<Text>().text = "I\'m " + "<player_name>" + " by the way.";
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                GetComponentInChildren<Text>().text = "You are pretty hot.";
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                GetComponentInChildren<Text>().text = "Yeah...";
                                                break;
                                        }
                                        Debug.Log("Response Type: " + this.type);
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEGATIVE:
                                switch (topic)
                                {
                                    case ResponseData.Topic.WEATHER:
                                        switch (type)
                                        {
                                            case ResponseData.Type.NPC:
                                                GetComponentInChildren<Text>().text = "Pardon me?";
                                                break;
                                            case ResponseData.Type.POSITIVE:
                                                GetComponentInChildren<Text>().text = "Sorry if I\'m awkward, I\'m not used to socializing like this.";
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                GetComponentInChildren<Text>().text = "I have to go...";
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                GetComponentInChildren<Text>().text = "I\'m not having a good day.";
                                                break;
                                        }
                                        Debug.Log("Response Type: " + this.type);
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (topic)
                                {
                                    case ResponseData.Topic.WEATHER:
                                        switch (type)
                                        {
                                            case ResponseData.Type.NPC:
                                                GetComponentInChildren<Text>().text = "...";
                                                break;
                                            case ResponseData.Type.POSITIVE:
                                                GetComponentInChildren<Text>().text = "It\'s very peaceful out here, actually.";
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                GetComponentInChildren<Text>().text = "...";
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                GetComponentInChildren<Text>().text = "So, what have you been up to today?";
                                                break;
                                        }
                                        Debug.Log("Response Type: " + this.type);
                                        break;
                                }
                                break;
                        }
                        break;
                    case ResponseData.Type.NEUTRAL:
                        switch (history[history.Count - 1])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (topic)
                                {
                                    case ResponseData.Topic.WEATHER:
                                        switch (type)
                                        {
                                            case ResponseData.Type.NPC:
                                                GetComponentInChildren<Text>().text = "Oh, I understand.";
                                                break;
                                            case ResponseData.Type.POSITIVE:
                                                GetComponentInChildren<Text>().text = "So, what brings you outdoors?";
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                GetComponentInChildren<Text>().text = "Good.";
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                GetComponentInChildren<Text>().text = "Me too.";
                                                break;
                                        }
                                        Debug.Log("Response Type: " + this.type);
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEGATIVE:
                                switch (topic)
                                {
                                    case ResponseData.Topic.WEATHER:
                                        switch (type)
                                        {
                                            case ResponseData.Type.NPC:
                                                GetComponentInChildren<Text>().text = "...";
                                                break;
                                            default:
                                                GetComponentInChildren<Text>().text = "";
                                                break;
                                        }
                                        Debug.Log("Response Type: " + this.type);
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (topic)
                                {
                                    case ResponseData.Topic.WEATHER:
                                        switch (type)
                                        {
                                            case ResponseData.Type.NPC:
                                                GetComponentInChildren<Text>().text = "Indoors is nice, but nature is better.";
                                                break;
                                            case ResponseData.Type.POSITIVE:
                                                GetComponentInChildren<Text>().text = "Can\'t argue with you there, nature is soothing.";
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                GetComponentInChildren<Text>().text = "Nature works for us, not the other way around.";
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                GetComponentInChildren<Text>().text = "Why do you like the outdoors moreso?";
                                                break;
                                        }
                                        Debug.Log("Response Type: " + this.type);
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
            case 3:
                Debug.Log("Setting responses at stage 4");
                switch (history[history.Count - 3])
                {
                    case ResponseData.Type.POSITIVE:
                        switch (history[history.Count - 2])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (history[history.Count - 1])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "<NPC_name>" + ", nice to meet you.";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "That\'s a cool name.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "That\'s a weird name.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "So " + "<NPC_name>" + ", what brings you outdoors today?";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "...excuse me?";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "My apologies, I didn\'t mean anything by it.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "I said you are pretty hot.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "It just looks like the heat\'s getting to you a bit.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "It\'s quite peaceful here.";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "Isn\'t it lovely? It\'s the best sound in the world.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "It was until you showed up.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "It\'s quite boring.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEGATIVE:
                                switch (history[history.Count - 1])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "It\'s okay, would you like me to go?";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "No it\'s okay, I\'m just not in the mood to talk.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "Yes please.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "I don\'t really mind.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "Sorry, bye then.";
                                                        break;
                                                    default:
                                                        GetComponentInChildren<Text>().text = "";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "I'm sorry to hear that, would you like to talke about it?";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "...sure.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "No.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "I don\'t want to trouble you.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (history[history.Count - 1])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "Haha, until I came along, right?";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "Not at all, I\'m enjoying our conversation.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "Honestly, yes.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "I wouldn\'t put myself down like that.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "...what about you?";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "I quite like it when it\'s hot. It\'s the still air that puts me off.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "...";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "I don\'t mind as long as I\'m comfortable in it.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "Oh, well...I just listened to some music.";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "Which music did you listen to?";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "I don't want to talk about music.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "Why did you listen to music?";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    case ResponseData.Type.NEUTRAL:
                        switch (history[history.Count - 2])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (history[history.Count - 1])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "Mainly the fresh air, but the birds too.";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "I can always hear them but never see them.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "I hate birds, all they do is screech and crap everywhere.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "I hate birds, they creep me out.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "...";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "I\'m sorry if that sounded condescending.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "What\'s wrong, can\'t handle honesty?";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "Very good.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "Mind if I ask why you\'re out?";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "I just felt like coming outdoors.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "I do, I\'d rather keep to myself.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "Same reason as yourself I assume?";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (history[history.Count - 1])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "My name\'s " + "<NPC_name>" + " by the way.";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "Nice to meet you " + "<NPC_name>" + ", I\'m " + "<player_name>" + ".";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "I\'m not looking for a friend.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "Oh, that\'s nice.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "Sure, okay.";
                                                        break;
                                                    default:
                                                        GetComponentInChildren<Text>().text = "";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (topic)
                                        {
                                            case ResponseData.Topic.WEATHER:
                                                switch (type)
                                                {
                                                    case ResponseData.Type.NPC:
                                                        GetComponentInChildren<Text>().text = "Being indoors all the time it unhealthy. I like fresh air and Sunlight.";
                                                        break;
                                                    case ResponseData.Type.POSITIVE:
                                                        GetComponentInChildren<Text>().text = "You\'re right, maybe I should get out more.";
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        GetComponentInChildren<Text>().text = "I bet you're a lot of fun at parties.";
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        GetComponentInChildren<Text>().text = "You know what, maybe you\'re right.";
                                                        break;
                                                }
                                                Debug.Log("Response Type: " + this.type);
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
            case 4:
                Debug.Log("Setting responses at stage 5");
                switch (history[history.Count - 4])
                {
                    case ResponseData.Type.POSITIVE:
                        switch (history[history.Count - 3])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (history[history.Count - 2])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Haha I suppose it is. Well, I should get going.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "It was nice meeting you " + "<NPC_name>" + ".";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Leave already!";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "...but not as cool as mine!";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Well, it\'s the only one I\'ve got.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I\'m joking, that\'s a pretty cool name.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Poor you.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "Do you have a nickname?";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I was listening to some classical music, then I felt like socializing.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "You like the classics too?";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "The classics are outdated.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I\'m not a big fan of the classics, but to each their own.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Haha it\'s okay.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "You seem like a really pleasant person...";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "I get flustered around attractive people.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I thought I\'d try flirting for fun.";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I suggest we both get indoors, we don\'t want to get sunburnt.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Would you care to come home with me.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "Is there something wrong?";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Oh I\'m fine, thanks.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Okay that\'s good.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "You seem like the sort of person to worry about.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "If you say so.";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Perhaps not THE best, but it\'s definitely up there.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "It\'s a matter of opinion.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "I hope your children don\'t share your views.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I disagree, nothing can beat this.";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "...I didn\'t mean to disturb you.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I\'m kidding! It\'s so nice to chat with someone outdoors.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Are you deaf? Go away.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I must leave now.";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "To each their own I say.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Please excuse me.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "This is not a debate, it\'s boring.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I prefer having things happening around me.";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEGATIVE:
                                switch (history[history.Count - 2])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I understand.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Thank you.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Good.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "No worries, sorry to bother you.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Thank you.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Please leave.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "In that case then I\'ll keep to myself.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Thank you, I\'m sorry.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "On second thought, leave me alone.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "So, what\'s got you down?";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "It\'s complicated, but I feel like I\'m just not good enough.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Can you give me some money?";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "You wouldn\'t understand.";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "No worries, my apologies.";
                                                                break;
                                                            default:
                                                                GetComponentInChildren<Text>().text = "";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "If you say so, I\'ll be going then.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Have a nice day.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Yeah I did. Now go away.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (history[history.Count - 2])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Haha, me too. I didn\'t get your name.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "It\'s " + "<player_name>" + ", what about yours?";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "I\'d rather not say.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "It\'s " + "<player_name>" + ".";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I was hoping you wouldn\'t say that.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I\'m sorry if I was rude, I have sensitive hearing.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Now you\'ve ruined my day!";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I\'m sorry, I didn\'t mean to offend you.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I\'m just being ironic.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Oh I see.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "I thought you got the hint.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "What does that mean?";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I know. Still air spoils hot weather. Maybe that\'s what\'s bothering me.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "We should find a bench in the shade.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Stop whining and deal with it.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I can\'t help you there.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                Debug.Log("GAME OVER");
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Ah I see, that\'s a very accommodating mindset to have.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "It\'s bound to change as life goes on.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "...but not accommodating enough for you.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "Thank you.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "The classics. Which music do you like?";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I like the classics too.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "The classics are for boring old people.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "The classics are okay, but I prefer modern music.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Really, you don\'t like music then?";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "It is an interesting topic but I feel more inclined to chat about other things.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Not when you\'re talking about it, no.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I\'m more one for the visual arts.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "It relaxes me and enables me to focus on daily tasks.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "It’s interesting how music can do that.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "...like being mundane?";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "Oh.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    case ResponseData.Type.NEUTRAL:
                        switch (history[history.Count - 3])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (history[history.Count - 2])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "It's strange because it sounds like there's many of them.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Oh well, as long as they\'re happy and keep doing what they do best.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "How on Earth could you possibly know that?!";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "Too many you think?";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "You make them sound like insufferable babies!";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Maybe I should get a pet hawk to hunt them all.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "I hope they all die.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "They\'re worse!";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Oh, are you ornithophobic?";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I\'m just afraid they\'ll give me a disease or hurt me.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Not that bad, I just hate birds.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "If that means I don\'t like birds, then yes I am.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "It was, but if you are sorry then I\'m happy to put that behind us.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I\'m glad.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "You seem a lot more hurt than you should be.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "...ahahahaha! Whatever.";
                                                                break;
                                                            default:
                                                                GetComponentInChildren<Text>().text = "";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Uh, okay...";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I think good weather birngs good people together.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "...if you move your fat ass the Sun might actually come out.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "Which weather did you say you preffered again?";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I get it.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "You might also get to see me around here again.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "...congratulations.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I mean, who doesn\'t like the outdoors?";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Okay, if you say so.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Thank you.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Just leave me alone!";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I was hoping you could answer that for me. I\'m outdoors for the tranquillity it offers.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "The weather is quite tranquil isn\'t it?";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "...";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I like to keep most things to myself.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (history[history.Count - 2])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Cool, it\'s nice to meet you too. Maybe we\'ll meet again?";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Definitely, I\'d like that.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "I hope not.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I doubt it.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Well shucks, I thought I\'d try.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "You\'re a nice person, but I\'m afraid I simply don\'t have the time to dedicate myself.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "That was a very poor attempt.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I\'m not very good at maintaining friendships.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "...am I boring you?";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "No, I\'m just off-form today. My apologies.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Yes.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "No, of course not.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 1])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "It doesn\'t hurt to, does it?";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "I\'m glad I came out today.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "I just wish people weren\'t boring like you.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I just wish I lived nearer to the park.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "Hey...at least I can go to parties.";
                                                                break;
                                                            default:
                                                                GetComponentInChildren<Text>().text = "";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (topic)
                                                {
                                                    case ResponseData.Topic.WEATHER:
                                                        switch (type)
                                                        {
                                                            case ResponseData.Type.NPC:
                                                                GetComponentInChildren<Text>().text = "I\'d like to think so, but I digress.";
                                                                break;
                                                            case ResponseData.Type.POSITIVE:
                                                                GetComponentInChildren<Text>().text = "Thank you for the tip.";
                                                                break;
                                                            case ResponseData.Type.NEGATIVE:
                                                                GetComponentInChildren<Text>().text = "Okay, no need to rub your \"superior intellect\" in my face.";
                                                                break;
                                                            case ResponseData.Type.NEUTRAL:
                                                                GetComponentInChildren<Text>().text = "I\'ll think about what you said.";
                                                                break;
                                                        }
                                                        Debug.Log("Response Type: " + this.type);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
            case 5:
                Debug.Log("Setting NPC end remarks.");
                switch (history[history.Count - 5])
                {
                    case ResponseData.Type.POSITIVE:
                        switch (history[history.Count - 4])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (history[history.Count - 3])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "As with you <player_name>, see you around!";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...yes your majesty.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'ll try to keep that in mind, see ya!";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Oh, haha. Well it was nice chatting, I\'ve gotta run.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'d rather be <NPC_name> than a jerk any day. See ya.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I do, it\'s <NPC_nickname>.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I love the classics.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'m sorry you think that way, you are missing out.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "That\'s fair enough.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Well I\'d prefer to be friends if that\'s okay...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'d stop right now and change your approach if I were you.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'m not interested in relationships at the moment.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Oh, I agree. Well it was nice meeting you. Take care!";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You are making me feel uncomfortable, you seemed so nice at first.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Nothing, I have to get going now.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'ll be going now, it was nice to meet you.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I hope your bantering skills improve haha.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;

                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Tends to be that way with people.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...what a vague comment...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Well, when you put it like that it sounds to me like you lack stimulus.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Oh! Haha, you had me there for a second.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "How rude! I never!";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Oh, bye.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "No worries, good talking with you.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You sound like quite the politician.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You sound like quite the adventurer.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEGATIVE:
                                switch (history[history.Count - 3])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "No problem.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "That\'s okay.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'m going, I\'m going.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Take care.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "It\'s no problem, please take care.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...well fine then, goodbye.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'m sure everything will be okay, you should give yourself a chance.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Sorry, but I can\'t.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Then this conversation is pointless.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You too.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Fine, no need to be rude about it.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...have a nice day.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (history[history.Count - 3])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Nice, it\'s <NPC_name>.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Okay. Well, see you.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Nice meeting you <player_name>.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'m sorry to hear that, don\'t worry I shan\'t bother you.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "With your attitide that makes two of us.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "It\'s okay, but you could be more subtle though.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Yeah.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You were misleading.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You're welcome.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "That sounds nice, sure.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Yep, will do.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'ll cope.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "As things do.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...uh...good one...?";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You\'re welcome.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "They are indeed timeless masterpieces.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Not too sure why you\'d jump to that conclusion.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "To each their own.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Like what?";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "How should I talk about it then, professor?";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Oh, fair enough.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Indeed it is.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Oh definitely, you\'re my biggest role model on that.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Yeah, I recommend it.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    case ResponseData.Type.NEUTRAL:
                        switch (history[history.Count - 4])
                        {
                            case ResponseData.Type.POSITIVE:
                                switch (history[history.Count - 3])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Exactly.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I don\'t know the exact number but I can tell there are lot\'s here.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I think there\'s just the right amount.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Haha, or a drone.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Maybe we should change the topic...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'ve never met someone so antiavian.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I understand, but no-one\'s asking you to eat them. They\'re practically harmless.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Not that bad, I just hate birds.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "It does. It must be challenging for you to come outside.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEGATIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Me too, I\'ll be leaving now.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "My emotions are mine, not yours.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...well, good day.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "That\'s an interesting philosophy, I like that.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...are you sure it isn\'t your ego that\'s blocking it?";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I didn\'t, but seeing as you asked - Sunny with a light breeze.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "That would be very nice, it\'s been pleasant talking to you.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Well, I have to go now, bye!";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Beats me. Anyways, I bid you a good day.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "No worries.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "All you had to do was ask nicely!";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Indeed.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...anyway I must get going now, maybe see you again.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "That makes you a difficult person for others to know.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case ResponseData.Type.NEUTRAL:
                                switch (history[history.Count - 3])
                                {
                                    case ResponseData.Type.POSITIVE:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Okay then, you have a nice day.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "If you put it like that then neither do I.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Well if you are ever around I\'ll come and say hello.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEGATIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "It\'s okay, I understand, although I must say that I am disappointed, you seem like a nice person too.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Hey at least I tried, bye then.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "...";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'m sorry to hear that, it\'s okay don\'t worry.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "You\'re a very blunt individual.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Oh okay then, I\'m glad.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case ResponseData.Type.NEUTRAL:
                                        switch (history[history.Count - 2])
                                        {
                                            case ResponseData.Type.POSITIVE:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "That\'s good.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I\'d rather be boring than obnoxious.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "Parks are nice, although getting out at all is a treat.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            case ResponseData.Type.NEUTRAL:
                                                switch (history[history.Count - 1])
                                                {
                                                    case ResponseData.Type.POSITIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "It\'s always better to see people in-person than on a screen.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEGATIVE:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "It doesn\'t take superior intellect to recognize that was a rude thing to say.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    case ResponseData.Type.NEUTRAL:
                                                        switch (topic)
                                                        {
                                                            case ResponseData.Topic.WEATHER:
                                                                switch (type)
                                                                {
                                                                    case ResponseData.Type.NPC:
                                                                        GetComponentInChildren<Text>().text = "I hope it helps.";
                                                                        break;
                                                                    default:
                                                                        GetComponentInChildren<Text>().text = "";
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
        }
        Debug.Log("history.Count = " + history.Count);
    }

    /// <summary>
    /// Function <c>onSelected</c> is attached to a button which
    /// the player can click on. When invoked it adds the type of
    /// the chosen response to the list of historic response types
    /// and then uses it in the function <c>setContents</c> to
    /// update all the other responses in the conversation.
    /// </summary>
    public void onSelected()
    {
        history.Add(this.type);
    }

    public void setTopic(ResponseData.Topic t)
    {
        topic = t;
    }

    public void resetHistory()
    {
        Debug.Log("Clearing history.");
        history.Clear();
    }
}
