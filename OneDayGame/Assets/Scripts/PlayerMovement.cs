using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //ссылка на RigidBody компонента "rb"
    public Rigidbody rb;
    public float forwardForce = 800f;
    public float sidewaysForce = 600; //боковая скорость

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("<color=green>Hello World!</color>");
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime); 
        //addForce - добавляет крутящийся момент к физическому телу.
        //Time.deltatime - time between frames

        //поворот вправо
        if (Input.GetKey("d")) // 
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange); 
            //velocityChange = Добавить физическому телу мгновенное изменение скорости, игнорируя его массу. Доавляет мгновенную скорость, то есть не надо ждать окончания кадра, то сработал AddForce
        }
        //поворот влево
        if (Input.GetKey("a"))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if(rb.position.y < -1f) //если упал с платформы = проиграл.
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
