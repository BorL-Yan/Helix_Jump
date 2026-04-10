using System;
using UnityEngine;

namespace Ball.Controller
{
    public class BallShadowVisibilityController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _root;

        [SerializeField] private float _maxShadowAlpha;
        [SerializeField] private float _maxViewDistance;

        [SerializeField] private LayerMask _layerMask;

       
        private void Update()
        {
            Debug.DrawLine(_root.transform.position, _root.transform.position + Vector3.down * _maxViewDistance, Color.blue);
            if (Physics.Raycast(_root.transform.position, Vector3.down, out var hit, _maxViewDistance, _layerMask))
            {
                if (hit.transform.name == "EmptyCollider")
                {
                    _spriteRenderer.color = Color.black * 0;
                    return;
                }
                _spriteRenderer.transform.position = hit.point;// + new Vector3(0, 0.01f, 0);
                float alphaDistance = Mathf.InverseLerp(0, _maxViewDistance, hit.distance);
                _spriteRenderer.color = new Color(0, 0, 0, Mathf.Lerp(_maxShadowAlpha, 0, alphaDistance));
                _spriteRenderer.transform.localScale = Vector3.one * Mathf.Lerp(0.08f, 0, alphaDistance);
            }
            else
            {
                _spriteRenderer.color = Color.black * 0;
            }
        }
    }
}