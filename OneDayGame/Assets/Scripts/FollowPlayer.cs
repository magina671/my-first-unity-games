using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // переменная player ссылается на Transform в Inspector
    public Vector3 offset; 
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset; // изменении позиции камеры за слежкой за игроком.
        //transform привязывает Transform(из Inspector) к GameObject
            
    }
}
