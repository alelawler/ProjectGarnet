using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] AudioClip grassStep;
    [SerializeField] AudioClip stoneStep;
    [SerializeField] AudioClip metalStep;
    [SerializeField] AudioClip grunt1;
    [SerializeField] AudioClip grunt2;
    [SerializeField] AudioClip grunt3;
    [SerializeField] AudioClip slide;
    [SerializeField] AudioClip empty;
    [SerializeField] AudioClip deathGrunt;
    [SerializeField] AudioClip deathGrunt2;
    [SerializeField] AudioClip pickup;

    [Header("Audio Source")]
    [SerializeField] AudioSource audioSource;

    public enum Actions
    {
        Run,
        Sprint,
        Stop,
        Jump,
        Slide,
        Grunt,
        Empty,
        Die,
        Pickup,
    }

    public void PlaySound(Actions actions, string tag)
    {
        int chance = Random.Range(0, 100);

        switch (actions)
        {
            case Actions.Run:
                 switch (tag)
                {
                    case "Grass":
                        audioSource.clip = grassStep;
                        audioSource.loop = true;
                        audioSource.Play();
                        break;
                    case "Stone":
                        audioSource.clip = stoneStep;
                        audioSource.loop = true;
                        audioSource.Play();
                        break;
                    case "Metal":
                        audioSource.clip = metalStep;
                        audioSource.loop = true;
                        audioSource.Play();
                        break;
                }
                break;
            case Actions.Sprint:
                break;
            case Actions.Stop:
                audioSource.loop = false;
                audioSource.Stop();
                break;
            case Actions.Jump:
                audioSource.loop = false;
                audioSource.Stop(); 
                break;
            case Actions.Slide:
                audioSource.PlayOneShot(slide);
                break;
            case Actions.Grunt:
                if (chance < 100 && chance > 66)
                {
                    audioSource.PlayOneShot(grunt1);
                }
                else if (chance <= 66 && chance > 33)
                {
                    audioSource.PlayOneShot(grunt2);
                }
                else if (chance <= 33)
                {
                    audioSource.PlayOneShot(grunt3);
                }
                break;
            case Actions.Empty:
                audioSource.PlayOneShot(empty, .3f);
                break;
            case Actions.Die:
                if (chance > 49)
                {
                    audioSource.PlayOneShot(deathGrunt);
                }
                else
                {
                    audioSource.PlayOneShot(deathGrunt2);
                }
                break;
            case Actions.Pickup:
                audioSource.PlayOneShot(pickup);
                break;
            default:
                break;
        }
    }
}
