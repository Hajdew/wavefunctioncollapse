using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class JumpController : MonoBehaviour {

	[Range(1, 100f)]
	public float JumpStrength = 2f;

	private CharacterController characterController;
	private Transform cameraTransform;

	private float verticalSpeed = 0f;
	private float timeInAir = 0f;
	private bool jumpLocked = false;

	public LayerMask CollisionLayers;

	void OnEnable() {
		this.characterController = this.GetComponent<CharacterController>();
	}
	
	void Update () {
		bool touchesGround = this.onGround();

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

		if (touchesGround) { 	
			this.timeInAir = 0;
		} 
        else 
        {
			this.timeInAir += Time.deltaTime;
		}

		if (Input.GetAxisRaw("Jump") < 0.1f) {
			this.jumpLocked = false;
		}
		if (!this.jumpLocked && this.timeInAir < 0.5f && Input.GetAxisRaw("Jump") > 0.1f) {
			this.timeInAir = 0.5f;
			this.verticalSpeed = this.JumpStrength;
			this.jumpLocked = true;
		}

		this.characterController.Move(Vector3.up * Time.deltaTime * this.verticalSpeed);
	}

	public void Enable() {
		this.verticalSpeed = 0;
	}

	private bool onGround() {
		var ray = new Ray(this.transform.position, Vector3.down);
		return Physics.SphereCast(ray, this.characterController.radius, this.characterController.height / 2 - this.characterController.radius + 0.1f, this.CollisionLayers);
	}
}
