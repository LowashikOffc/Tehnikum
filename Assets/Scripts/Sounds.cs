using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public List<AudioSource> sounds;

    public void playsoundatpoint_(int num, Vector3 pos, float vol)
    {
        AudioSource.PlayClipAtPoint(sounds[num].clip, pos, vol);
    }
    public void playsound_(int num)
    {
        sounds[num].Play();
    }
}
