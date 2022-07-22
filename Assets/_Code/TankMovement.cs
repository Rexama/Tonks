using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class TankMovement : MonoBehaviour
{
    private Vector2 aimPos = Vector2.zero;
    private bool isFireButtonPresed = false;

    public GameObject gun;
    public GameObject ammo;
    public Transform spawnPoint;
    private Rigidbody2D rb;
    private UnityEngine.Camera _cam;
    private string _type;
    public float ammoSpeed = 30f;
    public float moveForce = 10f;

    public UnityEvent OnShoot;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        _cam = UnityEngine.Camera.main;
        _type = transform.GetComponent<PlayerInput>().currentControlScheme;

        if (_type == "Mouse")
        {
            gameObject.tag = "Mouse";
            GameLogic.Ins.Mouse = gameObject;
        }
        else
        {
            gameObject.tag = "Controller";
            GameLogic.Ins.Controller = gameObject;
        }
    }


    public void OnFire(InputAction.CallbackContext context)
    {
        isFireButtonPresed = context.action.triggered;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (_type == "Mouse")
        {
            aimPos = _cam.ScreenToWorldPoint((Vector3) context.ReadValue<Vector2>())- transform.position;
        }
        else
        {
            aimPos = context.ReadValue<Vector2>();
        }
    }

    void Update()
    {
        if (aimPos.magnitude != 0)
            gun.transform.right = (Vector3) aimPos;
        if (isFireButtonPresed)
        {
            OnShoot?.Invoke();

            var newAmmo = Instantiate(ammo, gun.transform.position, quaternion.identity);
            newAmmo.transform.right = gun.transform.right;
            rb.AddForce(-gun.transform.right * moveForce);
            newAmmo.GetComponent<Rigidbody2D>().velocity = gun.transform.right.normalized * ammoSpeed;
            isFireButtonPresed = false;
        }
    }
}