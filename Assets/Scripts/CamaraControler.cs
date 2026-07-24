using UnityEngine;

public class CamaraControler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform objetivo;
    public float velocidadcamara = 0.025f;
    public Vector3 desplazamiento;

    private void LateUpdate()
    {
        Vector3 posicionDesada = objetivo.position + desplazamiento;

        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDesada, velocidadcamara);

        transform.position = posicionDesada;
    }


}
