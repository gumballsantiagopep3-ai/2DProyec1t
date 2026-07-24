using System.Runtime.CompilerServices;
using UnityEngine;

public class PausarMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject menuPausa;
    public bool juegoPausa = false;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (juegoPausa)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }

        }
    }

    public void Reanudar() 
    { 
        menuPausa.SetActive(false);
        Time.timeScale = 1.0f;
        juegoPausa = false;
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausa = true;
    }

}