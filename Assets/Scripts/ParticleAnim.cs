using UnityEngine;

public class ParticleAnim : MonoBehaviour
{
    [SerializeField] private ParticleSystem fuego;

    public void Activar()
    {
        fuego.Stop();
        fuego.Play();
    }

    public void Desactivar()
    {
        fuego.Stop();
    }
}
