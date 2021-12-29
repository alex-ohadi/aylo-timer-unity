using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using System;

public class Timer : MonoBehaviour
{

    private bool MasterOn;
    bool ApplicationIsPaused;

    public Timer mainTimer;

    [Header("Enter Time To Count Down From")]
    public int total = 0;
    public int minutes = 0;
    public int seconds = 0;

    // AbsTime
    [HideInInspector]
    public int absTotal = 0;
    [HideInInspector]
    public int absMinutes = 0;
    [HideInInspector]
    public int absSeconds = 0;

    public bool Notifications;
    public AudioSource Sound;
    public bool TenSecondWarning;
    public AudioSource TenSecondWarningSound;
    public bool Vibrate;
    public bool Flash;
    public GameObject FlashScreenObject;
    private float flashTimer;

    [Header("Children")]
    public Text Text;
    public Text DisplayAbsTime;
    public Text Title;
    public GameObject Clock;
    public Image Fill;

    [Header("Restart")]
    public bool Restart_Automatically;
    public Button RestartButton;
    public Animator setInactiveAnimation;
    public GameObject radialReglarAnimation;


    // privates
    private int totalStartValue;
    private int minutesStartValue;
    private int secondsStartValue;
    [HideInInspector]
    public bool start;
    private int OffsetDelay30MinTimerCount = 0;

    [Header("Toggles Changed in Settings")]
    // Toggles In Settigngs
    public Toggle enableToggle;
    private bool toggleNotificationsFlag = true;
    private bool tenSecondWarningToggleFlag = true;
    private bool toggleVibrate = true;
    private bool toggleFlash = true;
    private bool MainEnded = false;

    public Timer(int total, int minutes, int seconds)
    {
        this.total = total;
        this.minutes = minutes;
        this.seconds = seconds;

        total = totalStartValue;
        minutesStartValue = minutes;
        secondsStartValue = seconds;
    }

    // Inital set up
    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        MasterOn = true;
        ApplicationIsPaused = false;

        totalStartValue = total;
        minutesStartValue = minutes;
        secondsStartValue = seconds;

    }


    /* Function to control Timer Start and Stop */
    public void StartTimer()
    {
        start = true;
        MainEnded = false;
        CalculateAbsTime();
    }

    public void StopTimer()
    {
        start = false;
        Clock.GetComponent<Animator>().enabled = (false);
        Fill.fillAmount = 0;
        OffsetDelay30MinTimerCount = 0;
    }

    /* Function to control Timer Resume and Pause */
    // Fixing bug where absoulte times were being called on Resume, and not Start only
    public void ResumeTimer()
    {
        start = true;
    }

    public void PauseTimer()
    {
        start = false; 
    }

    private float _timer = 0f;
    private void FixedUpdate()
    {

        /* calculuate timestep */
        if (start)
        {
            _timer += Time.fixedDeltaTime;

            if (_timer >= 0.995f)
            {

                _timer = 0f;

                // Tick one second
                total -= 1;

                // Seconds
                seconds -= 1;
                if (seconds < 0)
                {
                    seconds = 59;
                    minutes -= 1;
                }
            }
        }

        if (MainEnded == false)
        {
            /* check if 0 */
            if (total <= 0) 
            {
                if (gameObject.tag == "MainTimer")
                {
                    MainEnded = true;
                    StopTimer();
                    ResetTimer();
                }

                if (MainEnded == false)
                {
                    // Play Sounds and stuff
                    if (Sound != null)
                    {
                        if (!Sound.isPlaying && Notifications == true && MasterOn)
                        {

                            Sound.Play();
                        }
                    }

                    if (Vibrate && MasterOn)
                    {
                        StartCoroutine(StartVibrateEnumerator());
                    }
                    if (Flash && MasterOn)
                    {
                        StartCoroutine(StartFlashEnumerator());
                    }

                    // if Reset flag not set, stop it
                    if (gameObject.tag == "PowerUpTimer")
                    {
                        StopTimer(); // stopping timer will make absolute timer to set to 0
                        if (RestartButton != null)
                            RestartButton.interactable = true;
                        if (setInactiveAnimation != null)
                            setInactiveAnimation.enabled = (false);
                        if (radialReglarAnimation != null)
                        {
                            radialReglarAnimation.GetComponent<ProgressBar>().currentPercent = 0;
                            radialReglarAnimation.GetComponent<ProgressBar>().isOn = false;

                        }
                    }

                    ResetTimer();

                }
            }

            // Display
            if (seconds > 10)
            {
                Text.text = minutes + ":" + seconds;
            }
            else if (seconds == 10 && minutes == 0)
            {
                Text.text = minutes + ":" + seconds;
                if (Notifications && TenSecondWarning && MasterOn)
                {
                    if (!TenSecondWarningSound.isPlaying)
                    {
                        TenSecondWarningSound.Play();
                    }
                }
            }
            else if (seconds == 10)
            {
                Text.text = minutes + ":" + seconds;
            }
            else
            {
                Text.text = minutes + ":0" + seconds;
            }
        }

    }

    public void ResetTimer()
    {
        if (gameObject.tag == "RocketTimer")
        {
            total = totalStartValue;
            minutes = minutesStartValue;
            seconds = secondsStartValue;
        }
        else if (gameObject.tag == "SniperTimer")
        {

            OffsetDelay30MinTimerCount++;
            if (OffsetDelay30MinTimerCount == 4) // when delay reaches third (count = 4) time, add 1 second
            {
                total = totalStartValue + 1; // add 1
                minutes = minutesStartValue;
                seconds = secondsStartValue + 1;
                OffsetDelay30MinTimerCount = 0;
            }
            else // other wise reset
            {
                total = totalStartValue;
                minutes = minutesStartValue;
                seconds = secondsStartValue;
            }
        }
        else
        {

            total = totalStartValue;
            minutes = minutesStartValue;
            seconds = secondsStartValue;
        }

        CalculateAbsTime();

    }

    void CalculateAbsTime()
    {

        if (mainTimer != null)
        {

            if (mainTimer.total >= total && start != false)
            {

                absTotal = mainTimer.total - total;

                absMinutes = absTotal / 60;
                absSeconds = Mathf.FloorToInt(absTotal % 60);

            }
            else
            {
                absTotal = 0;
                absMinutes = 0;
                absSeconds = 0;

                // Stop Timer
                StopTimer();

            }


            if (absSeconds > 10)
            {
                DisplayAbsTime.text = absMinutes + ":" + (absSeconds);
            }

            else if (absSeconds == 10)
            {
                DisplayAbsTime.text = absMinutes + ":" + (absSeconds);
            }
            else
            {
                DisplayAbsTime.text = absMinutes + ":0" + (absSeconds);
            }
        }
    }


    // Use Almost Never --  Only if you know what your doing (Cancel Custom Timer)
    public void EmptyReset()
    {
        total = totalStartValue;
        minutes = minutesStartValue;
        seconds = secondsStartValue;
        ResetAbsTime();
    }


    public void toggleNotifications()
    {
        if (toggleNotificationsFlag)
        {
            Notifications = true;
        }
        else
        {
            Notifications = false;
        }
        toggleNotificationsFlag = !toggleNotificationsFlag;

    }

    public void Enable()
    {
        if (enableToggle.isOn)
        {
            MasterOn = true;
            Title.gameObject.SetActive(true);
            Text.gameObject.SetActive(true);
            DisplayAbsTime.gameObject.SetActive(true);
            if (Clock != null)
            {
                Clock.SetActive(true);
            }

        }
        else
        {
            MasterOn = false;
            Title.gameObject.SetActive(false);
            Text.gameObject.SetActive(false);
            DisplayAbsTime.gameObject.SetActive(false);
            if (Clock != null)
            {
                Clock.SetActive(false);
            }
           
 
        }

    }



    public void toggleTenSecondWarning()
    {
        if (tenSecondWarningToggleFlag)
        {
            TenSecondWarning = true;
        }
        else
        {
            TenSecondWarning = false;
        }

        tenSecondWarningToggleFlag = !tenSecondWarningToggleFlag;
    }

    public void toggleVibrateWarning()
    {
        if (toggleVibrate)
        {
            Vibrate = true;
        }
        else
        {
            Vibrate = false;
        }

        toggleVibrate = !toggleVibrate;
    }

    public void toggleFlashWarning()
    {
        if (toggleFlash)
        {
            Flash = true;
        }
        else
        {
            Flash = false;
        }

        toggleFlash = !toggleFlash;
    }


    private IEnumerator StartFlashEnumerator()
    {
        // Flash twice
        FlashScreenObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        FlashScreenObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.2f);
        FlashScreenObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        FlashScreenObject.SetActive(false);


    }
    private IEnumerator StartVibrateEnumerator()
    {
        // Vibrate every 0.2 seconds;
        Handheld.Vibrate();
        yield return new WaitForSecondsRealtime(0.4f);
        Handheld.Vibrate();
        yield return new WaitForSecondsRealtime(0.4f);
        Handheld.Vibrate();

    }



       void ResetAbsTime()
        {
            if (mainTimer != null)
            {
               absTotal = 0;
               absMinutes = 0;
               absSeconds = 0;

            if (absSeconds > 10)
            {
                DisplayAbsTime.text = absMinutes + ":" + (absSeconds);
            }

            else if (absSeconds == 10)
            {
                DisplayAbsTime.text = absMinutes + ":" + (absSeconds);

            }
            else
            {
                DisplayAbsTime.text = absMinutes + ":0" + (absSeconds);
            }
            }

        }

}
