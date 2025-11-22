using UnityEngine;
using UnityEngine.XR.Management;

public class RigLoader : MonoBehaviour
{
    [Header("Rigs de la Escena")]
    [SerializeField] private GameObject pcRig;
    [SerializeField] private GameObject vrRig;

    private void Awake()
    {
        // Por defecto desactivamos ambos para evitar conflictos al cargar
        pcRig.SetActive(false);
        vrRig.SetActive(false);

        StartCoroutine(CheckXR());
    }

    private System.Collections.IEnumerator CheckXR()
    {
        // Esperamos un frame para asegurar que el subsistema XR reporta estado
        yield return null; 

        var xrSettings = XRGeneralSettings.Instance;
        if (xrSettings != null && xrSettings.Manager != null && xrSettings.Manager.isInitializationComplete)
        {
            // Si hay un headset inicializado -> Activar VR
            Debug.Log("Modo VR Detectado");
            vrRig.SetActive(true);
        }
        else
        {
            // Si no hay headset o falló la carga -> Fallback a PC
            Debug.Log("Modo Monitor (PC) Detectado");
            pcRig.SetActive(true);
        }
    }
}