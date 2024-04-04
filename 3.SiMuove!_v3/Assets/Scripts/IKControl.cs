using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;

using System.Reflection;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour 
{
    protected Animator animator;
    
    public bool ikActive = false;
    public string jsonFilePath; 

    JSONfile frame;

    Vector3 pRHand;
    Vector3 pLHand;
    Vector3 pRFoot;
    Vector3 pLFoot;
    // Vector3 pRElbow;
    // Vector3 pLElbow;
    // Vector3 pRKnee;
    // Vector3 pLKnee;

    List<float> keypoints_2d;
    List<float> old_keypoints_2d;

    int t = 0;
    int counter = 0;

    private bool flag = false;
    
    //--------------------------------------
    public GameObject Hips;
    public GameObject LeftLeg;   // decommentare per calibrazione automatica
    public GameObject RigthLeg;  // decommentare per calibrazione automatica
    public GameObject LeftFoot;
    public GameObject RigthFoot;
    public GameObject LeftHand;
    public GameObject RigthHand;

    Vector3 _init_HipsPos;
    Vector3 _init_LeftLegPos;     // decommentare per calibrazione automatica
    Vector3 _init_RigthLegPos;    // decommentare per calibrazione automatica
    Vector3 _init_LeftFootPos;
    Vector3 _init_RigthFootPos;
    Vector3 _init_LeftHandPos;
    Vector3 _init_RigthHandPos;
    //--------------------------------------

    private void Start()
    {
        animator = GetComponent<Animator>();

        _init_HipsPos = Hips.transform.position;

        _init_LeftFootPos = LeftFoot.transform.position;
        _init_RigthFootPos = RigthFoot.transform.position;
        _init_LeftHandPos = LeftHand.transform.position;
        _init_RigthHandPos = RigthHand.transform.position;
    }    

    private void Update()
    {
        if(jsonFilePath != null)
        {
            // if(Input.GetKeyDown(KeyCode.L))     // bisogna premere ad ogni frame per andare avanti
            if(Input.GetKey(KeyCode.L))         // si può tenere premuto per andare avanti --- da modificare ulteriormente con l'altra negazione se non è premuto
            // inserire il ciclo automatico col tempo
            {
                string iframe = counter.ToString("000000000000");
                if(System.IO.File.Exists(jsonFilePath + "/soldatino_" + iframe + "_keypoints.json"))
                {
                    string mocapFrame = System.IO.File.ReadAllText(jsonFilePath + "/soldatino_" + iframe + "_keypoints.json"); 
                    if(mocapFrame != null)
                    {
                        // ottieni mocap  
                        frame = JsonUtility.FromJson<JSONfile>(mocapFrame);

                        keypoints_2d = frame.people[0].pose_keypoints_2d;
                        for (int i=0; i<keypoints_2d.Count/3; i++)
                        {
                            keypoints_2d[(i*3)+1]=-keypoints_2d[(i*3)+1];
                        }
                        
                        float RfootXPos = frame.people[0].pose_keypoints_2d[11*3];
                        float LfootXPos = frame.people[0].pose_keypoints_2d[14*3];
                        float RhandXPos = frame.people[0].pose_keypoints_2d[4*3];
                        float LhandXPos = frame.people[0].pose_keypoints_2d[7*3];

                        if(RfootXPos==0 || LfootXPos==0 || RhandXPos==0 || LhandXPos==0)
                        {
                            if(RfootXPos==0)
                            {
                                keypoints_2d[(11*3)+1]=old_keypoints_2d[(11*3)+1];
                                keypoints_2d[(11*3)]=old_keypoints_2d[(11*3)];
                            }
                            if(LfootXPos==0)
                            {
                                keypoints_2d[(14*3)+1]=old_keypoints_2d[(14*3)+1];
                                keypoints_2d[(14*3)]=old_keypoints_2d[(14*3)];
                            }
                            if(RhandXPos==0)
                            {
                                keypoints_2d[(4*3)+1]=old_keypoints_2d[(4*3)+1];
                                keypoints_2d[(4*3)]=old_keypoints_2d[(4*3)];
                            }
                            if(LhandXPos==0)
                            {
                                keypoints_2d[(7*3)+1]=old_keypoints_2d[(7*3)+1];
                                keypoints_2d[(7*3)]=old_keypoints_2d[(7*3)];
                            }
                        }
                        old_keypoints_2d = keypoints_2d.ConvertAll(point => (float)point); 
                        t = 1;

                        // PER NORMALIZZARE: INIZIO -------------------------------------------------------------------

                        //----------------------------------------------------------------------
                        //ottieni posizioni gambe del mocap per calcolo lunghezza bacino
                        float RLegXPos = keypoints_2d[9*3];
                        float RLegYPos = keypoints_2d[(9*3)+1];
                        float LLegXPos = keypoints_2d[12*3];
                        float LLegYPos = keypoints_2d[(12*3)+1];
                        float mocapDistance = (float)Math.Sqrt(  Math.Pow( RLegXPos-LLegXPos, 2f) +  Math.Pow( RLegYPos-LLegYPos, 2f) );
                        
                        //ottieni posizioni gambe dell'avatar per calcolo lunghezza bacino
                        _init_RigthLegPos = RigthLeg.transform.position;
                        _init_LeftLegPos = LeftLeg.transform.position;
                        float avatarDistance = (float)Math.Sqrt(  Math.Pow( _init_RigthLegPos.x-_init_LeftLegPos.x, 2f) +  Math.Pow( _init_RigthLegPos.y-_init_LeftLegPos.y, 2f));

                        float scale = avatarDistance/mocapDistance;
                        for (int i=0; i<keypoints_2d.Count/3; i++)
                        {
                            keypoints_2d[i*3]=keypoints_2d[i*3]*scale;
                            keypoints_2d[(i*3)+1]=keypoints_2d[(i*3)+1]*scale;
                        }

                        //----------------------------------------------------------------------
                        //confronto posizione ombelico
                        float hipsXPos = keypoints_2d[8*3];
                        float hipsYPos = keypoints_2d[(8*3)+1];

                        _init_HipsPos = Hips.transform.position;

                        float xDistHips = _init_HipsPos.x - hipsXPos;
                        float yDistHips = _init_HipsPos.y - hipsYPos;
                        for (int i=0; i<keypoints_2d.Count/3; i++)
                        {
                            keypoints_2d[i*3]=keypoints_2d[i*3]+xDistHips;
                            keypoints_2d[(i*3)+1]=keypoints_2d[(i*3)+1]+yDistHips;
                        }
                        // PER NORMALIZZARE: FINE -------------------------------------------------------------------
       
                        if( !flag )
                        {
                            StartCoroutine( CounterUpAfterWaiting() );
                        }

                    }
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }
        }
    }

    private IEnumerator CounterUpAfterWaiting()
    {
        flag = true;
        yield return new WaitForSeconds(0.05f);
        counter = counter +1;
        flag = false;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if(animator) 
        {
            //if the IK is active, set the position and rotation directly to the goal.
            if(ikActive) 
            {
                if (t==1)
                {

                    //IK Goal
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
                    pRHand.x = keypoints_2d[4*3];
                    pRHand.y = keypoints_2d[(4*3)+1];
                    pRHand.z = _init_RigthHandPos.z;
                    animator.SetIKPosition(AvatarIKGoal.RightHand,pRHand);

                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
                    pLHand.x = keypoints_2d[7*3];
                    pLHand.y = keypoints_2d[(7*3)+1];
                    pLHand.z = _init_LeftHandPos.z;
                    animator.SetIKPosition(AvatarIKGoal.LeftHand,pLHand);

                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot,1);
                    pRFoot.x = keypoints_2d[11*3];
                    pRFoot.y = keypoints_2d[(11*3)+1];
                    pRFoot.z = _init_RigthFootPos.z;
                    animator.SetIKPosition(AvatarIKGoal.RightFoot,pRFoot);

                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1);
                    pLFoot.x = keypoints_2d[14*3];
                    pLFoot.y = keypoints_2d[(14*3)+1];
                    pLFoot.z = _init_LeftFootPos.z;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot,pLFoot);

                    //IK Hint
                    // animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow,1);
                    // pRElbow.x = keypoints_2d[3*3];
                    // pRElbow.y = keypoints_2d[(3*3)+1];
                    // animator.SetIKHintPosition(AvatarIKHint.RightElbow,pRElbow);

                    // animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow,1);
                    // pLElbow.x = keypoints_2d[6*3];
                    // pLElbow.y = keypoints_2d[(6*3)+1];
                    // animator.SetIKHintPosition(AvatarIKHint.LeftElbow,pLElbow);

                    // animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee,1);
                    // pRKnee.x = keypoints_2d[10*3];
                    // pRKnee.y = keypoints_2d[(10*3)+1];
                    // animator.SetIKHintPosition(AvatarIKHint.RightKnee,pRKnee);

                    // animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee,1);
                    // pLKnee.x = keypoints_2d[13*3];
                    // pLKnee.y = keypoints_2d[(13*3)+1];
                    // animator.SetIKHintPosition(AvatarIKHint.LeftKnee,pLKnee);

                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else 
            {          
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot,0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,0);
            }
        }
    }    
}

[System.Serializable]
public class JSONfile
{
    public float version;
    public List<coordinates> people = new List<coordinates>();
}

[System.Serializable]
public class coordinates
{
    public List<int> person_id;
    public List<float> pose_keypoints_2d;
    public List<float> face_keypoints_2d;
    public List<float> hand_left_keypoints_2d;
    public List<float> hand_right_keypoints_2d;
    public List<float> pose_keypoints_3d;
    public List<float> face_keypoints_3d;
    public List<float> hand_left_keypoints_3d;
    public List<float> hand_right_keypoints_3d;
}