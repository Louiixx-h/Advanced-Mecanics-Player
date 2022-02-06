using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Animator m_AnimatorController;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private Transform m_PlayerTransform;

    [Header("CHECK GROUND")]
    [SerializeField] private Transform m_GroundPosition;
    [SerializeField] private LayerMask m_LayerGround;
    [SerializeField] private bool mIsGrounded = false;

    
    [SerializeField] private CharacterController m_CharacterController;

    private float mSpeed = 10f;
    private float mDirectionSpeed = 10f;
    private float mGravity = 9.81f;
    private float mJumpHeight = 100f;
    private float radiusCheckSphere = 0.08f;

    private Vector3 mVertical = Vector3.zero;
    private Vector3 mMovement = Vector3.zero;
    public Vector2 Movement
    {
        get { return mMovement; }
        set { 
            mMovement.x = value.x; 
            mMovement.z = value.y;
            HandleMove(mMovement.x, mMovement.z);
        }
    }

    private int mIsRunHash;
    private int mIsFallHash;

    private void Awake()
    {
        mIsRunHash = Animator.StringToHash("run");
        mIsFallHash = Animator.StringToHash("fall");
    }

    void Update()
    {
        HandleCheckGround();
        HandleGravity();
        RunAnimation();
        FallAnimation();
    }

    void HandleCheckGround()
    {
        bool isGround = Physics.CheckSphere(
            m_GroundPosition.position,
            radiusCheckSphere,
            m_LayerGround
        );

        if (isGround)
        {
            mIsGrounded = true;
            mVertical.y = 0f;
        }
        else mIsGrounded = false;
    }

    void HandleMove(float horizontal, float forward)
    {
        mMovement.x = horizontal;
        mMovement.z = forward;
        Vector3 mov = new Vector3(horizontal, 0f, forward);
        
        if (mIsGrounded)
        {
            mMovement.y = 0f;
            if (mov.magnitude >= 0.1f)
            {
                HandleDirection(forward, horizontal);

                mMovement.y -= mGravity * Time.deltaTime;
                m_CharacterController.Move(mMovement.normalized * mSpeed * Time.deltaTime);
            }
        }
    }

    void HandleGravity()
    {
        mVertical.y -= mGravity * Time.deltaTime;
        m_CharacterController.Move(mVertical.normalized * mSpeed * Time.deltaTime);
    }

    void HandleDirection(float inputX, float inputY)
    {
        float angleCamera = m_MainCamera.transform.rotation.eulerAngles.y;
        float angle = Mathf.Atan2(inputX, inputY) * Mathf.Rad2Deg + angleCamera;
        Quaternion dir = Quaternion.Euler(0, angle, 0f);

        m_PlayerTransform.rotation = Quaternion.Slerp(
            m_PlayerTransform.rotation,
            dir,
            Time.deltaTime * mDirectionSpeed
        );
        mMovement = dir * Vector3.forward;
    }



    void RunAnimation() {
        if (mMovement.magnitude >= 0.1f && mIsGrounded)
        {
            m_AnimatorController.SetBool(mIsRunHash, true);
        }
        else
        {
            m_AnimatorController.SetBool(mIsRunHash, false);
        }
    }

    void FallAnimation()
    {
        if (!mIsGrounded)
        {
            m_AnimatorController.SetBool(mIsFallHash, true);
        }
        else
        {
            m_AnimatorController.SetBool(mIsFallHash, false);
        }
    }
}
