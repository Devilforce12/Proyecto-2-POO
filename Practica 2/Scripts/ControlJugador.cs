using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    public float velocidad = 5f; //velocidad del jugardor
    public float gravedad = -9.8f; //la gravedad
    private CharacterController controller; //nos permite registrar el movimiento
    private Vector3 velocidadVertical; //nos permite saber que rapido caemos

    public Transform camara;//registra la camara que va a ser los ojos del jugador
    public float sensibilidadMouse = 500f; //movimiento de camara / sensibilidad del Mouse
    private float rotacionXVertical = 0f;//los grados que puede voltear a ver el jugador

    void Start()
    {
        controller = GetComponent<CharacterController>(); //busca el conponenete controller

        //Esta linea bloquea el puntero del mouse en los limites de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        ManejadorVista();
        ManejadorMovimiento();
    }

    void ManejadorVista()
    {
        //1. leer el input del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;
        //2. construir la rotación horizontal 
        transform.Rotate(Vector3.up * mouseX);
        //3. registro de la rotación vertical
        rotacionXVertical -= mouseY;
        //4. limitar la rotación vertical
        Mathf.Clamp(rotacionXVertical, -90f, 90f);
        //5. aplicar la rotacion vertical               x          y  z
        camara.localRotation = Quaternion.Euler(rotacionXVertical, 0, 0);
    }

    void ManejadorMovimiento()
    {
        //1. leer el input del mouse (WASD o la flachas de direccion)
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        //2. crear el vector de movimiento 
            //se almacena de forma local el registro de dirrección de movimiento 
        Vector3 direccion = transform.right * inputX + transform.forward * inputZ;
        //3. mover el CharacterController
        controller.Move(direccion * velocidad * Time.deltaTime);
        //4. Aplicar la gravedad al jugador
            //registro si estoy en el piso para que en un futuro comportamiento de salto 
        if(controller.isGrounded && velocidadVertical.y <0)
        {
            velocidadVertical.y = -2f;//una pequeña fuerza hacia abajo para mantenerlo pegado al piso
        }
        //Aplicamos la aceleracion de la gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        //movemos el controlador hacia abajo
        controller.Move(velocidadVertical * Time.deltaTime);
    }

}//NO TE PASES DE AQUI
