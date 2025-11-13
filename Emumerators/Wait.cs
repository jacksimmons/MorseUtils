using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Helper class with many methods of waiting for things to happen (via coroutines).
/// </summary>
public static class Wait
{
    /// <summary>
    /// Waits for <paramref name="durationSecs"/> seconds, then calls <paramref name="then"/> if non-null and terminates.
    /// </summary>
    public static IEnumerator WaitThen(float durationSecs, Action then = null)
    {
        yield return new WaitForSeconds(durationSecs);
        then?.Invoke();
        yield return null;
    }


    /// <summary>
    /// Every <paramref name="intervalSecs"/>, checks if <paramref name="getCondition"/> returns true.
    /// If so, calls <paramref name="then"/> if non-null and terminates.
    /// </summary>
    public static IEnumerator WaitUntilConditionThen(Func<bool> getCondition, float intervalSecs, Action then = null)
    {
        while (!getCondition())
        {
            yield return new WaitForSeconds(intervalSecs);
        }
        then?.Invoke();
        yield return null;
    }


    /// <summary>
    /// Every <paramref name="intervalSecs"/>, checks if any condition (trueCondition) in <paramref name="getConditionList"/> returns true.
    /// If so, calls the equivalent of <code><paramref name="thenList"/>[<paramref name="getConditionList"/>.IndexOf(trueCondition)]</code>
    /// if non-null and terminates.
    /// Expects <paramref name="getConditionList"/> to have same length as <paramref name="thenList"/>.
    /// </summary>
    public static IEnumerator WaitUntilConditionThen(List<Func<bool>> getConditionList, float intervalSecs, List<Action> thenList = null)
    {
        int GetFirstTrueCondition()
        {
            for (int i = 0; i < getConditionList.Count; i++)
            {
                if (getConditionList[i]())
                {
                    return i;
                }
            }
            return -1;
        }

        float secondsWaited = 0;
        int trueIndex = GetFirstTrueCondition();
        while (trueIndex < 0)
        {
            trueIndex = GetFirstTrueCondition();
            yield return new WaitForSeconds(intervalSecs);
            secondsWaited += intervalSecs;
        }

        thenList?[trueIndex].Invoke();
        yield return null;
    }


    /// <summary>
    /// Every <paramref name="interval"/> seconds, calls <paramref name="exitEarlyCond"/>.
    /// If it returned `true`, then call <paramref name="then"/> if non-null and return.
    /// If it returned `false`, keep waiting.
    /// 
    /// Once <paramref name="durationSecs"/> seconds have passed, call <paramref name="checkAfterWaiting"/>.
    /// If it returned `true`, then call <paramref name="then"/> if non-null and return.
    /// If it returned `false`, keep waiting.
    /// </summary>
    /// <param name="checkAfterWaiting"></param>
    /// <param name="exitEarlyCond"></param>
    /// <param name="interval"></param>
    /// <param name="then"></param>
    /// <returns></returns>
    public static IEnumerator WaitThenAdvanced(float durationSecs, Func<bool> checkAfterWaiting, Func<bool> exitEarlyCond, float interval, Action then = null)
    {
        float secondsWaited = 0;
        while (true)
        {
            // Exit early
            if (exitEarlyCond()) break;

            yield return new WaitForSeconds(interval);

            secondsWaited += interval;

            // After waiting long enough, check
            if (secondsWaited > durationSecs && checkAfterWaiting()) break;
        }
        then?.Invoke();
        yield return null;
    }


    public static IEnumerator WaitForConditionMeanwhile(Func<float, bool> getConditionMeanwhile)
    {
        while (!getConditionMeanwhile(Time.deltaTime))
        {
            yield return new WaitForEndOfFrame();

        }
        yield return null;
    }


    /// <summary>
    /// Uses a findObj function to attempt to locate the object every waitTime, and then calls the "then" action
    /// with the found object as a parameter.
    public static IEnumerator WaitForObjectThen<T>(Func<T> findObj, float secondsToWait,
        Action<T> then = null) where T : class
    {
        T obj = null;
        while (obj == null)
        {
            obj = findObj();
            yield return new WaitForSeconds(secondsToWait);
        }
        then?.Invoke(obj);
        yield return null;
    }


    public static IEnumerator LoadSceneThenWait(string sceneName,
        Func<bool> conditionToActivate, float secondsToWait)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        load.allowSceneActivation = false;

        while (!load.isDone)
        {
            if (load.progress >= 0.9f)
            {
                if (conditionToActivate())
                    load.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(secondsToWait);
        }

        yield return null;
    }
}