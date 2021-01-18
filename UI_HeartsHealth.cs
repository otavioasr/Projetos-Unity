using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HeartsHealth : MonoBehaviour
{
    [SerializeField] private Sprite heart0Sprite;
    [SerializeField] private Sprite heart1Sprite;
    [SerializeField] private Sprite heart2Sprite;
    [SerializeField] private Sprite heart3Sprite;
    [SerializeField] private Sprite heart4Sprite;

    [SerializeField] private AnimationClip heartEmptyAnimationclip;

    private List<HeartImage> heartImageList;
    private HeartsHealthSystem heartsHealthSystem;
    private bool isHealing;

    private void Awake ()
    {
        heartImageList = new List<HeartImage>();
    }

    private void Start()
    {        
        InvokeRepeating("HealingAnimatedPeriodic", .5f, .1f);
    }

    public void SetHeartsHealthSystem ( HeartsHealthSystem heartsHealthSystem)
    {
        this.heartsHealthSystem = heartsHealthSystem;
        List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
        Vector2 heartAnchoredPosition = new Vector2 (0, 0);
        for (int i = 0; i < heartList.Count; i++)
        {
            HeartsHealthSystem.Heart heart = heartList[i];
            CreateHeartImage (heartAnchoredPosition).SetHeartFragment (heart.GetFragmentAmount());
            heartAnchoredPosition += new Vector2 (20f, 0);
        }

        heartsHealthSystem.OnDamage += HeartsHealthSystem_OnDamage;
        heartsHealthSystem.OnHealed += HeartsHealthSystem_OnHealed;
        heartsHealthSystem.OnDead += HeartsHealthSystem_OnDead;
    }

    private void HeartsHealthSystem_OnDamage (object sender, System.EventArgs e)
    {
        RefreshAllHearts ();
    }

    private void HeartsHealthSystem_OnDead (object sender, System.EventArgs e)
    {
        Debug.Log ("dead");
    }

    private void HeartsHealthSystem_OnHealed (object sender, System.EventArgs e)
    {
        isHealing = true;
    }

    private void RefreshAllHearts ()
    {
        List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
        for (int i = 0; i < heartImageList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i];
            HeartsHealthSystem.Heart heart = heartList[i];
            heartImage.SetHeartFragment(heart.GetFragmentAmount());

            heartImage.PlayHeartEmptyAnimation();                       
        }
    }

    private void HealingAnimatedPeriodic ()
    {
        if (isHealing)
        {
            bool fullyHealed = true;
            List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
            for (int i = 0; i < heartList.Count; i++)
            {
                HeartImage heartImage = heartImageList[i];
                HeartsHealthSystem.Heart heart = heartList[i];
                if (heartImage.GetFragmentAmount () != heart.GetFragmentAmount ())
                {
                    heartImage.AddHeartVisualFragment ();
                    fullyHealed = false;    
                    break;
                }
            }
            if (fullyHealed)
                isHealing = false;
        }
    }

    private HeartImage CreateHeartImage (Vector2 anchoredPosition)
    {
        GameObject heartGameObject = new GameObject ("Heart", typeof(Image), typeof(Animation));
        heartGameObject.transform.parent = transform;
        heartGameObject.transform.localPosition = Vector3.zero;

        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(24f, 24f);
        heartGameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        heartGameObject.GetComponent<Animation>().AddClip(heartEmptyAnimationclip, "HeartEmpty");

        Image heartImageUI = heartGameObject.GetComponent<Image>();
        heartImageUI.sprite = heart0Sprite;
        
        //heartImageUI.scale  = new Vector2(24f, 24f);
        //heartImageUI.SetNativeSize();
        
        HeartImage heartImage = new HeartImage (this, heartImageUI, heartGameObject.GetComponent<Animation>()); 
        heartImageList.Add (heartImage);
        return heartImage;
    }

    public class HeartImage
    {
        private int fragments;
        private Image heartImage;
        private UI_HeartsHealth ui_HeartsHealth;
        private Animation animation;

        public HeartImage (UI_HeartsHealth ui_HeartsHealth, Image heartImage, Animation animation)
        {
            this.ui_HeartsHealth = ui_HeartsHealth;
            this.heartImage = heartImage;
            this.animation = animation;
        }

        public void SetHeartFragment (int fragments)
        {
            this.fragments = fragments;
            switch (fragments)
            {
                case 0: heartImage.sprite = ui_HeartsHealth.heart0Sprite; break;
                case 1: heartImage.sprite = ui_HeartsHealth.heart1Sprite; break;
                case 2: heartImage.sprite = ui_HeartsHealth.heart2Sprite; break;
                case 3: heartImage.sprite = ui_HeartsHealth.heart3Sprite; break;
                case 4: heartImage.sprite = ui_HeartsHealth.heart4Sprite; break;
            }

        }

        public int GetFragmentAmount ()
        {
            return fragments;
        }

        public void AddHeartVisualFragment ()
        {
            SetHeartFragment (fragments + 1);
        }

        public void PlayHeartEmptyAnimation ()
        {
            animation.Play("HeartEmpty", PlayMode.StopAll);
        }
    }
}
