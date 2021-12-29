using UnityEngine;
using UnityEngine.UI;

public class NotificationChanger : MonoBehaviour
{
    public Sprite on; 
    public Sprite off;
    public GameObject TimeObject;
    public Timer Timer;
    private Image image;
    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void onClickBell()
    {
        if (toggle)
        {
            image.sprite = on;
            TimeObject.SetActive(true);
            Timer.Notifications = true;

        }
        else
        {
            image.sprite = off;
            TimeObject.SetActive(false);
            Timer.Notifications = false;
        }

        toggle = !toggle;
    }
}
