using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    public static ConversationManager cm;
    public static Conversation ourConversation;
    public static Conversation.Difficulty difficulty;
    
    private void awake()
    {
        cm = this;
        difficulty = Conversation.Difficulty.MEDIUM;
        ourConversation = new Conversation(difficulty);
    }
}

public class Conversation : MonoBehaviour
{
    //list of stages in the conversation
    int numberOfStages;
    //ArrayList stages;
    List<Stage> stages;

    //difficulty of conversation
    public enum Difficulty
    {
        EASY, MEDIUM, HARD
    }
    Difficulty difficulty;

    public Conversation(Difficulty difficulty)
    {
        this.difficulty = difficulty;
        //sets the number of stages in the conversation depending on the difficulty setting
        switch (difficulty)
        {
            case Difficulty.EASY:
                this.numberOfStages = 3;
                break;
            case Difficulty.MEDIUM:
                this.numberOfStages = 5;
                break;
            case Difficulty.HARD:
                this.numberOfStages = 7;
                break;
        }
        //creates a space in the list for each stage
        this.stages = new List<Stage>(numberOfStages);

        //fills the list of stages up with stages.
        for (int i = 0; i < stages.Capacity; i++)
        {
            this.stages[i] = new Stage(difficulty, stages);
        }
    }

    public List<Stage> getStages()
    {
        return this.stages;
    }
    public Difficulty getDifficulty()
    {
        return this.difficulty;
    }
    public void setStage(int index, Stage s)
    {
        this.stages[index] = s;
    }
    public void setDifficulty(Difficulty d)
    {
        this.difficulty = d;
    }
}

public class Stage : MonoBehaviour
{
    //list of possible responses which each stage has   (determined by conversation difficulty)
    int numberOfResponses;
    ArrayList possibleResponses;
    List<Stage> stages;
    //the response given at each stage is set by the player once they choose a possible response.
    Response.Type playerResponseType;
    
    public Stage(Conversation.Difficulty difficulty, List<Stage> stages)
    {
        //each instance of a stage knows about itself and all the other stages registered.
        this.stages = stages;
        switch (difficulty)
        {
            case Conversation.Difficulty.EASY:
                numberOfResponses = 3;
                break;
            case Conversation.Difficulty.MEDIUM:
                numberOfResponses = 5;
                break;
            case Conversation.Difficulty.HARD:
                numberOfResponses = 7;
                break;
        }
        this.possibleResponses = new ArrayList(numberOfResponses);

        //each new repsonse is constructed based on the stage of the conversation, its difficulty, the response type and the index of the response.
        for (int i = 0; i < possibleResponses.Capacity; i++)
        {
            this.possibleResponses[i] = new Response(this, difficulty, i);
        }
    }

    public ArrayList getResponses()
    {
        return this.possibleResponses;
    }
    public List<Stage> getAllStages()
    {
        return this.stages;
    }
    public Response.Type getPlayerResponse()
    {
        return this.playerResponseType;
    }
    public void setResponse(int index, Response r)
    {
        this.possibleResponses[index] = r;
    }
}

public class Response : MonoBehaviour
{
    //response type
    public enum Type
    {
        POSITIVE, NEGATIVE, NEUTRAL
    }
    Type type;
    //text contained in response    (string)
    string text;

    //tailor's the response to the conversation's history
    public Response(Stage stage, Conversation.Difficulty difficulty, int index)
    {
        switch (difficulty)
        {
            //the difficulty of the conversation determines the number of responses for a single stage.
            case Conversation.Difficulty.EASY:
                //the index of the response determines its type.
                switch (index)
                {
                    case 0:
                        this.type = Type.POSITIVE;
                        for (int i = 0; i < stage.getAllStages().Capacity; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    this.text = "Yes, it certainly is!";
                                    break;
                                case 1:
                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            this.text = "It can be TOO hot though, can\'t it?";
                                            break;
                                        case Type.NEGATIVE:
                                            Debug.Log("Game Over");
                                            break;
                                        case Type.NEUTRAL:
                                            this.text = "...";
                                            break;
                                    }
                                    break;
                                case 2:
                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 3:
                                    switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 4:
                                    switch (stage.getAllStages()[i].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
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

                        }
                        break;
                    case 1:
                        this.type = Type.NEGATIVE;
                        for (int i = 0; i < stage.getAllStages().Capacity; i++)
                        {
                            switch (i) 
                            {
                                case 0:
                                    this.text = "...";
                                    break;
                                case 1:
                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            this.text = "Sorry, I would like to be left alone.";
                                            break;
                                        case Type.NEGATIVE:
                                            Debug.Log("Game Over");
                                            break;
                                        case Type.NEUTRAL:
                                            this.text = "...";
                                            break;
                                    }
                                    break;
                                case 2:
                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 3:
                                    switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 4:
                                    switch (stage.getAllStages()[i].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
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

                        }
                        break;
                    case 2:
                        this.type = Type.NEUTRAL;
                        for (int i = 0; i < stage.getAllStages().Capacity; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    this.text = "I don\'t really care about the weather.";
                                    break;
                                case 1:
                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            this.text = "...";
                                            break;
                                        case Type.NEGATIVE:
                                            Debug.Log("Game Over");
                                            break;
                                        case Type.NEUTRAL:
                                            this.text = "...";
                                            break;
                                    }
                                    break;
                                case 2:
                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    break;
                                                case Type.NEGATIVE:
                                                    break;
                                                case Type.NEUTRAL:
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 3:
                                    switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            break;
                                                        case Type.NEGATIVE:
                                                            break;
                                                        case Type.NEUTRAL:
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                case 4:
                                    switch (stage.getAllStages()[i].getPlayerResponse())
                                    {
                                        case Type.POSITIVE:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEGATIVE:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case Type.NEUTRAL:
                                            switch (stage.getAllStages()[i - 3].getPlayerResponse())
                                            {
                                                case Type.POSITIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEGATIVE:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case Type.NEUTRAL:
                                                    switch (stage.getAllStages()[i - 2].getPlayerResponse())
                                                    {
                                                        case Type.POSITIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEGATIVE:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
                                                                    break;
                                                            }
                                                            break;
                                                        case Type.NEUTRAL:
                                                            switch (stage.getAllStages()[i - 1].getPlayerResponse())
                                                            {
                                                                case Type.POSITIVE:
                                                                    break;
                                                                case Type.NEGATIVE:
                                                                    break;
                                                                case Type.NEUTRAL:
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

                        }
                        break;
                }
                break;
            case Conversation.Difficulty.MEDIUM:
                switch (index)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
                break;
            case Conversation.Difficulty.HARD:
                switch (index)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                }
                break;
        }
    }

    public Type getType()
    {
        return this.type;
    }
    public string getText()
    {
        return this.text;
    }
    public void setType(Type t)
    {
        this.type = t;
    }
    public void setText(string s)
    {
        this.text = s;
    }
}

public class ResponseDatabase : MonoBehaviour
{
    List<Response> positiveResponses;
    List<Response> negativeResponses;
    List<Response> neutralResponses;

    public ResponseDatabase(Conversation.Difficulty difficulty, int capacity)
    {
        initAllResponses(capacity);
    }

    public void initAllResponses(int capacity)
    {
        this.positiveResponses = new List<Response>(capacity);
        this.negativeResponses = new List<Response>(capacity);
        this.neutralResponses = new List<Response>(capacity);
    }
}
