using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    private void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.collider.tag == "Obstacle") // если тег коллайдера, с которым мы ударились = "Obstacle":
        {
            movement.enabled = false; //если мы стокнемся с obstacle, то скрипт на передвижение отключится.
            FindObjectOfType<GameManager>().EndGame(); 
        }
    }
}
