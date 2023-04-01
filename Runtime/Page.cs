using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Core
{
    public class Page : MonoBehaviour, IPage
    {
        public event Action PageReady, PageClosed;
        public bool IsVisible { get; private set; }

        protected IPage NextPage { private get; set; }
        protected GameObject Content;
        protected List<ITransitionEffect> PageElements;
        protected int TransitionedElements, TotalValidElements;

        protected virtual void Awake()
        {
            Content = transform.GetChild(0).gameObject;

            PageClosed += OnPageClosed;

            ScanForAnimators();
        }

        public void ScanForAnimators()
        {
            ResetPageAnimatorData();
            
            Content.SetActive(true);
            PageElements = GetComponentsInChildren<ITransitionEffect>().ToList();
            foreach (var pageElement in PageElements.Where(pageElement => !pageElement.IsInManualMode()))
            {
                pageElement.TransitionComplete += OnElementTransitionComplete;

                // Determine the total amount of elements we need to wait for that are NOT in manual mode.
                TotalValidElements++;
            }
            Content.SetActive(false);
        }

        private void ResetPageAnimatorData()
        {
            TotalValidElements = 0;
            if (PageElements == null)
            {
                PageElements = new List<ITransitionEffect>();
            }
            else
            {
                foreach (var pageElement in PageElements.Where(pageElement => !pageElement.IsInManualMode()))
                {
                    pageElement.TransitionComplete -= OnElementTransitionComplete;
                }
                PageElements.Clear();
            }
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnDestroy()
        {
            PageClosed -= OnPageClosed;
        }

        private void OnElementTransitionComplete(bool isVisible)
        {
            TransitionedElements++;

            if (TransitionedElements == TotalValidElements)
            {
                if (isVisible)
                {
                    PageReady?.Invoke();
                    IsVisible = true;
                }
                else
                {
                    PageClosed?.Invoke();
                    IsVisible = false;
                    Content.SetActive(false);
                }

                TransitionedElements = 0;
            }
        }

        public void SetContentState(bool state)
        {
            Content.SetActive(state);
        }

        public virtual void Open()
        {
            Content.SetActive(true);
            foreach (var element in PageElements.Where(element => !element.IsInManualMode()))
            {
                element.StartTransition(UIAnimatorTrigger.OnEntry);
            }
        }

        public void Close()
        {
            foreach (var element in PageElements.Where(element => !element.IsInManualMode()))
            {
                element.StartTransition(UIAnimatorTrigger.OnExit);
            }
        }

        public void TriggerNextPage(IPage page)
        {
            if (page == null)
            {
                Debug.LogError($"{gameObject.name} reports it's next page is null!");

                return;
            }

            NextPage = page;
            Close();
        }

        protected virtual void OnPageClosed()
        {
            if (NextPage != null)
            {
                NextPage.Open();
                NextPage = null;
            }
        }
    }
}
