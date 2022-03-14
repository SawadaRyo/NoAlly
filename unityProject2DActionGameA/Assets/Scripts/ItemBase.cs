using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(Collider))]
public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] AudioClip m_getSound = default;
    [SerializeField] AudioSource m_audio;
    public abstract void Activate(Collider other);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //AudioSource.PlayClipAtPoint(m_getSound, Camera.main.transform.position);
            m_audio.PlayOneShot(m_getSound);
            Activate(other);
            Destroy(this.gameObject);
        }
    }
}