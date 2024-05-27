using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Com.Google.Android.Gms.Ads;
using Com.Google.Android.Gms.Ads.Nativead;
using QuickDate.Helpers.Utils;
using System;

namespace QuickDate.Helpers.Ads
{
    public class TemplateView : FrameLayout
    {
        private int TemplateType;
        private NativeTemplateStyle Styles;
        private NativeAd NativeAd;
        private NativeAdView NativeAdView;

        private TextView PrimaryView;
        private TextView SecondaryView;

        //private RatingBar RatingBar;
        private TextView TertiaryView;
        private ImageView IconView;

        private MediaView MediaView;

        //private AppCompatButton CallToActionView;

        private new LinearLayout Background;

        public static readonly string MediumTemplate = "medium_template";
        public static readonly string NativeContentAd = "NativeContentAd";


        protected TemplateView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TemplateView(Context context) : base(context)
        {

        }

        public TemplateView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitView(context, attrs);
        }

        public TemplateView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitView(context, attrs);
        }

        public TemplateView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context,
            attrs, defStyleAttr, defStyleRes)
        {
            InitView(context, attrs);
        }

        public void SetStyles(NativeTemplateStyle styles)
        {
            Styles = styles;
            ApplyStyles();
        }

        public NativeAdView GetNativeAdView()
        {
            return NativeAdView;
        }

        private void ApplyStyles()
        {
            try
            {
                Drawable mainBackground = Styles.GetMainBackgroundColor();
                if (mainBackground != null)
                {
                    Background.Background = mainBackground;
                    if (PrimaryView != null)
                    {
                        PrimaryView.Background = mainBackground;
                    }

                    if (SecondaryView != null)
                    {
                        SecondaryView.Background = mainBackground;
                    }

                    if (TertiaryView != null)
                    {
                        TertiaryView.Background = mainBackground;
                    }
                }

                Typeface primary = Styles.GetPrimaryTextTypeface();
                if (primary != null)
                {
                    PrimaryView?.SetTypeface(primary, TypefaceStyle.Normal);
                }

                Typeface secondary = Styles.GetSecondaryTextTypeface();
                if (secondary != null)
                {
                    SecondaryView?.SetTypeface(secondary, TypefaceStyle.Normal);
                }

                Typeface tertiary = Styles.GetTertiaryTextTypeface();
                if (tertiary != null)
                {
                    TertiaryView?.SetTypeface(tertiary, TypefaceStyle.Normal);
                }

                //Typeface ctaTypeface = Styles.GetCallToActionTextTypeface();
                //if (ctaTypeface != null)
                //{
                //    CallToActionView?.SetTypeface(ctaTypeface, TypefaceStyle.Normal);
                //}

                Color primaryTypefaceColor = Styles.GetPrimaryTextTypefaceColor();
                if (primaryTypefaceColor > 0)
                {
                    PrimaryView?.SetTextColor(primaryTypefaceColor);
                }

                Color secondaryTypefaceColor = Styles.GetSecondaryTextTypefaceColor();
                if (secondaryTypefaceColor > 0)
                {
                    SecondaryView?.SetTextColor(secondaryTypefaceColor);
                }

                Color tertiaryTypefaceColor = Styles.GetTertiaryTextTypefaceColor();
                if (tertiaryTypefaceColor > 0)
                {
                    TertiaryView?.SetTextColor(tertiaryTypefaceColor);
                }

                //var ctaTypefaceColor = Styles.GetCallToActionTypefaceColor();
                //if (ctaTypefaceColor > 0)
                //{
                //    CallToActionView?.SetTextColor(ctaTypefaceColor);
                //}

                //float ctaTextSize = Styles.GetCallToActionTextSize();
                //if (ctaTextSize > 0)
                //{
                //    CallToActionView?.SetTextSize(ComplexUnitType.Sp, ctaTextSize);
                //}

                float primaryTextSize = Styles.GetPrimaryTextSize();
                switch (primaryTextSize)
                {
                    case > 0:
                        PrimaryView?.SetTextSize(ComplexUnitType.Sp, primaryTextSize);
                        break;
                }

                float secondaryTextSize = Styles.GetSecondaryTextSize();
                switch (secondaryTextSize)
                {
                    case > 0:
                        SecondaryView?.SetTextSize(ComplexUnitType.Sp, secondaryTextSize);
                        break;
                }

                float tertiaryTextSize = Styles.GetTertiaryTextSize();
                switch (tertiaryTextSize)
                {
                    case > 0:
                        TertiaryView?.SetTextSize(ComplexUnitType.Sp, tertiaryTextSize);
                        break;
                }

                //Drawable ctaBackground = Styles.GetCallToActionBackgroundColor();
                //if (ctaBackground != null && CallToActionView != null)
                //{
                //    CallToActionView.Background = ctaBackground;
                //}

                Drawable primaryBackground = Styles.GetPrimaryTextBackgroundColor();
                if (primaryBackground != null && PrimaryView != null)
                {
                    PrimaryView.Background = primaryBackground;
                }

                Drawable secondaryBackground = Styles.GetSecondaryTextBackgroundColor();
                if (secondaryBackground != null && SecondaryView != null)
                {
                    SecondaryView.Background = secondaryBackground;
                }

                Drawable tertiaryBackground = Styles.GetTertiaryTextBackgroundColor();
                if (tertiaryBackground != null && TertiaryView != null)
                {
                    TertiaryView.Background = tertiaryBackground;
                }

                Invalidate();
                RequestLayout();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private bool AdHasOnlyStore(NativeAd nativeAd)
        {
            string store = nativeAd.Store;
            string advertiser = nativeAd.Advertiser;
            return !TextUtils.IsEmpty(store) && TextUtils.IsEmpty(advertiser);
        }

        public void SetNativeAd(NativeAd nativeAd)
        {
            try
            {
                NativeAd = nativeAd;

                string store = nativeAd.Store;
                string advertiser = nativeAd.Advertiser;
                string headline = nativeAd.Headline;
                string body = nativeAd.Body;
                string cta = nativeAd.CallToAction;
                //int starRating = Convert.ToInt32(nativeAd.StarRating);
                NativeAd.Image icon = nativeAd.Icon;

                string secondaryText;

                //if (CallToActionView != null)
                //    NativeAdView.CallToActionView = CallToActionView;

                NativeAdView.HeadlineView = PrimaryView;
                NativeAdView.MediaView = MediaView;
                SecondaryView.Visibility = ViewStates.Visible;
                if (AdHasOnlyStore(nativeAd))
                {
                    NativeAdView.StoreView = SecondaryView;
                    secondaryText = store;
                }
                else if (!TextUtils.IsEmpty(advertiser))
                {
                    NativeAdView.AdvertiserView = SecondaryView;
                    secondaryText = advertiser;
                }
                else
                {
                    secondaryText = "";
                }

                PrimaryView.Text = headline;

                //if (CallToActionView != null)
                //    CallToActionView.Text = cta;

                //  Set the secondary view to be the star rating if available.
                //if (starRating > 0)
                //{
                //    SecondaryView.Visibility=ViewStates.Gone;
                //    RatingBar.Visibility = ViewStates.Visible;
                //    RatingBar.Max=5;
                //    NativeAdView.StarRatingView=RatingBar;
                //}
                //else
                //{
                //    SecondaryView.Text=secondaryText;
                //    SecondaryView.Visibility = ViewStates.Visible;
                //    RatingBar.Visibility= ViewStates.Gone;
                //}

                if (string.IsNullOrEmpty(secondaryText))
                {
                    SecondaryView.Visibility = ViewStates.Gone;
                }
                else
                {
                    SecondaryView.Visibility = ViewStates.Visible;
                    SecondaryView.Text = secondaryText;
                }

                if (icon != null)
                {
                    IconView.Visibility = ViewStates.Visible;
                    IconView.SetImageDrawable(icon.Drawable);
                }
                else
                {
                    IconView.Visibility = ViewStates.Gone;
                }

                if (TertiaryView != null && !string.IsNullOrEmpty(body))
                {
                    TertiaryView.Text = body;
                    NativeAdView.BodyView = TertiaryView;
                }
                else if (TertiaryView != null)
                {
                    TertiaryView.Visibility = ViewStates.Gone;
                }

                NativeAdView.SetNativeAd(nativeAd);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// To prevent memory leaks, make sure to destroy your ad when you don't need it anymore.
        /// This method does not destroy the template view.
        /// </summary>
        public void DestroyNativeAd()
        {
            NativeAd.Destroy();
        }

        public string GetTemplateTypeName()
        {
            return TemplateType switch
            {
                Resource.Layout.gnt_medium_template_view => MediumTemplate,
                Resource.Layout.gnt_NativeContentAd_view => NativeContentAd,
                _ => ""
            };
        }

        private void InitView(Context context, IAttributeSet attributeSet)
        {
            try
            {
                TypedArray attributes = context.Theme.ObtainStyledAttributes(attributeSet, Resource.Styleable.TemplateView, 0, 0);

                try
                {
                    TemplateType = attributes.GetResourceId(Resource.Styleable.TemplateView_gnt_template_type, Resource.Layout.gnt_medium_template_view);
                }
                finally
                {
                    attributes.Recycle();
                }

                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                inflater.Inflate(TemplateType, this);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnFinishInflate()
        {
            try
            {
                base.OnFinishInflate();

                NativeAdView = (NativeAdView)FindViewById(Resource.Id.native_ad_view);

                switch (AppSettings.ShowAdMobNative)
                {
                    case false:
                        {
                            if (NativeAdView != null) NativeAdView.Visibility = ViewStates.Gone;
                            break;
                        }
                    default:
                        PrimaryView = (TextView)FindViewById(Resource.Id.primary);
                        SecondaryView = (TextView)FindViewById(Resource.Id.secondary);
                        TertiaryView = (TextView)FindViewById(Resource.Id.body);

                        //RatingBar = (RatingBar)FindViewById(Resource.Id.rating_bar);
                        //RatingBar.Enabled=false;

                        //CallToActionView = (AppCompatButton)FindViewById(Resource.Id.cta);
                        IconView = (ImageView)FindViewById(Resource.Id.icon);
                        MediaView = (MediaView)FindViewById(Resource.Id.media_view);
                        Background = (LinearLayout)FindViewById(Resource.Id.background);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void NativeContentAdView(NativeAd nativeAd)
        {
            try
            {
                NativeAdView = (NativeAdView)FindViewById(Resource.Id.nativeAdView);

                // Set other ad assets.
                NativeAdView.HeadlineView = NativeAdView.FindViewById(Resource.Id.contentad_headline);
                NativeAdView.BodyView = NativeAdView.FindViewById(Resource.Id.contentad_body);
                NativeAdView.CallToActionView = NativeAdView.FindViewById(Resource.Id.contentad_call_to_action);
                NativeAdView.IconView = NativeAdView.FindViewById(Resource.Id.contentad_logo);
                NativeAdView.AdvertiserView = NativeAdView.FindViewById(Resource.Id.contentad_advertiser);
                NativeAdView.ImageView = NativeAdView.FindViewById(Resource.Id.contentad_image);

                // The headline and mediaContent are guaranteed to be in every NativeAd.
                ((TextView)NativeAdView.HeadlineView).Text = nativeAd.Headline;

                // These assets aren't guaranteed to be in every NativeAd, so it's important to
                // check before trying to display them.
                if (string.IsNullOrEmpty(nativeAd.Body))
                {
                    NativeAdView.BodyView.Visibility = ViewStates.Gone;
                }
                else
                {
                    NativeAdView.BodyView.Visibility = ViewStates.Visible;
                    ((TextView)NativeAdView.BodyView).Text = nativeAd.Body;
                }

                if (string.IsNullOrEmpty(nativeAd.CallToAction))
                {
                    NativeAdView.CallToActionView.Visibility = ViewStates.Gone;
                }
                else
                {
                    NativeAdView.CallToActionView.Visibility = ViewStates.Visible;
                    ((AppCompatButton)NativeAdView.CallToActionView).Text = nativeAd.CallToAction;
                }

                switch (nativeAd.Icon)
                {
                    case null:
                        NativeAdView.IconView.Visibility = ViewStates.Gone;
                        break;
                    default:
                        ((ImageView)NativeAdView.IconView).SetImageDrawable(nativeAd.Icon.Drawable);
                        NativeAdView.IconView.Visibility = ViewStates.Visible;
                        break;
                }

                switch (nativeAd.Images?.Count)
                {
                    case 0:
                        NativeAdView.IconView.Visibility = ViewStates.Gone;
                        break;
                    default:
                        {
                            if (nativeAd.Images != null)
                                ((ImageView)NativeAdView.ImageView).SetImageDrawable(nativeAd.Images[0].Drawable);

                            NativeAdView.ImageView.Visibility = ViewStates.Visible;
                            break;
                        }
                }

                if (string.IsNullOrEmpty(nativeAd.Advertiser))
                {
                    NativeAdView.AdvertiserView.Visibility = ViewStates.Gone;
                }
                else
                {
                    ((TextView)NativeAdView.AdvertiserView).Text = nativeAd.Advertiser;
                    NativeAdView.AdvertiserView.Visibility = ViewStates.Visible;
                }

                // This method tells the Google Mobile Ads SDK that you have finished populating your
                // native ad view with this native ad.
                NativeAdView.SetNativeAd(nativeAd);

                // Get the video controller for the ad. One will always be provided, even if the ad doesn't
                // have a video asset.
                //VideoController vc = nativeAd.MediaContent?.VideoController;

                //switch (vc != null && vc.HasVideoContent)
                //{
                //    // Updates the UI to say whether or not this ad has a video asset.
                //    case true:
                //        //"Video status: Ad contains a %.2f:1 video asset."

                //        // Create a new VideoLifecycleCallbacks object and pass it to the VideoController. The
                //        // VideoController will call methods on this object when events occur in the video
                //        // lifecycle.
                //        vc.SetVideoLifecycleCallbacks(new MyVideoController());
                //        break;
                //    default:
                //        //"Video status: Ad does not contain a video asset."
                //        break;
                //}
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private class MyVideoController : VideoController.VideoLifecycleCallbacks
        {
            public MyVideoController()
            {

            }
        }
    }
}