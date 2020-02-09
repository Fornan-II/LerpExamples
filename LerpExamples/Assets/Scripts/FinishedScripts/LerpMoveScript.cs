using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Finished
{
    public class LerpMoveScript : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private float maxVelocity = 5.0f;
        [SerializeField] private float moveLerpRate = 0.2f;
        [SerializeField] private float rotateSlerpRate = 0.2f;

        // FixedUpdate is called at a consistent interval defined by Physic's settings.
        // We're using physics, so this is the method that should be used.
        private void FixedUpdate()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //SimplePlayerMovement(input);
            LerpPlayerMovement(input);
        }

        private void SimplePlayerMovement(Vector2 inputVector)
        {
            // Set player's velocity to the input direction
            // Player only has control over their X Z movement, the Y velocity is controlled by gravity.
            Vector3 newVelocity = new Vector3(inputVector.x, rigidBody.velocity.y, inputVector.y) * maxVelocity;

            rigidBody.velocity = newVelocity;

            // Align the player's rotation to the new velocity
            // Setting the y component to 0 because we don't want the player to be facing up or down; we want them to stay aligned with the X Z plane.
            newVelocity.y = 0.0f;
            // Only align if the player's velocity is not zero, weird snapping occurs otherwise when no input.
            if (newVelocity != Vector3.zero)
                transform.forward = newVelocity;
        }

        private void LerpPlayerMovement(Vector2 inputVector)
        {
            Vector3 newVelocity = new Vector3(inputVector.x, rigidBody.velocity.y, inputVector.y) * maxVelocity;

            // Instead of setting velocity directly, have the velocity lerp to the newVelocity target over time.
            // The velocity moves 20% (or whatever value moveLerpRate is set to) of the way from it current velocity to the target velocity every physic's tick (0.02 seconds by default).
            // You can see a graph of how this works here: https://www.desmos.com/calculator/t4rbsrpj0u
            newVelocity = Vector3.Lerp(rigidBody.velocity, newVelocity, moveLerpRate);
            rigidBody.velocity = newVelocity;

            // Again, set y component to 0 so that the player stays aligned to the X Z plane.
            newVelocity.y = 0.0f;

            // Align the player using the same idea as velocity, except using slerp because we are changing the rotation of the player.
            if (newVelocity != Vector3.zero)
                transform.forward = Vector3.Slerp(transform.forward, newVelocity, rotateSlerpRate);
        }


#if UNITY_EDITOR
        // This is just my way of getting this gameobject's Rigidbody reference.
        // Similar to GetComponent in Start()
        private void OnValidate()
        {
            if (!rigidBody)
                rigidBody = GetComponent<Rigidbody>();
        }
#endif
    }
}
