using UnityEngine;

public class Reef : MonoBehaviour
{
    [SerializeField]
    private float _damage = 50f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _debuffSpeed = 0.5f;
    [SerializeField]
    private Boat _boat = null;

    [SerializeField]
    private AudioSource _audioSource = null;
    [SerializeField]
    private AudioClip _audioClip = null;

    public float Damage { get => _damage; set => _damage = value; }
    public float DebuffSpeed { get => _debuffSpeed; set => _debuffSpeed = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !_boat.IsWreck)
        {
            _boat.caughtReefEnter(_damage, _debuffSpeed);
            _audioSource.PlayOneShot(_audioClip);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _boat.caughtReefStay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _boat.caughtReefExit();
        }
    }

    private void Start()
    {
        _boat = GameObject.Find("Player").GetComponent<Boat>();
    }
}
