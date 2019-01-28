using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Important: You should not manually attach an instance of this script to 
any GameObject in Unity, it will be handled by Bolt automatically. If you 
want to manually attach a Bolt.GlobalEventListener somewhere then you 
should not add the [BoltGlobalBehaviour] attribute.
 */


/*What this attribute does is that it allows Bolt to automatically detect this 
* script and create an instance of it which lives together with Bolt and 
* is destroyed when Bolt is shut down.*/
[BoltGlobalBehaviour]

/*
 The Bolt.GlobalEventListener class itself DOES inherit 
 from MonoBehaviour so you can do all the normal Unity 
 stuff as usual, but it adds several Bolt specific methods on-top.
     */
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    List<string> logMessages = new List<string>();

    public override void SceneLoadLocalDone(string scene)
    {
        //base.SceneLoadLocalDone(scene);

        // randomize a position (From example, test purposes only)
        var spawnPosition = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));

        // instantiate cube
        // This is where we spawn Player objects
        BoltNetwork.Instantiate(BoltPrefabs.Cube, spawnPosition, Quaternion.identity);  
        Debug.Log("SPAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAWWWWWWN");
    }

    public override void OnEvent(LogEvent evnt)
    {
        logMessages.Insert(0, evnt.Message);
    }

    void OnGUI()
    {
        // only display max the 5 latest log messages
        int maxMessages = Mathf.Min(5, logMessages.Count);

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height - 100, 400, 100), GUI.skin.box);

        for (int i = 0; i < maxMessages; ++i)
        {
            GUILayout.Label(logMessages[i]);
        }

        GUILayout.EndArea();
    }


    /*
    public override void Connected(BoltConnection connection)
    {
        Debug.Log(connection.ToString() + " connected to us");
        /* Posibly spawn a client Player with this snippet from the Bolt API
        BoltLog.Info("Accept Token {0} using Connect Token {1}", connection.AcceptToken, connection.ConnectToken);
        var player = BoltNetwork.Instantiate(BoltPrefabs.Player);
        player.transform.position = spawnPoint.transform.position;
        var initData = prototype.GetNewPlayer(GameLogic.PlayableClass.Mercenary);
        Configure(player, initData);
        player.AssignControl(connection);
        
    }
    */
}
