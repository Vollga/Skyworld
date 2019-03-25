using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    [Header("General")]
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;
    [Header("Super Jump Settings")]
    public float SuperJumpHeight = 4f;
    public bool _SuperJumpEnabled = true;
    public bool _SuperJumpOnTimer = true;
    public int SuperJumpTimer = 4;

    [Header ("Particles")]
    public ParticleSystem JumpParticles;
    public ParticleSystem BlowParticles;

    [Header("Collectables")]
    public bool _hasWindOrb = false;
    public bool _hasJPOrb = false;
    public bool _hasCloudOrb = false;


    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private bool _hasSuperJumped = false;
    private Transform _groundChecker;
    private Transform templeRespawn;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        templeRespawn = GameObject.Find("TempleRespawn").GetComponent<Transform>();
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        if (_isGrounded == true)
        {
            _hasSuperJumped = false;
            StopCoroutine("JumpTimer");
            //print("grounded");
        }

        //_body.useGravity = true;

        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        if (Input.GetButtonDown("Superjump") && _SuperJumpEnabled && !_hasSuperJumped)
        {
            Vector3 vel = _body.velocity;
            vel.y = Mathf.Sqrt(SuperJumpHeight * -2f * Physics.gravity.y);
            vel.x = 0;
            _body.velocity = vel;
            JumpParticles.Play(true);

            //_body.AddForce(Vector3.up * Mathf.Sqrt(SuperJumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            _hasSuperJumped = true;
            if (_SuperJumpOnTimer)
            {
                StartCoroutine("JumpTimer");
            }
        }

        if (Input.GetButton("Hover") && !_isGrounded)
        {
            if (_body.velocity.y < 0f)
            {
                Vector3 vel = _body.velocity;
                vel.y = -1f;
                _body.velocity = vel;
            }
            //_body.useGravity = false;
        }
    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSecondsRealtime(SuperJumpTimer);
        //print("superjump reset");
        _hasSuperJumped = false;
    }

    void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Enter");
        if (other.gameObject.name == "JPOrb") {
            Destroy(other.gameObject);
            _hasJPOrb = true;
            StartCoroutine("Respawn");
        }
        else if (other.gameObject.name == "WindOrb") {
            Destroy(other.gameObject);
            _hasWindOrb = true;
            StartCoroutine("Respawn");
        }
        else if (other.gameObject.name == "CloudOrb") {
            Destroy(other.gameObject);
            _hasCloudOrb = true;
            StartCoroutine("Respawn");
        }
        else if (other.gameObject.name == "Flow") {
            print("schinken");
            //Destroy(other.gameObject);
            //_hasCloudOrb = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "CloudTrigger")
        {
            print("Fluffy Cloud!!");
            if (Input.GetButtonDown("Wind"))
            {
                //print("DIE!");
                BlowParticles.Play(true);
                other.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(1);
        gameObject.transform.position = templeRespawn.transform.position;
    }
}