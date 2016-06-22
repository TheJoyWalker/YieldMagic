using System;

public interface IEasingContainer
{
    /// <summary>
    /// easing function
    /// </summary>
    /// <param name="progress">[0-1]</param>
    /// <returns></returns>
    float Ease(float progress);
}