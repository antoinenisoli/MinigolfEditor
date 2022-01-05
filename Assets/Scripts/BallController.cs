using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public static BallController instance;                 
    [SerializeField] LineRenderer lineRenderer;     
    [SerializeField] float MaxForce;                
    [SerializeField] float forceModifier = 0.5f;    
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] Transform visualChildren;

    float clampedForce;                                   
    Rigidbody rb;                              
    Vector3 lastPos, groundPoint;
    Vector3 direction;
    public MeshOutline meshOutline;
    Camera mainCam => CameraFollow.instance.cam;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
        meshOutline = GetComponentInChildren<MeshOutline>();
        lastPos = transform.position;

        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);
        lineRenderer.SetPosition(1, lineRenderer.transform.localPosition);
        lineRenderer.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        visualChildren.DOComplete();
        visualChildren.DOShakeScale(0.2f, 0.5f, 50);

        if (collision.gameObject.CompareTag("InvisibleGround"))
        {
            print("tp");
            Stop();
            transform.position = lastPos;
        }
    }

    public void InitThrow()                                          
    {
        lineRenderer.gameObject.SetActive(true);                            
        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);  
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
        clampedForce = 0;
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
        visualChildren.DOComplete();
        visualChildren.DOShakeScale(0.2f, 0.5f, 50);

        lastPos = transform.position;
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
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);    
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))    
            position = hit.point;

        position.y = transform.position.y;
        return position;                                               
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
            CameraRotation.instance.RotateCamera();

        if (rb.velocity == Vector3.zero && GameManager.instance.gameStatus == GameStatus.Playing)
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

    private void FixedUpdate()
    {
        meshOutline.enabled = Physics.Linecast(mainCam.transform.position, transform.position, obstacleLayer);
    }
}
