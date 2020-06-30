using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class shootGun : MonoBehaviour
{

    public static shootGun instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than 1 buildmanager");
            return;
        }
        instance = this;
        shoot = false;
    }

    public static bool shoot;

    public static GameObject Bullet_Emitter;

    public GameObject Bullet;

    public float Bullet_force;

    public float Bullet_rotate;

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean releaseAction;
    public SteamVR_Action_Boolean test;

    private GameObject collidingObject;
    private GameObject objectInHand;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    // Update is called once per frame
    void Update()
    {
        // if (test.GetLastStateDown(handType))
        //{
       
        if (objectInHand)
        {
            // Debug.Log("sdf");
        }

        if (shoot)
        {
            Debug.Log("tets");
            GameObject Tijdelijke_bullet_handler;
            Tijdelijke_bullet_handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

            Tijdelijke_bullet_handler.transform.Rotate(Vector3.down * Bullet_rotate);

            Rigidbody Tijdelijke_bullet;
            Tijdelijke_bullet = Tijdelijke_bullet_handler.GetComponent<Rigidbody>();

            Tijdelijke_bullet.AddForce(transform.forward * Bullet_force);
            //this.transform.Translate (new Vector3 (5,5,5));
            //transform.forward * Bullet_force

            Destroy(Tijdelijke_bullet_handler, 2.0f);

            shoot = false;
            Bullet_Emitter = null;
            return;
        }


    }

    public void Shoot()
    {
        Debug.Log("op");
        //if (releaseAction.GetLastStateDown(handType))
        //{
            Debug.Log("tets");
            GameObject Tijdelijke_bullet_handler;
            Tijdelijke_bullet_handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

            Tijdelijke_bullet_handler.transform.Rotate(Vector3.down * Bullet_rotate);

            Rigidbody Tijdelijke_bullet;
            Tijdelijke_bullet = Tijdelijke_bullet_handler.GetComponent<Rigidbody>();

            Tijdelijke_bullet.AddForce(transform.forward * Bullet_force);
            //this.transform.Translate (new Vector3 (5,5,5));
            //transform.forward * Bullet_force

            Destroy(Tijdelijke_bullet_handler, 20.0f);
        //}
    }
    
}
