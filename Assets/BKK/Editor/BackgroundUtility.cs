using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class BackgroundUtility
{
    public static void StartBackgroundTask(Task update, Action end = null)
    {
        async void ClosureCallback()
        {
            try
            {
                await update;

                if (update.IsCompleted)
                {
                    end?.Invoke();

                    EditorApplication.update -= ClosureCallback;
                }
            }
            catch (Exception exception)
            {
                end?.Invoke();

                Debug.LogException(exception);
                EditorApplication.update -= ClosureCallback;
            }
        }

        EditorApplication.update += ClosureCallback;
    }
}
