using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private float distanceBetweenBallAndMouseClickLimit = 1.5f; 
    private float distanceBetweenBallAndMouseClick;             
    private bool canRotate = false;                            

    void Update()
    {
        if (GameManager.singleton.gameStatus != GameStatus.Playing) 
            return; 

        if (Input.GetMouseButtonDown(0) && !canRotate)         
        {
            GetDistance();                                     
            canRotate = true;                                 

            if (distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
                BallController.instance.MouseDownMethod();       
        }

        if (canRotate)                                       
        {
            if (Input.GetMouseButton(0))                    
            {                                                  
                if (distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
                    BallController.instance.MouseNormalMethod(); 
                else
                    CameraRotation.instance.RotateCamera(Input.GetAxis("Mouse X"));
            }

            if (Input.GetMouseButtonUp(0))                     
            {
                canRotate = false;                             
                                                                
                if (distanceBetweenBallAndMouseClick <= distanceBetweenBallAndMouseClickLimit)
                    BallController.instance.MouseUpMethod();      
            }
        }
    }

    void GetDistance()
    {
        Plane plane = new Plane(Camera.main.transform.forward, BallController.instance.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
        float dist;  
        
        if (plane.Raycast(ray, out dist))
        {
            var v3Pos = ray.GetPoint(dist);                            
            distanceBetweenBallAndMouseClick = Vector3.Distance(v3Pos, BallController.instance.transform.position);
        }
    }

}
