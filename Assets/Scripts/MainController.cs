using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;
using System;

public class MainController : MonoBehaviour
{


    // statics
    public static bool ifPaused = false;


    // Timers
    public Timer mainTime;
    public Timer rocketTime;
    public Timer sniperTime;
    public Timer powerUpTime;

    // Pie Timers
    public Animator rocketAnim;
    public Animator sniperPieTimer;
    public Animator matchTimePieTimer;
    public Animator powerUpPieTimer;
    public GameObject powerUpAnimator;


    // Start/Stop Buttons
    public Button StartStop;
    public Button PauseResume;
    public Button PowerUp_PickedUp;

    // Cancel custom button
    public Button CancelCustom;


    // Button Texts
    public TextMeshProUGUI StartStopText;
    public TextMeshProUGUI PauseResumeText;
    
    // Toggles
    public GameObject TimeSettingsDropdown;
    public static bool TournamentMode = true;
    public static bool MatchmakingModeFifteen = false;
    public static bool MatchmakingModeTwelve = false;
    public static bool MatchmakingModeFive = false;
    public static bool TestMode10Seconds = false;

    // Bools
    private bool Start_Toggle = true;
    private bool Pause_Toggle = true;
    private bool PowerUpStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // Never go dim

        initMainTime();
        PowerUp_PickedUp.interactable = false;

        TimeSettingsDropdown.SetActive(true);
    }

    public void initMainTime()
    {
        if (MatchmakingModeFifteen)
        {
            mainTime.total = 899;
            mainTime.minutes = 14;
            mainTime.seconds = 59;
        }
        else if (TournamentMode)
        {
            mainTime.total = 1799;
            mainTime.minutes = 29;
            mainTime.seconds = 59;
        }
        else if (MatchmakingModeTwelve)
        {
            mainTime.total = 719;
            mainTime.minutes = 11;
            mainTime.seconds = 59;
        }
        else if (MatchmakingModeFive)
        {
            mainTime.total = 299;
            mainTime.minutes = 4;
            mainTime.seconds = 59;
        }
        else if (TestMode10Seconds)
        {
            mainTime.total = 10;
            mainTime.minutes = 0;
            mainTime.seconds = 10;
        }
        else
        {
            mainTime.total = 0;
            mainTime.minutes = 0;
            mainTime.seconds = 0;
        }
    }

    // ----  Start/Stop Button Is Pressed -----
    public void StartTimer()
    {
        if (Start_Toggle)
        {
            ifPaused = false;
            // Reset Main Time
            initMainTime();

            // Start Countdown
            mainTime.StartTimer();
            rocketTime.StartTimer();
            sniperTime.StartTimer();

            PowerUp_PickedUp.interactable = true;
            CancelCustom.interactable = (false);


            TimeSettingsDropdown.SetActive(false);
            PauseResume.interactable = true;


            rocketAnim.enabled = (true);
            sniperPieTimer.enabled = (true);
            matchTimePieTimer.enabled = (true);
            powerUpAnimator.GetComponent<ProgressBar>().currentPercent = 0;

            rocketAnim.SetTrigger("Play");
            sniperPieTimer.SetTrigger("Play");

            Start_Toggle = !Start_Toggle;
            StartStopText.text = "Cancel";

        }
        else
        {
            ifPaused = true;
            // Stop Countdown, make start button interatable
            mainTime.StopTimer();
            rocketTime.StopTimer();
            sniperTime.StopTimer();
            powerUpTime.StopTimer();

            mainTime.ResetTimer();
            rocketTime.ResetTimer();
            sniperTime.ResetTimer();
            powerUpTime.ResetTimer();

         
            PowerUp_PickedUp.interactable = false;
            CancelCustom.interactable = (false);


            TimeSettingsDropdown.SetActive(true);
            PauseResume.interactable = false;
            Start_Toggle = !Start_Toggle;
            Pause_Toggle = true;


            rocketAnim.SetTrigger("Stop");
            sniperPieTimer.SetTrigger("Stop");
            powerUpPieTimer.SetTrigger("Stop");
            powerUpAnimator.GetComponent<ProgressBar>().currentPercent = 0;

            matchTimePieTimer.gameObject.SetActive(false);
            matchTimePieTimer.gameObject.SetActive(true);


            rocketAnim.enabled = (false);
            sniperPieTimer.enabled = (false);
            matchTimePieTimer.enabled = (false);
            powerUpPieTimer.enabled = (false);
            powerUpAnimator.GetComponent<ProgressBar>().isOn = false;
            PowerUpStarted = false;

            initMainTime();

            PauseResumeText.text = "Pause";
            StartStopText.text = "Start";

        }

    }

    public void PauseTimer()
    {
        if (Pause_Toggle)
        {
            ifPaused = true;
            mainTime.PauseTimer();
            sniperTime.PauseTimer();
            rocketTime.PauseTimer();
            powerUpTime.PauseTimer();
            PauseResumeText.text = "Resume";

            rocketAnim.enabled = (false);
            sniperPieTimer.enabled = (false);
            matchTimePieTimer.enabled = (false);
            powerUpPieTimer.enabled = (false);
            powerUpAnimator.GetComponent<ProgressBar>().isOn = false;
            PowerUp_PickedUp.interactable = false; // do not allow custom to be started when paused
            CancelCustom.interactable = (false);

        }
        else
        {
            ifPaused = false;
            mainTime.ResumeTimer();
            sniperTime.ResumeTimer();
            rocketTime.ResumeTimer();
            if (PowerUpStarted == true)
            { // only play if it was previously started
                powerUpTime.ResumeTimer();
                powerUpPieTimer.enabled = (true);
                powerUpPieTimer.SetTrigger("Play");
                powerUpAnimator.GetComponent<ProgressBar>().isOn = true;
                CancelCustom.interactable = (true);

            }

            sniperPieTimer.enabled = (true);
            sniperPieTimer.SetTrigger("Play");

            rocketAnim.enabled = (true);
            rocketAnim.SetTrigger("Play");

            matchTimePieTimer.enabled = (true);
            matchTimePieTimer.enabled = (true);

            PauseResumeText.text = "Pause";
        }
        Pause_Toggle = !Pause_Toggle;
    }

    float timer;


    // --- Power up button clicked ---
    public void startPowerUpTimer()
    {
        PowerUpStarted = true;
        PowerUp_PickedUp.interactable = false;
        CancelCustom.interactable = (true);
        powerUpTime.StartTimer();
        powerUpPieTimer.enabled = (true);
        powerUpAnimator.GetComponent<ProgressBar>().isOn = true;
    }

    // -- Power up cancel button clicked ---
    public void cancelPowerUpTimer()
    {
        PowerUpStarted = false;
        PowerUp_PickedUp.interactable = true;
        CancelCustom.interactable = (false);
        powerUpTime.StopTimer();
        powerUpTime.EmptyReset();
        powerUpPieTimer.enabled = (false);
        powerUpAnimator.GetComponent<ProgressBar>().isOn = false;
        powerUpAnimator.GetComponent<ProgressBar>().currentPercent = 0;

    }

    public void SetTournamentModeTime()
    {
        TournamentMode = true;
        MatchmakingModeFifteen = false;
        MatchmakingModeTwelve = false;
        MatchmakingModeFive = false;
        TestMode10Seconds = false;


        initMainTime();

    }

    public void SetMatchMakingTimeFifteen()
    {
        MatchmakingModeFifteen = true;
        TournamentMode = false;
        MatchmakingModeTwelve = false;
        MatchmakingModeFive = false;
        TestMode10Seconds = false;



        initMainTime();

    }

    public void SetMatchMakingTimeTwelve()
    {
        MatchmakingModeTwelve = true;
        MatchmakingModeFifteen = false;
        TournamentMode = false;
        MatchmakingModeFive = false;
        TestMode10Seconds = false;


        initMainTime();

    }

    public void SetMatchMakingTimeFive()
    {
        MatchmakingModeFive = true;
        MatchmakingModeTwelve = false;
        MatchmakingModeFifteen = false;
        TournamentMode = false;
        TestMode10Seconds = false;

        initMainTime();

    }

    public void SetMatchMakingTime10Seconds()
    {
        TestMode10Seconds = true;
        MatchmakingModeFive = false;
        MatchmakingModeTwelve = false;
        MatchmakingModeFifteen = false;
        TournamentMode = false;

        initMainTime();

    }

    public bool getTournamentMode() {

        return TournamentMode;
    }


}