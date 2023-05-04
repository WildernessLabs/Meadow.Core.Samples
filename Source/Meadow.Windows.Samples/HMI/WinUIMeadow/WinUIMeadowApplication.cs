// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Meadow;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIMeadow
{
    public class WinUIMeadowApplication<T> : Application, IApp
        where T : class, IMeadowDevice
    {
        public CancellationToken CancellationToken => throw new NotImplementedException();

        public static T Device => Resolver.Services.Get<IMeadowDevice>() as T;

        protected WinUIMeadowApplication()
        {
        }

        public void InvokeOnMainThread(Action<object?> action, object? state = null)
        {
            Dispatcher.UIThread.Post(() => action(state));
        }

        virtual public Task OnError(Exception e)
        {
            return Task.CompletedTask;
        }

        virtual public Task OnShutdown()
        {
            return Task.CompletedTask;
        }

        virtual public void OnUpdate(Version newVersion, out bool approveUpdate)
        {
            approveUpdate = true;
        }

        virtual public void OnUpdateComplete(Version oldVersion, out bool rollbackUpdate)
        {
            rollbackUpdate = false;
        }

        virtual public Task Run()
        {
            return Task.CompletedTask;
        }

        public virtual Task InitializeMeadow()
        {
            return Task.CompletedTask;
        }

        Task IApp.Initialize()
        {
            return InitializeMeadow();
        }

        protected void LoadMeadowOS()
        {
            new Thread((o) =>
            {
                _ = MeadowOS.Start(this, null);
            })
            {
                IsBackground = true
            }
            .Start();
        }
    }
}
