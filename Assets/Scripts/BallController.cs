using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public static BallController instance;                 
    [SerializeField] LineRenderer lineRenderer;     
    [SerializeField] float MaxForce;                
    [SerializeField] float forceModifier = 0.5f;    
    [SerializeField] LayerMask groundLayer;            

    float clampedForce;                                   
    Rigidbody rb;                              
    Vector3 lastPos, groundPoint;
    bool canShoot = false, ballIsStatic = true;   
    Vector3 direction;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
        InitThrow();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InvisibleGround"))
        {
            print("tp");
            transform.position = lastPos;
        }
    }

    public void InitThrow()                                          
    {
        canShoot = false;
        lastPos = ClickedPoint();
        lineRenderer.gameObject.SetActive(true);                            
        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);  
    }

    public void ManageBallForce()                                         
    {
        groundPoint = ClickedPoint();
        float force = Vector3.Distance(groundPoint, transform.position) * forceModifier;
        clampedForce = Mathf.Clamp(force, 0, MaxForce);
        direction = transform.position - groundPoint;
        direction.y = 0;
        UIManager.instance.PowerSlider.value = clampedForce / MaxForce;    
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, groundPoint);  
    }

    public void ThrowBall()                                            
    {
        canShoot = true;                                                  
        lineRenderer.gameObject.SetActive(false);
        rb.AddForce(direction.normalized * clampedForce, ForceMode.Impulse);
        print(clampedForce);
        clampedForce = 0;
        UIManager.instance.PowerSlider.value = clampedForce;
        LevelManager.instance.NewShot();
    }

    Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;                              
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))    
            position = hit.point;

        position.y = transform.position.y;
        return position;                                               
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
            CameraRotation.instance.RotateCamera();

        if (rb.velocity == Vector3.zero)
        {
            rb.angularVelocity = Vector3.zero;

            if (Input.GetMouseButtonDown(0))
                InitThrow();
            if (Input.GetMouseButton(0))
                ManageBallForce();
            if (Input.GetMouseButtonUp(0))
                ThrowBall();
        }
    }
}
