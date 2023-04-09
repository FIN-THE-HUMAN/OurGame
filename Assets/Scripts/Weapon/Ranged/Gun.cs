using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [SerializeField] private KeyCode Fire = KeyCode.Mouse0;
    [SerializeField] private Rigidbody Bullet;
    [SerializeField] private float Force;
    [SerializeField] private Transform FirePosition;

    private float cooldown = 0.2f;
    private float lastFireTime;

    public UnityEvent<Gun, Rigidbody> Shoot;

    void Update()
    {
        if (Input.GetKey(Fire))
        {
            if (Time.time > lastFireTime + cooldown)
            {
                lastFireTime = Time.time;
                Shot();
            }
        }
    }

    private void Shot()
    {
        var c = Instantiate(Bullet, FirePosition.position, Quaternion.identity);
        c.gameObject.SetActive(true);
        c.AddForce(transform.forward * Force);
        Shoot.Invoke(this, c);
    }
}
