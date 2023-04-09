using Infrastructure;
using ApplicationLifeCycle.Messages;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace ApplicationLifeCycle
{
    /// <summary>
    /// An entry point to the application, where we bind all the common dependencies to the root DI scope.
    /// </summary>
    public class ApplicationLifeCycle : LifetimeScope
    {
        [SerializeField] UpdateRunner m_UpdateRunner;
        [Tooltip("The first scene that is to be loaded once the ApplicationController is initialized.")]
        [SerializeField] string m_LandingSceneName;

        IDisposable m_Subscriptions;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponent(m_UpdateRunner);

            builder.RegisterInstance(new MessageChannel<QuitApplicationMessage>()).AsImplementedInterfaces();
        }

        private void Start()
        {
            var quitApplicationSub = Container.Resolve<ISubscriber<QuitApplicationMessage>>();

            var subHandles = new DisposableGroup();
            subHandles.Add(quitApplicationSub.Subscribe(QuitGame));
            m_Subscriptions = subHandles;

            Application.wantsToQuit += OnWantToQuit;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(m_UpdateRunner.gameObject);
            Application.targetFrameRate = 120;
            SceneManager.LoadScene(m_LandingSceneName);
        }

        private bool IsItSafeToQuit()
        {
            // determine if anything is currently unsaved

            return true;
        }

        /// <summary>
        /// In builds, outgoing network requests may not send if we are quitting on the same frame.
        /// So we need to delay just briefly to allow requests to happen (though we needn't wait for their results).
        /// </summary>
        private IEnumerator SaveStatesBeforeQuit()
        {
            // We want to quit anyways, so if anything happens while trying to leave, log the exception and carry on
            try 
            {
                // Close networking stuff here
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
            yield return null;
            QuitGame(new QuitApplicationMessage());
        }

        private bool OnWantToQuit() 
        {
            // safely exit any online services
            var canQuit = IsItSafeToQuit();
            if (!canQuit)
            {
                StartCoroutine(SaveStatesBeforeQuit());
            }
            return canQuit;
        }

        private void QuitGame(QuitApplicationMessage msg)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif  
        }
    }
}