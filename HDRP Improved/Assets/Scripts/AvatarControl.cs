using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarControl : MonoBehaviour
{
	[SerializeField]float jumpSpeed = 5f;
	[SerializeField]float walkSpeed = 3f;
	[SerializeField]float gravity = 3f;
	[SerializeField]float walkDirectionChangeFactor = 4f;
	[SerializeField]float rotationAngularSpeed = Mathf.PI;
	Vector3 speed;

	CharacterController characterController;
	void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// Check jump
		if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			speed.y = jumpSpeed;
		}

		// Calculate walkspeed
		Vector3 walkDirection = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
			{ walkDirection.z = 1f; }
		else if (Input.GetKey(KeyCode.S))
			{ walkDirection.z = -1f; }

		if (Input.GetKey(KeyCode.A))
			{ walkDirection.x = -1f; }
		else if (Input.GetKey(KeyCode.D))
			{ walkDirection.x = 1f; }

		walkDirection.Normalize();
		walkDirection = Camera.main.transform.TransformDirection(walkDirection);
		walkDirection.y = 0f;
		walkDirection.Normalize();
		walkDirection *= walkSpeed;

		// Interpolate speed (inertia)
		Vector3 planarSpeed = speed;
		planarSpeed.y = 0;
		planarSpeed = (walkDirection - planarSpeed) / walkDirectionChangeFactor;
		speed += planarSpeed;

		// Apply gravity
		speed.y += gravity * Time.deltaTime;

		// Move character
        characterController.Move(speed * Time.deltaTime);

		// Rotate character towards
		// camera forward
		Vector3 camForward = Camera.main.transform.forward;
		camForward.y = 0f;

		planarSpeed = speed;
		planarSpeed.y = 0f;
		Vector3 newCharacterForward = Vector3.RotateTowards(transform.forward, camForward, rotationAngularSpeed * Time.deltaTime * (planarSpeed.magnitude / walkSpeed) , 0f);

		// Ugly
		Vector3 pointToLookAt = transform.position + newCharacterForward;
		transform.LookAt(pointToLookAt);
    }
}
