using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnteringPort : MonoBehaviour
{


    [SerializeField]
    private Enum.IslandType _islandType = Enum.IslandType.Alpha;

    [SerializeField]
    private AudioSource _audioSource = null;
    [SerializeField]
    private AudioClip _audioClip = null;
    [SerializeField]
    private GameObject _text = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _text.SetActive(true);
            _audioSource.PlayOneShot(_audioClip);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                Entering();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _text.SetActive(false);
        }
    }

    private void Entering()
    {
        LoadingSceneController.LoadScene(_islandType);
    }
}
