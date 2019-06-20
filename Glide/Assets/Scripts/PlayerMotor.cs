using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private float baseSpeed = 9.0f;
    private float rotSpeedX = 7.0f;
    private float rotSpeedY = 5.0f;

    private float deathTime;
    private float deathDuration = 2;

    public GameObject deathExplosion;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
 
        //create the trail
        GameObject trail = Instantiate(Manager.Instance.playerTrails[SaveManager.Instance.state.activeTrail]);

        //set the trail as a children of a model
        trail.transform.SetParent(transform.GetChild(0));

        //fix the rotation of the trail
        trail.transform.localEulerAngles = Vector3.forward * -90f;
    }

    private void Update()
    {
        //if player is dead ( has a deathtime)
        if (deathTime != 0)
        {
            //wait x seconds then restart the level
            if(Time.time - deathTime > deathDuration)
            {
                SceneManager.LoadScene("Game");
            }

            return;
        }

        //give the player forward velocity 
        Vector3 moveVector = transform.forward * baseSpeed;

        //gather players input 
        Vector3 inputs = Manager.Instance.GetPlayerInput();

        //get the delta direction 
        Vector3 yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

        //make sure we limit the player from doing a loop 
        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;

        //if hes not going too far up/down, add the ditrection to moveVector
        if (maxX < 90 && maxX > 70 || maxX < 290 && maxX > 270)
        {
            //Too far , don not do anything
        }
        else
        {
            //add the direction to the current move
            moveVector += dir;

            // have the player face where he is going 
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        //move him!
        controller.Move(moveVector * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //set a death timestamp
        deathTime = Time.time;

        //player explosion effect
        GameObject go = Instantiate(deathExplosion) as GameObject;
        go.transform.position = transform.position;

        //hide player mesh
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
