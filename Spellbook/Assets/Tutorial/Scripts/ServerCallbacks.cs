using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This attribure tells Bolt to only create an instance of 
 * this class on the server.
 * Using this attribute means that you don't have to 
 * manually deal with keeping a game object around with 
 * this script attached to it.
 *  for more on global callbacks visit:
 *  https://doc.photonengine.com/en-us/bolt/current/in-depth/global-callbacks */
[BoltGlobalBehaviour(BoltNetworkModes.Server)]

/*
 In Bolt you use EventName.Create(); to create a new event, you then 
 assign the properties you want and call eventObject.Send(); to send 
 it on it's way. The Create method has several overloads with different 
 parameters that lets you specify who the event should go to, how it
 should be delivered, etc.
     */
public class ServerCallbacks : Bolt.GlobalEventListener
{
    public override void Connected(BoltConnection connection)
    {
        var log = LogEvent2.Create();
        log.Message = string.Format("{0} connected", connection.RemoteEndPoint);
        log.Send();
    }

    public override void Disconnected(BoltConnection connection)
    {
        var log = LogEvent2.Create();
        log.Message = string.Format("{0} disconnected", connection.RemoteEndPoint);
        log.Send();
    }
}
