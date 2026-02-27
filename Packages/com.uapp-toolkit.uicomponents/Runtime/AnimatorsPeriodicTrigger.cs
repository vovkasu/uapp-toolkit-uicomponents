using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAppToolkit.UIComponents
{
    public class AnimatorsPeriodicTrigger : MonoBehaviour
    {
        public string InitialTrigger;
        public string PeriodicTrigger;
        public float PeriodSec = 1f;
        private Coroutine _triggeringTask;

        public void StartTriggering(List<Animator> animators)
        {
            StartTriggering(animators, PeriodicTrigger, PeriodSec, InitialTrigger);
        }

        public void StartTriggering(List<Animator> animators, string periodicTrigger, float periodSec, string initialTrigger)
        {
            if (animators == null || animators.Count == 0)
            {
                return;
            }
            if (!string.IsNullOrEmpty(initialTrigger))
            {
                foreach (var animator in animators)
                {
                    animator.SetTrigger(initialTrigger);
                }
            }
            if(_triggeringTask != null)
            {
                StopCoroutine(_triggeringTask);
            }
            _triggeringTask = StartCoroutine(TriggerAnimatorsAsync(animators, periodicTrigger, periodSec));
        }

        private IEnumerator TriggerAnimatorsAsync(List<Animator> animators, string periodicTrigger, float periodSec)
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger(periodicTrigger);
                yield return new WaitForSeconds(periodSec);
            }
        }
    }
}