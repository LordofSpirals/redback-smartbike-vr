using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAudioManager : MonoBehaviour
{
    // Dictionary pairing surface Materials (the key) with the appropriate sound effect (the value)
    private Dictionary<string, AudioClip> surfaceSounds = new Dictionary<string, AudioClip>();
    private PlayerMovementController playerController;
    private AudioSource contactPoint;
    public AudioClip grassAudio;
    public AudioClip roadAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        surfaceSounds.Add("Ground (Instance)", grassAudio);
        surfaceSounds.Add("Side Walk (Instance)", roadAudio);
        surfaceSounds.Add("Road (Instance)", roadAudio);
        playerController = GetComponentInParent<PlayerMovementController>();
        contactPoint = transform.Find("ContactPoint").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Only execute further if bike is moving
        if (!playerController.IsMoving())
        {
            contactPoint.Stop(); //Stop playing sound if bike isn't moving
            return;
        }

        //Use raycast to get current surface bike is on
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit);
        contactPoint.enabled = true;
        string ground = hit.collider.gameObject.GetComponent<Renderer>().material.name;
        contactPoint.clip = getAudioForSurface(ground);
        contactPoint.Play();
        //Debug.Log(hit.collider.gameObject.GetComponent<Renderer>().material);
        //Debug.Log(playerController.IsMoving());
    }

    //Check dictionary for surface and return appropriate audio clip
    private AudioClip getAudioForSurface(string surfaceName)
    {
        try
        {
            AudioClip clip = surfaceSounds[surfaceName];
            //Debug.Log(clip);
            return clip;
        }
        catch
        {
            Debug.Log("ERROR: No audio clip found for " + surfaceName + " material!");
            return null;
        }
    }
}
