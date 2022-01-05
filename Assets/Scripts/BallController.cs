using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public static BallController instance;                 
    [SerializeField] LineRenderer lineRenderer;     
    [SerializeField] float MaxForce;                
    [SerializeField] float forceModifier = 0.5f;    
    [SerializeField] GameObject areaAffector;       
    [SerializeField] LayerMask groundLayer;            

    float force;                                   
    Rigidbody rb;                              
    Vector3 startPos, endPos;
    bool canShoot = false, ballIsStatic = true;   
    Vector3 direction;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody>();                 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Destroyer")                             
            LevelManager.instance.LevelFailed();                   
        else if (other.name == "Hole")                            
            LevelManager.instance.LevelComplete();                 
    }

    public void MouseDownMethod()                                          
    {
        if(!ballIsStatic) 
            return;             
        
        startPos = ClickedPoint();                                          
        lineRenderer.gameObject.SetActive(true);                            
        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);  
    }

    public void MouseNormalMethod()                                         
    {
        if(!ballIsStatic) 
            return;  
        
        endPos = ClickedPoint();                                               
        force = Mathf.Clamp(Vector3.Distance(endPos, startPos) * forceModifier, 0, MaxForce);  
        UIManager.instance.PowerBar.fillAmount = force / MaxForce;              
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(endPos));  
    }

    public void MouseUpMethod()                                            
    {
        if(!ballIsStatic) 
            return; 
        
        canShoot = true;                                                  
        lineRenderer.gameObject.SetActive(false);                          
    }

    Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;                              
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))    
            position = hit.point;                                      

        return position;                                               
    }

    void Update()
    {
        if (rb.velocity == Vector3.zero && !ballIsStatic)
        {
            ballIsStatic = true;
            LevelManager.instance.ShotTaken();
            rb.angularVelocity = Vector3.zero;
            areaAffector.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (canShoot)
        {
            canShoot = false;
            ballIsStatic = false;
            direction = startPos - endPos;
            rb.AddForce(direction * force, ForceMode.Impulse);
            areaAffector.SetActive(false);
            UIManager.instance.PowerBar.fillAmount = 0;
            force = 0;
            startPos = endPos = Vector3.zero;
        }
    }
}
