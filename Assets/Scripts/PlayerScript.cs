using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

    //Defining Numbers for aspects of character movement
    private float movementSpeed = 7.0f;
    private float mouseSensitivity = 2.0f;
    float verticalRotation = 0;
    private float visionYaxisRange = 60.0f;

    /// <summary>
    /// Used for intitalisation.
    /// Sets states of the cursor
    /// </summary>
    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        GetComponent<MeshRenderer>().enabled = false;
    }
	
	/// <summary>
    /// Update is called once per frame.
    /// Code for player movement and camera control.
    /// </summary>
	void Update () {
        float RotationLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, RotationLeftRight, 0);

        //Ability to change the mouse sensitivity in-game via pressing the 1 key
        //or the 2 key
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (mouseSensitivity >= 1.0f)
                mouseSensitivity = mouseSensitivity - 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (mouseSensitivity <= 9.5f)
                mouseSensitivity = mouseSensitivity + 0.5f;
        }

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -visionYaxisRange, visionYaxisRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        float verticalSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float horizontalSpeed = Input.GetAxis("Horizontal") * movementSpeed;

        Vector3 speed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        speed = transform.rotation * speed;
        CharacterController cc = GetComponent<CharacterController>();

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            cc.SimpleMove(speed*1.5f);
        }
        else
        {
            cc.SimpleMove(speed);
        }
    }
}
