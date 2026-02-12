using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{   
    [SerializeField] private float _forceImpulse;
    [SerializeField] private Transform _scaleTarget;
    [SerializeField] private Transform _mainCamera;
    [SerializeField] private Transform _cameraAttachTarget;
    [SerializeField] private GameObject _ballMarkerPrefab;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private long _vibrationTime = 50;
    [SerializeField] private int _vibrationAmplitude = 150;
    [SerializeField] private float _jumpAnimationDuration = 0.4f;
    [SerializeField] private RotationBallRenderer _rotationBallRenderer;

    [SerializeField] private Slider _sliderVibraTime;
    [SerializeField] private Slider _sliderVibraAmplitude;

    private bool _activated = false;
    private bool _ignoreNextCollision = false;

    private VibrationCollision _vibrationCollision;
    private PlatformBoom _platformBoom; 
    private CameraTriggerAttachment _cameraTriggerAttachment;
    private BallMarker _ballMarker;
    private MarkerAttachment _ballMarkerAttachment;
    private BallAnimation _ballAnimation;

    
   
    private void Awake()
    {
        _sliderVibraTime.onValueChanged.AddListener(OnTimeChanged);
        _sliderVibraAmplitude.onValueChanged.AddListener(OnAmplitudeChanged);        
        _ballAnimation = new BallAnimation(_scaleTarget, _jumpAnimationDuration);
        _vibrationCollision = new VibrationCollision();
        //_platformBoom = new PlatformBoom(_forceImpulse);
        _cameraTriggerAttachment = new CameraTriggerAttachment(_mainCamera, _cameraAttachTarget);        
        _ballMarker = new BallMarker(_ballMarkerPrefab);
        _ballMarkerAttachment = new MarkerAttachment();

    }
    private void  OnCollisionEnter(Collision collision)
    {
        if (_activated) return;
        _activated = true;
        DoActiaonCollision(collision);
        _rotationBallRenderer.jumprotation();
        _ballAnimation.Jump(_scaleTarget);
        
        Invoke("AllowCollision", 0.1f);   
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_activated) return;  
        _activated = true;
        DoActionTriger(other);
        Invoke("AllowCollision",0.1f);
    }
    private void DoActiaonCollision(Collision collision)
    { 
        if (_ignoreNextCollision)
            return;
        _vibrationCollision.VibrationMethod(_vibrationTime,_vibrationAmplitude);
        GameObject marker = _ballMarker.AttachMarker(collision);
        _ballMarkerAttachment.TieMarker(collision, marker);
        _cameraTriggerAttachment.Detach();
        _ignoreNextCollision = true;        
    }
 
    private void DoActionTriger(Collider other)
    { 
        if (other.CompareTag("TrigerFloor"))
        {
            _cameraTriggerAttachment.Attach();
        }
        GameObject parent = other.gameObject.transform.parent.gameObject;
        //_platformBoom.BoomMethod(parent);

    }  
    private void AllowCollision()
    {
        _ignoreNextCollision = false;
        _activated = false;
    }

    public void OnTimeChanged(float value)
    {
        value *= 500f;
        _vibrationTime = (long)value;
        Debug.Log(_vibrationTime);
    }
    public void OnAmplitudeChanged(float value)
    {
        value *= 255f;
        _vibrationAmplitude = (int)value ;
    }
}