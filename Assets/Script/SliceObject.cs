using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class SliceObject : MonoBehaviour
{
/*    [SerializeField] private GameObject Knife;
*/    [SerializeField] private GameObject slice;

    [SerializeField] Material cross;

    [SerializeField] private Transform StartSlicePoint;
    [SerializeField] private Transform EndSlicePoint;
    [SerializeField] private LayerMask Slicemask;

    public VelocityEstimator velo;
    void Start()
    {
        
    }

    void Update()
    {
        bool hit = Physics.Linecast(StartSlicePoint.position, EndSlicePoint.position, out RaycastHit hit1, Slicemask);
        if (hit)
        {

            GameObject target = hit1.transform.gameObject;  
            Slice(target);
        } 
    }

    private void Slice(GameObject Target)
    {
        Vector3 velocity = velo.GetVelocityEstimate();
        
        Vector3 planenormal = Vector3.Cross(EndSlicePoint.position - StartSlicePoint.position, velocity);
       
        planenormal.Normalize();
        Debug.Log(planenormal);
        SlicedHull hull = Target.Slice(EndSlicePoint.position, planenormal);

        Debug.Log(Target.transform.gameObject.name);
        if (hull != null) { 
            GameObject upper = hull.CreateUpperHull(Target,Target.GetComponent<MeshRenderer>().material);
            upper.layer = LayerMask.NameToLayer("Swort");
            SetupSliceComponent(upper);
            GameObject lower = hull.CreateLowerHull(Target, Target.GetComponent<MeshRenderer>().material);
            lower.layer = LayerMask.NameToLayer("Swort");

            SetupSliceComponent(lower);

            Destroy(Target);
        }
        else
        {
            Debug.Log("Gagal");
        }
    }


    public void SetupSliceComponent(GameObject sliceobject)
    {

    
        Rigidbody rb = sliceobject.AddComponent<Rigidbody>();
        MeshCollider collider = sliceobject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(200f, sliceobject.transform.position, 1);
    }
}
