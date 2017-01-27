﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.NPC
{
    public class GeneralInteraction : MonoBehaviour
    {
        private Transform _objectToLookAt = null;
        private bool _isRotating = false;
        private Quaternion _defaultRotation;

        public void Start()
        {
            //Store the default rotation of the NPC in case it is rotated towards the player object later.
            _defaultRotation = transform.rotation;

        }

        public void Update()
        {
            if (_isRotating)
            {
                RotateTowards(2f);
            }
        }

        /// <summary>
        /// This is a Monobehavior callback that happens when the player enters a collider that is set to type "trigger"
        /// To enable this, a collider needs to be added to the game object with the "Is Trigger" option selected.
        /// Look at the Sphere Collider component on the Merchant game object in Unity to see how it was setup.
        /// Another requirement is that the Player GO needs to have a Rigidbody and Collider on it. 
        /// We'll talk about a Rigidbody in more detail in a future episode.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            //The Player Game Object needs to have the Tag set to "Player" for this check to work.
            if (other.gameObject.tag.Equals("Player"))
            {
                _objectToLookAt = other.transform;
                _isRotating = true;
            }
        }

        /// <summary>
        /// This is a monobehavior callback that happens when the player exits a collider that is set to type "trigger".
        /// To enable this, a collider needs to be added to the game object with the "Is Trigger" option selected.
        /// Look at the Sphere Collider component on the Merchant game object in Unity to see how it was setup.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            _objectToLookAt = null;
            _isRotating = true;
        }

        /// <summary>
        /// This method is similiar to what was written on the episode. 
        /// The differences are:
        ///   1. Removed the distance check, since it's triggered only after the player has entered the area.
        ///   2. Checked to see if we have an object to look at. If not, it rotates back to default.
        /// </summary>
        /// <param name="speed">How quickly the rotation will occur.</param>
        public void RotateTowards(float speed)
        {
            Quaternion lookRotation;

            if (_objectToLookAt != null)
            {
                Vector3 direction = (_objectToLookAt.position - transform.position).normalized;
                 lookRotation = Quaternion.LookRotation(direction);
            }
            else
            {
                lookRotation =  _defaultRotation;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

            if (Quaternion.Angle(transform.rotation, lookRotation) < 1)
            {
                _isRotating = false;
            }
        }
    }
}
