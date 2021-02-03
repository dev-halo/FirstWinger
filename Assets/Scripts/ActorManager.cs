using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager
{
    private readonly Dictionary<int, Actor> Actors = new Dictionary<int, Actor>();

    public bool Regist(int actorInstanceID, Actor actor)
    {
        if (actorInstanceID == 0)
        {
            Debug.LogError("Regist Error! ActorInstanceID is not set! ActorInstanceID = " + actorInstanceID);
            return false;
        }

        if (Actors.ContainsKey(actorInstanceID))
        {
            if (actor.GetInstanceID() != Actors[actorInstanceID].GetInstanceID())
            {
                Debug.LogError("Regist Error! already exist! ActorInstanceID = " + actorInstanceID);
                return false;
            }

            Debug.Log(actorInstanceID + " is already registed!");
            return true;
        }

        Actors.Add(actorInstanceID, actor);
        Debug.Log("Actor Regist ID = " + actorInstanceID + ", actor = " + actor.name);
        return true;
    }

    public Actor GetActor(int actorInstanceID)
    {
        if (!Actors.ContainsKey(actorInstanceID))
        {
            Debug.LogError("GetActor Error! no exist ActorInstanceID = " + actorInstanceID);
            return null;
        }

        return Actors[actorInstanceID];
    }
}
