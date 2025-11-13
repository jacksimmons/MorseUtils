// Sanitised 29/9/24
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Static class for handling operations related to aspect ratios.
/// </summary>
public static class AspectRatio
{
    public static bool AreResolutionsEqual(Resolution a, Resolution b)
    {
        int aWidth = a.width;
        int aHeight = a.height;
        int aHz = Mathf.RoundToInt((float)a.refreshRateRatio.value);

        int bWidth = b.width;
        int bHeight = b.height;
        int bHz = Mathf.RoundToInt((float)b.refreshRateRatio.value);

        if (aWidth == bWidth && aHeight == bHeight && aHz == bHz)
            return true;
        return false;
    }


    /// <summary>
    /// Gets an array of resolutions which are 16:9, and distinct enough - e.g. 1080p 59.4Hz and 1080 59.9Hz are not distinct enough.
    /// Each resolution must also have width no more than that of the native resolution, otherwise it is discarded.
    /// The one which is added first remains and subsequent non-distinct resolutions are discarded, and displayed refresh rate is rounded up.
    /// </summary>
    public static Resolution[] GetSupportedResolutions()
    {
        // Eliminate "identical" refresh rates (e.g. choice of 59.9Hz and 60Hz is annoying)
        Resolution firstRes = Screen.resolutions[0];
        int prevW = firstRes.width, prevH = firstRes.height;
        int prevHz = Mathf.RoundToInt((float)firstRes.refreshRateRatio.value);
        List<Resolution> uniqueRefreshRateAndValidAspectRatioResolutions = new(Screen.resolutions);

        // Go from last element in the list, down to the second element in the list.
        for (int i = Screen.resolutions.Length - 1; i >= 1; i--)
        {
            Resolution res = Screen.resolutions[i];
            Resolution prev = Screen.resolutions[i - 1];

            if (AreResolutionsEqual(res, prev))
            {
                uniqueRefreshRateAndValidAspectRatioResolutions.Remove(prev);
            }
        }

        return uniqueRefreshRateAndValidAspectRatioResolutions.ToArray();
    }
}