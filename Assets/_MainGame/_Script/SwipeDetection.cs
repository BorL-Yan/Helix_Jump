using UnityEngine;

public class SwipeDetection : MonoBehaviour
{   
    //slide dnel popoxakani ev detectioni anun@
    public static event OnSwipeInput ControllerMoveDelta;
    public delegate void OnSwipeInput(Vector2 direction);

    private Vector2 _tapPosition;
    private Vector2 _controllerMoveDelta;

    private bool _isMobile;
    void Start()
    {
        QualitySettings.vSyncCount = 0;     //  
        Application.targetFrameRate = 200;   //
        _isMobile = Application.isMobilePlatform;
    }
    void Update()
    {
        if (!_isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _tapPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                _controllerMoveDelta = (Vector2)Input.mousePosition/Screen.width - _tapPosition/Screen.width;
                _tapPosition = Input.mousePosition;

                ControllerMoveDelta?.Invoke(_controllerMoveDelta);
            }

        }
        else
        {
            if (Input.touchCount > 0)
            {
               
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    
                    _tapPosition = Input.GetTouch(0).position;                    
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {

                    _controllerMoveDelta = Input.GetTouch(0).position/Screen.width - _tapPosition/Screen.width;
                    _tapPosition = Input.GetTouch(0).position;
                    ControllerMoveDelta?.Invoke(_controllerMoveDelta);
                }
            }    
        }     
    } 
}
