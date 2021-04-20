using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Orientation = Android.Widget.Orientation;

namespace WoWonder.Library.Anjo.StoriesProgressView
{
    public class StoriesProgressView : LinearLayout
    {
        private readonly LayoutParams ProgressBarLayoutParam = new LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1);
        private readonly LayoutParams SpaceLayoutParam = new LayoutParams(5, ViewGroup.LayoutParams.WrapContent);

        private readonly List<PausableProgressBar> ProgressBars = new List<PausableProgressBar>();

        private int StoriesCount = -1;
        //pointer of running animation
        private int Current = -1;
        private IStoriesListener StoriesListener;
        bool  IsComplete;

        private bool   IsSkipStart;
        private bool  IsReverseStart;

        public interface IStoriesListener
        {
            void OnNext();

            void OnPrev();

            void OnComplete();
        }


        protected StoriesProgressView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public StoriesProgressView(Context context) : base(context)
        {
            Init(context, null);
        }

        public StoriesProgressView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public StoriesProgressView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public StoriesProgressView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context, attrs);
        }

        private void Init(Context context, IAttributeSet attrs)
        {
            Orientation = Orientation.Horizontal;
            if (attrs != null)
            {
                TypedArray typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.StoriesProgressView);
                StoriesCount = typedArray.GetInt(Resource.Styleable.StoriesProgressView_progressCount, 0);
                typedArray.Recycle(); 
            }
            BindViews();
        }

        private void BindViews()
        {
            ProgressBars.Clear();
            RemoveAllViews();

            for (int i = 0; i < StoriesCount; i++)
            {
                PausableProgressBar p = CreateProgressBar();
                ProgressBars.Add(p);
                AddView(p);
                if (i + 1 < StoriesCount)
                {
                    AddView(CreateSpace());
                }
            }
        }

        private PausableProgressBar CreateProgressBar()
        {
            PausableProgressBar p = new PausableProgressBar(Context) {LayoutParameters = ProgressBarLayoutParam};
            return p;
        }

        private View CreateSpace()
        {
            View v = new View(Context) {LayoutParameters = SpaceLayoutParam};
            return v;
        }

        /**
    * Set story count and create views
    *
    * @param storiesCount story count
    */
        public void SetStoriesCount(int storiesCount)
        {
            StoriesCount = storiesCount;
            BindViews();
        }
         
        /// <summary>
        /// Set storiesListener
        /// </summary>
        /// <param name="storiesListener">StoriesListener</param>
        public void SetStoriesListener(IStoriesListener storiesListener)
        {
            StoriesListener = storiesListener;
        }

        /// <summary>
        /// Skip current story
        /// </summary>
        public void Skip()
        {
            if (IsSkipStart || IsReverseStart) return;
            switch (IsComplete)
            {
                case true:
                    return;
            }
            switch (Current)
            {
                case < 0:
                    return;
            }
            PausableProgressBar p = ProgressBars[Current];
            IsSkipStart = true;
            p.SetMax();
        }

        /// <summary>
        /// Reverse current story
        /// </summary>
        public void Reverse()
        {
            if (IsSkipStart || IsReverseStart) return;
            switch (IsComplete)
            {
                case true:
                    return;
            }
            switch (Current)
            {
                case < 0:
                    return;
            }
            PausableProgressBar p = ProgressBars[Current];
            IsReverseStart = true;
            p.SetMin();
        }
         
        /// <summary>
        /// Set a story's duration
        /// </summary>
        /// <param name="duration">millisecond</param>
        public void SetStoryDuration(long duration)
        {
            for (int i = 0; i < ProgressBars.Count; i++)
            {
                ProgressBars[i].SetDuration(duration);
                ProgressBars[i].SetCallback(Callback(i));
            }
        }

        /// <summary>
        /// Set stories count and each story duration
        /// </summary>
        /// <param name="durations"></param>
        public void SetStoriesCountWithDurations(long[] durations)
        {
            StoriesCount = durations.Length;
            BindViews();
            for (int i = 0; i < ProgressBars.Count; i++)
            {
                ProgressBars[i].SetDuration(durations[i]);
                ProgressBars[i].SetCallback(Callback(i));
            }
        }


        private PausableProgressBar.ICallback Callback(int index)
        {
            return new MyPassableProgressBar(this , index);
        }
         
        private class MyPassableProgressBar : PausableProgressBar.ICallback
        {
            private readonly int Index;
            private readonly StoriesProgressView ProgressView;

            public MyPassableProgressBar(StoriesProgressView progressView , int index)
            {
                ProgressView = progressView;
                Index = index;
            }
            public void OnStartProgress()
            {
                ProgressView.Current = Index;
            }

            public void OnFinishProgress()
            {
                switch (ProgressView.IsReverseStart)
                {
                    case true:
                    {
                        ProgressView.StoriesListener?.OnPrev();
                        switch (ProgressView.Current - 1)
                        {
                            case <= 0:
                            {
                                PausableProgressBar p = ProgressView.ProgressBars[ProgressView.Current - 1];
                                p.SetMinWithoutCallback();
                                ProgressView.ProgressBars[--ProgressView.Current].StartProgress();
                                break;
                            }
                            default:
                                ProgressView.ProgressBars[ProgressView.Current].StartProgress();
                                break;
                        }
                        ProgressView.IsReverseStart = false;
                        return;
                    }
                }
                int next = ProgressView.Current + 1;
                if (next <= ProgressView.ProgressBars.Count - 1)
                {
                    ProgressView.StoriesListener?.OnNext();
                    ProgressView.ProgressBars[next].StartProgress();
                }
                else
                {
                    ProgressView.IsComplete = true;
                    ProgressView.StoriesListener?.OnComplete();
                }
                ProgressView.IsSkipStart = false;
            }
        }


        /// <summary>
        /// Start progress animation
        /// </summary>
        public void StartStories()
        {
            ProgressBars[0].StartProgress();
        }

        /// <summary>
        /// Start progress animation from specific progress
        /// </summary>
        /// <param name="from"></param>
        public void StartStories(int from)
        {
            for (int i = 0; i < from; i++)
            {
                ProgressBars[i].SetMaxWithoutCallback();
            }
            ProgressBars[from].StartProgress();
        }

        /// <summary>
        /// Need to call when Activity or Fragment destroy
        /// </summary>
        public void Destroy()
        {
            IsComplete = false;
            foreach (var p in ProgressBars)
            {
                p.Clear();
            }
        }

        /// <summary>
        /// Pause story
        /// </summary>
        public void Pause()
        {
            switch (Current)
            {
                case < 0:
                    return;
                default:
                    ProgressBars[Current].PauseProgress();
                    break;
            }
        }

        /// <summary>
        /// Resume story
        /// </summary>
        public void Resume()
        {
            switch (Current)
            {
                case < 0:
                    return;
                default:
                    ProgressBars[Current].ResumeProgress();
                    break;
            }
        }
    }
}