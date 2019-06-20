using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private List<Transform> rings = new List<Transform>();

    public Material activeRing;
    public Material inactiveRing;
    public Material finalRing;

    private int ringPassed = 0;

    private void Start()
    {
        //set the objective field in the game scene 
        FindObjectOfType<GameScene>().objective = this; 

        //at the start of the level , assign inactive to all rings 
        foreach(Transform t in transform)
        {
            rings.Add(t);
            t.GetComponent<MeshRenderer>().material = inactiveRing;
        }

        //making sure we're not stupid
        if (rings.Count == 0)
        {
            Debug.Log("There is no objectives assigne don this level , make sure you put some rings under the Objective Object");
            return;
        }

        //activate the first ring
        rings[ringPassed].GetComponent<MeshRenderer>().material = activeRing;
        rings[ringPassed].GetComponent<Ring>().ActivateRing();
    }

    public void NextRing()
    {
        //play FX on the current ring
        rings[ringPassed].GetComponent<Animator>().SetTrigger("collectionTrigger");

        //up the int
        ringPassed++;

        //if it is the final ring, let's call the victory
        if (ringPassed == rings.Count)
        {
            Victory();
            return;
        }

        //if this is the previous last, give the next ring the "Final ring" material
        if(ringPassed == rings.Count - 1)
            rings[ringPassed].GetComponent<MeshRenderer>().material = finalRing;
        else
            rings[ringPassed].GetComponent<MeshRenderer>().material = activeRing;

        //in both cases, we need to activate the ring!
        rings[ringPassed].GetComponent<Ring>().ActivateRing();
    }

    public Transform GetCurrentRing()
    {
        return rings[ringPassed];
    }

    private void Victory()
    {
        FindObjectOfType<GameScene>().CompleteLevel();
    }
}
