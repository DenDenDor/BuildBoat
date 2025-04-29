using UnityEngine;

public class AdvancedBuoyancy : MonoBehaviour
{
    public Transform[] floaters;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public float waterHeight = 0f;
    
    Rigidbody rb;
    int floatersUnderwater;
    bool underwater;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        floatersUnderwater = 0;
        
        foreach(Transform floater in floaters)
        {
            float difference = floater.position.y - waterHeight;
            
            if(difference < 0)
            {
                rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floater.position, ForceMode.Force);
                floatersUnderwater += 1;
                
                if(!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
        }
        
        if(underwater && floatersUnderwater == 0)
        {
            underwater = false;
            SwitchState(false);
        }
    }
    
    void SwitchState(bool isUnderwater)
    {
        if(isUnderwater)
        {
            rb.linearDamping = underWaterDrag;
            rb.angularDamping = underWaterAngularDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
            rb.angularDamping = airAngularDrag;
        }
    }
}