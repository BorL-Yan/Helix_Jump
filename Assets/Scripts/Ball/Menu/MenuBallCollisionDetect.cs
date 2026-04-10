using System;
using Ball.Controller;
using UnityEngine;

namespace Ball.Menu
{
    public class MenuBallCollisionDetect : MonoBehaviour
    {
        private bool _activated;
        [SerializeField] private MenuBallController _ballController;

        private void Start()
        {
            _activated = true;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!_activated) return;
            _activated = false;
            Invoke("DiactivateCollision", 0.1f);

            switch (collider.transform.tag)
            {
                case "Platform":
                {
                    _ballController.Jump();
                    SoundManager.Instance.Play(SoundType.Jump, 0.3f);
                    break;
                }
            }
        }
        
        private void DiactivateCollision()
        {
            _activated = true;
        }
    }
}