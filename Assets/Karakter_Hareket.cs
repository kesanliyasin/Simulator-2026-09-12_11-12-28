using UnityEngine;
using UnityEngine.InputSystem; // Yeni Girdi Sistemi kütüphanesi

public class Karakter_Hareket : MonoBehaviour
{
    public float hiz = 5f;

    void Update()
    {
        float yatay = 0f;
        float dikey = 0f;

        // Klavyeden donanımsal olarak doğrudan girdi okuma (Yeni Sistem)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) dikey = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) dikey = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) yatay = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) yatay = 1f;
        }

        Vector3 hareket = new Vector3(yatay, 0f, dikey);
        transform.Translate(hareket * hiz * Time.deltaTime);
    }
}