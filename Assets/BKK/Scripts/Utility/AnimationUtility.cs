using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BKK.Utility
{
    public static class AnimationUtility
    {
        public static AnimatorControllerParameterType GetAnimatorParameterType(this Animator animator,
            string parameterName)
        {
            for (var index = 0; index < animator.parameters.Length; index++)
            {
                var param = animator.GetParameter(index);

                if (param.name == parameterName)
                {
                    return param.type;
                }
            }

            return (AnimatorControllerParameterType) (-1);
        }

        public static bool CheckParameter(this Animator animator, string paramName)
        {
            return animator.parameters.Any(param => param.name == paramName);
        }
    }
}
