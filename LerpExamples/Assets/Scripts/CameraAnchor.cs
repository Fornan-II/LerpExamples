using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    [SerializeField] private Transform anchorTransform;

    private void OnTriggerEnter(Collider other)
    {
        //If the player enters this trigger, then move the camera to the anchorTransform
        // The camera's parent is set to the anchorTransform because if that anchorTransform was moving, you'd want the camera to still move with it after the camera's initial alignment.
        if (other.tag == "Player")
        {
            //Make camTransform instead of calling Camera.main over and over again because Camera.main is expensive
            //Don't use Camera.main in real situations, I only use it here for simplicity
            Transform camTransform = Camera.main.transform;

            camTransform.position = anchorTransform.position;
            camTransform.rotation = anchorTransform.rotation;

            camTransform.parent = anchorTransform;
        }
    }
}
