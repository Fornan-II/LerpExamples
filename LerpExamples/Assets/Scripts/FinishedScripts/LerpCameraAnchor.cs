using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Finished
{
    public class LerpCameraAnchor : MonoBehaviour
    {
        [SerializeField] private Transform anchorTransform;
        [SerializeField] private float moveDuration = 0.7f;
        [SerializeField] private AnimationCurve camMovementCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

        private void OnTriggerEnter(Collider other)
        {
            // If the player enters this trigger, then move the camera to the anchorTransform
            // The camera's parent is set to the anchorTransform because if that anchorTransform was moving, you'd want the camera to still move with it after the camera's initial alignment.
            if (other.tag == "Player")
            {
                // This is a pattern of storing a reference to an active coroutine, and stopping any existing coroutines before starting a new one.
                // This ensures there is only one coroutine active at a time.
                // If I wasn't using this pattern in this situation, we could encounter problems where the Camera is trying to be animated to multiple locations at once, looking terrible.
                if (moveToRoutine != null)
                    StopCoroutine(moveToRoutine);

                moveToRoutine = StartCoroutine(MoveCamToAnchor());
            }
        }

        Coroutine moveToRoutine;

        private IEnumerator MoveCamToAnchor()
        {
            Transform camTransform = Camera.main.transform;
            // Save the initial position/rotation of the camera so we can use them as the A values in our lerps.
            // We could do what is done in MoveScript with velocity and the player's rotation, but this is just another way of using lerps that opens up some other options
            Vector3 initPos = camTransform.position;
            Quaternion initRot = camTransform.rotation;

            // This forloop is used to manage how long this coroutine lasts for.
            for (float timer = 0.0f; timer < moveDuration; timer += Time.deltaTime)
            {
                // We determine how far along the lerp we should be currently using an InverseLerp on the timer's values (the timer starts at 0, and ends at moveDuration).
                // We can then manipulate this tValue however we like
                float tValue = Mathf.InverseLerp(0.0f, moveDuration, timer);

                // Here we use the tValue to get a value from an AnimationCurve named camMovementCurve. AnimationCurve's are a cool Unity data type that serialized to the inspector.
                // Using an AnimationCurve here allows us to change our movement animation to whatever we want from the inspector very easily.
                tValue = camMovementCurve.Evaluate(tValue);
                // We don't have to use an AnimationCurve here, you can really use any equation you want that is takes in values from 0 to 1. Here are a few examples:
                // tValue = tValue * tValue; // Start slow, end fast
                // tValue = Mathf.Sqrt(tValue); // Start fast, end slow

                // We then use this modified tValue for the camera's position and rotation
                // Again, lerp for changing position, and slerp for changing rotation
                camTransform.position = Vector3.Lerp(initPos, anchorTransform.position, tValue);
                camTransform.rotation = Quaternion.Slerp(initRot, anchorTransform.rotation, tValue);
                yield return null;
            }

            // Still make sure camera's position and rotation are set to the anchorTransform's after lerping is done
            // The lerp could have potentially finished just before finishing, with a tValue of 0.9999 or something instead of 1, meaning your camera would not be aligned quite right.
            camTransform.position = anchorTransform.position;
            camTransform.rotation = anchorTransform.rotation;

            camTransform.parent = anchorTransform;

            // Set moveToRoutine to null because of the coroutine pattern mentioned on line 17
            // Coroutines don't automatically become null when they finish so we have to set it ourselves.
            moveToRoutine = null;
        }
    }
}