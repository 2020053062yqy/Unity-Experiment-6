using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
     public float MoveForce = 100.0f;   //角色移动力度
    public float MaxSpeed = 5;   //初始水平速度为5
    public Rigidbody2D HeroBody;
    [HideInInspector]
    public bool bFaceRight = true;  
    [HideInInspector]
    public bool bJump = false;  //角色初始状态是禁止的
    public float JumpForce = 100; //角色跳跃力度
    public AudioClip[] JumpClips;
    public AudioSource audioSource;

    private Transform mGroundCheck;  //查看是否为地面

     public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        HeroBody = GetComponent<Rigidbody2D>();  //引用组件Rigidbody2D
        mGroundCheck = transform.Find("GroundCheck");  //是否找到地面
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         float h = Input.GetAxis("Horizontal");  //获取水平轴
        if(Mathf.Abs(HeroBody.velocity.x)<MaxSpeed)
        {
            HeroBody.AddForce(Vector2.right * h * MoveForce);
        }

        if(Mathf.Abs(HeroBody.velocity.x)>5)
        {
            HeroBody.velocity = new Vector2(Mathf.Sign(HeroBody.velocity.x) * MaxSpeed,
                                            HeroBody.velocity.y);
        }

        anim.SetFloat("speed", Mathf.Abs(h));
      


        if(h>0 && !bFaceRight)
        {
            flip();
        }
        else if(h<0 && bFaceRight)
        {
            flip();
        }
        //射线检测是通过按位与的操作进行而不是通过“==”操作进行判断
        if (Physics2D.Linecast(transform.position, mGroundCheck.position,
                                1<<LayerMask.NameToLayer("Ground")))
        {
            if(Input.GetButtonDown("Jump"))
            {
                bJump = true;
            }
        }

         Debug.DrawLine(transform.position, mGroundCheck.position, Color.red);

    }

      private void FixedUpdate()
    {
        if(bJump)
        {
            int i= Random.Range(0,JumpClips.Length);
            
            HeroBody.AddForce(Vector2.up * JumpForce);
            bJump = false;
            anim.SetTrigger("jump");
        }
    }

    private void flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        bFaceRight = !bFaceRight;
    }
}