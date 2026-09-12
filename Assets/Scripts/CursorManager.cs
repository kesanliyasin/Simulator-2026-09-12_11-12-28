using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        // ESC ile fareyi geri getirme (test için kullanışlı)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
        // Sol tıkla tekrar kilitleme
        else if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}