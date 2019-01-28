using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
ICube2State is a C# interface created by Bolt automatically when 
you run Assets/Bolt/Compile Assembly, that exposes all of the 
properties you have defined on the state. This gives you easy 
and statically typed access to all properties of your states.

The class we inherit from, which is defined as Bolt.EntityBehaviour<T> 
in the Bolt source code, takes the type of the state we want to 
use as it's generic parameter, this just tells Bolt the type of 
the state we want to access inside our CubeBehaviour script.

Bolt.EntityBehaviour<T> class inherits from MonoBehaviour and you 
can use all normal Unity methods in here also. */
public class Cube2Behaviour : Bolt.EntityBehaviour<ICube2State>
{

    /*
     Attached() can be thought of as the equivalent of the Start 
     method which exists in Unity, but it's called after the 
     game object has been setup inside Bolt and exists on the network. */
    public override void Attached()
    {

        /*
         state. - This part accesses the state of the entity, which is ICube2State;
         transform - The transform of the GameObject;
         CubeTransform. - Here we access the CubeTransform property we 
            defined in the state in the Bolt Assets and Bolt Editor windows;
         SetTransforms - Here we tell Bolt to use the transform of the 
            current game object where the CubeBehaviour script is attached to */
        state.SetTransforms(state.Cube2Transform, transform);


        //Most of the code in attached will be the exact same for everyone
        //But we call it in Attached still because SimulateOwner is 
        //called every frame.
        if (entity.isOwner)
        {
            state.Cube2Color = new Color(Random.value, Random.value, Random.value);
        }

        state.AddCallback("Cube2Color", ColorChanged);
    }


    /*
     SimulateOwner() can be thought of as the equivalent of the Update 
     method which exists in Unity.
     
     This method will only be called on the local player.
         */
    public override void SimulateOwner()
    {
        var speed = 4f;
        var movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { movement.z += 1; }
        if (Input.GetKey(KeyCode.S)) { movement.z -= 1; }
        if (Input.GetKey(KeyCode.A)) { movement.x -= 1; }
        if (Input.GetKey(KeyCode.D)) { movement.x += 1; }

        if (movement != Vector3.zero)
        {
            /*BoltNetwork.frameDeltaTime wraps Unity's Time.fixedDeltaTime*/
            transform.position = transform.position + 
                (movement.normalized * speed * BoltNetwork.frameDeltaTime);
        }
    }


    /* Takes our color from the state.CubeColor and assigns it to the 
     * Renderer's material.color property
     So in our game we can have players take dmg here?
         */
    void ColorChanged()
    {
        GetComponent<Renderer>().material.color = state.Cube2Color;
    }

    void OnGUI()
    {
        if (entity.isOwner)
        {
            GUI.color = state.Cube2Color;
            GUILayout.Label("@@@");
            GUI.color = Color.white;
        }
    }
}
