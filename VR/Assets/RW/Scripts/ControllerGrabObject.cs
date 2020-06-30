/*
 * Copyright (c) 2018 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using Valve.VR;


public class ControllerGrabObject : MonoBehaviour
{

    public static ControllerGrabObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than 1 buildmanager");
            return;
        }
        instance = this;
    }

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean releaseAction;
    public SteamVR_Action_Boolean test;
    public bool gepakt; 

    private GameObject collidingObject;
    private GameObject objectInHand;

    public GameObject emit;

    public GameObject gun;
    public GameObject guns;
    public bool enShoot;

    public void Start()
    {
        gepakt = false;
        enShoot = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "gun")
        {
            gun = guns.transform.Find("gun").gameObject;
            SetCollidingObject(other);
            Debug.Log("stay");
            if (releaseAction.GetLastStateDown(handType))
            {
                Debug.Log("oof");
                //if (enShoot)
                //{
                Debug.Log("sdf");

                emit = other.transform.Find("emiter").gameObject;
                Debug.Log(emit);
                shootGun.Bullet_Emitter = emit;
                shootGun.shoot = true;

                //}
            } else
            {
                emit = null;
            }
        }
        
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

    void Update()
    {
       

        if (test.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
                Debug.Log("pakken");
               // gepakt = true;
            }
        }

        if (collidingObject)
        {
            
        } else
        {
            enShoot = false;
        }

        if (releaseAction.GetLastStateDown(handType))
        {
            // enShoot = true;
            Debug.Log("release");
        }

      

            if (grabAction.GetLastStateDown(handType))
        {
            // enShoot = true;
            Debug.Log("grab");
        }

        if (test.GetLastStateDown(handType))
        {
            Debug.Log("test");
            if (collidingObject)
            { 
                GrabObject();
            }
        }

       

//        if (releaseAction.GetLastStateDown(handType))
//        {
//            if (objectInHand)
//            {
//                ReleaseObject();
//            }
//        }
    }

 //   private void OnTriggerEnter(Collider other)
//    {
//        if (gepakt)
//        {
 //           if (releaseAction.GetLastStateDown(handType))
  //          {
  //
    //            Debug.Log("trigger");
      //          if (gameObject.tag == "gun")
        //        {
          //          shootGun.shoot = true;
            //    }
              //  if (gameObject.tag == "gun1")
                //{
                  //  shootGun1.shoot = true;
                //}
                //if (gameObject.tag == "gun2")
              //  {
            //        //shootGun2.shoot = true;
          //          Debug.Log("random error");
        //        }
      //          // shootGun.shoot = true;
    //        }
  //      }
//    }

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

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            gepakt = false;
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
       
        objectInHand = null;
    }
}
