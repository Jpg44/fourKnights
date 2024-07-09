using UnityEngine;
using UnityEngine.UI;

public class DieModal : MonoBehaviour
{
    public Image dieModal;

    public void ShowModal()
    {
        Debug.Log("dddddddddddddxxvxcc");
        dieModal.gameObject.SetActive(true);
    }

    public void HideModal()
    {
        dieModal.gameObject.SetActive(false);
    }
}
