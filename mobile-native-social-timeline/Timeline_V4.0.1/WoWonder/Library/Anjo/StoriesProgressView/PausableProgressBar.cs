using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using JetBrains.Annotations;
using WoWonder.Helpers.Utils;

namespace WoWonder.Library.Anjo.StoriesProgressView
{
    public class PausableProgressBar : FrameLayout
    {
        private static readonly int DefaultProgressDuration = 2000;

        private View FrontProgressView;
        private View MaxProgressView;

        private new PassableScaleAnimation Animation;
        private long Duration = DefaultProgressDuration;
        private ICallback Callback;

        public interface ICallback
        {
            void OnStartProgress();
            void OnFinishProgress(); 
        }


        protected PausableProgressBar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public PausableProgressBar([NotNull] Context context) : base(context)
        {
            Init(context);
        }

        public PausableProgressBar([NotNull] Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public PausableProgressBar([NotNull] Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context);
        }

        public PausableProgressBar([NotNull] Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            try
            {
                LayoutInflater.From(context)?.Inflate(Resource.Layout.pausable_progress, this);
                FrontProgressView = FindViewById(Resource.Id.front_progress);
                MaxProgressView = FindViewById(Resource.Id.max_progress); // work around 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
         
        public void SetDuration(long duration)
        {
            Duration = duration;
        }

        public void SetCallback(ICallback callback)
        {
            Callback = callback;
        }

        public void SetMax()
        {
            FinishProgress(true);
        }

        public void SetMin()
        {
            FinishProgress(false);
        }

        public void SetMinWithoutCallback()
        {
            MaxProgressView.SetBackgroundResource(Resource.Color.progress_secondary);

            MaxProgressView.Visibility = ViewStates.Visible;
            if (Animation != null)
            {
                Animation.SetAnimationListener(null);
                Animation.Cancel();
            }
        }

        public void SetMaxWithoutCallback()
        {
            MaxProgressView.SetBackgroundResource(Resource.Color.progress_max_active);

            MaxProgressView.Visibility = ViewStates.Visible;
            if (Animation != null)
            {
                Animation.SetAnimationListener(null);
                Animation.Cancel();
            }
        }

        private void FinishProgress(bool  isMax)
        {
            switch (isMax)
            {
                case true:
                    MaxProgressView.SetBackgroundResource(Resource.Color.progress_max_active);
                    break;
            }
            MaxProgressView.Visibility =  isMax ? ViewStates.Visible : ViewStates.Gone;
            if (Animation != null)
            {
                Animation.SetAnimationListener(null);
                Animation.Cancel();
                Callback?.OnFinishProgress();
            }
        }

        public void StartProgress()
        {
            MaxProgressView.Visibility = ViewStates.Gone;

            Animation = new PassableScaleAnimation(0, 1, 1, 1, Dimension.Absolute, 0, Dimension.RelativeToSelf, 0)
            {
                Duration = Duration, Interpolator = new LinearInterpolator()
            };
            Animation.SetAnimationListener(new MyAnimationListener(this));
            Animation.FillAfter = true;
            FrontProgressView.StartAnimation(Animation);
        }


        public void PauseProgress()
        {
            Animation?.Pause();
        }

        public void ResumeProgress()
        {
            Animation?.Resume();
        }

        public void Clear()
        {
            if (Animation != null)
            {
                Animation.SetAnimationListener(null);
                Animation.Cancel();
                Animation = null;
            }
        }
         
        private class MyAnimationListener : Java.Lang.Object, Animation.IAnimationListener
        {
            private readonly PausableProgressBar ProgressBar;
            public MyAnimationListener(PausableProgressBar progressBar)
            {
                ProgressBar = progressBar;
            }

            public void OnAnimationEnd(Animation animation)
            {
                ProgressBar.Callback?.OnFinishProgress();
            }

            public void OnAnimationRepeat(Animation animation)
            {
               
            }

            public void OnAnimationStart(Animation animation)
            {
                try
                {
                    ProgressBar.FrontProgressView.Visibility = ViewStates.Visible;
                    ProgressBar.Callback?.OnStartProgress();
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }


        private class PassableScaleAnimation : ScaleAnimation
        {
            private long MElapsedAtPause = 0;
            private bool  MPaused = false;

            protected PassableScaleAnimation(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
            {
            }

            public PassableScaleAnimation(Context context, IAttributeSet attrs) : base(context, attrs)
            {
            }

            public PassableScaleAnimation(float fromX, float toX, float fromY, float toY) : base(fromX, toX, fromY, toY)
            {
            }

            public PassableScaleAnimation(float fromX, float toX, float fromY, float toY, float pivotX, float pivotY) : base(fromX, toX, fromY, toY, pivotX, pivotY)
            {
            }

            public PassableScaleAnimation(float fromX, float toX, float fromY, float toY, Dimension pivotXType, float pivotXValue, Dimension pivotYType, float pivotYValue) : base(fromX, toX, fromY, toY, pivotXType, pivotXValue, pivotYType, pivotYValue)
            {
            }

            public override bool GetTransformation(long currentTime, Transformation outTransformation, float scale)
            {
                MElapsedAtPause = MPaused switch
                {
                    true when MElapsedAtPause == 0 => currentTime - StartTime,
                    _ => MElapsedAtPause
                };
                StartTime = MPaused switch
                {
                    true => currentTime - MElapsedAtPause,
                    _ => StartTime
                };

                return base.GetTransformation(currentTime, outTransformation, scale);
            }

            /// <summary>
            /// pause animation
            /// </summary>
            public void Pause()
            {
                switch (MPaused)
                {
                    case true:
                        return;
                    default:
                        MElapsedAtPause = 0;
                        MPaused = true;
                        break;
                }
            }

            /// <summary>
            /// resume animation
            /// </summary>
            public void Resume()
            {
                MPaused = false;
            }

        }
    }
}