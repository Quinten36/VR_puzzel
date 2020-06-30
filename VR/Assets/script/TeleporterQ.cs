using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleporterQ : MonoBehaviour
{
    public GameObject q_pointer;
    public SteamVR_Action_Boolean q_teleportAction;

    private SteamVR_Behaviour_Pose q_pose = null;
    private bool q_HasPosition = false;

    private bool q_IsTeleporting = false;
    private float q_FadeTime = 0.5f;

    private void Awake()
    {
        q_pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    private void Update()
    {
        //pointer
        q_HasPosition = UpdatePointer();
        q_pointer.SetActive(q_HasPosition);

        //teleport
        if (q_teleportAction.GetStateUp(q_pose.inputSource))
        {
            TryTeleport();
        }
    }

    private void TryTeleport()
    {
        //check of in pos
        if (!q_HasPosition || q_IsTeleporting)
        {
            return;
        }

        // check of teleporting

        // get camera rig en pos
        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 hadPosition = SteamVR_Render.Top().head.position;

        // figure out translating
        Vector3 groundPos = new Vector3(hadPosition.x, cameraRig.position.y, cameraRig.position.z);
        Vector3 translateVector = q_pointer.transform.position - groundPos;

        // move
        StartCoroutine(MoveRig(cameraRig, translateVector));
    }

    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
    {
        // flag 
        q_IsTeleporting = true;
        //fade to black
        SteamVR_Fade.Start(Color.black, q_FadeTime, true);
        // tarnslate
        yield return new WaitForSeconds(q_FadeTime);
        cameraRig.position += translation;
        //fade to clear
        SteamVR_Fade.Start(Color.clear, q_FadeTime, true);
        //d-falg bool
        q_IsTeleporting = false;
       // yield return null;
    }

    private bool UpdatePointer()
    {
        // Ray from controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        // if its hit
        if (Physics.Raycast(ray, out hit))
        {
            q_pointer.transform.position = hit.point;
            return true;
        }

        // if not a hit

        return false;
    }
}
