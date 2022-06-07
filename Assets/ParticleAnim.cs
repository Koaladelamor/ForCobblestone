using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnim : MonoBehaviour
{
    public ParticleSystem fuego;

    public void activar()
    {
        fuego.Play();
    }

    public void desactivar()
    {
        fuego.Stop();
    }
}
