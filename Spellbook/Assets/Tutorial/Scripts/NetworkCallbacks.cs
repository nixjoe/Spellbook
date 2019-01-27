using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 The Bolt.GlobalEventListener class itself does inherit 
 from MonoBehaviour so you can do all the normal Unity 
 stuff as usual, but it adds several Bolt specific methods on-top.
     */
[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string scene)
    {
        //base.SceneLoadLocalDone(scene);

        // randomize a position
        var spawnPosition = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));

        // instantiate cube
        BoltNetwork.Instantiate(BoltPrefabs.Cube, spawnPosition, Quaternion.identity);
    }
}
