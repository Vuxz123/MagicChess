using System;
using com.ethnicthv.Other.Skill.Params;
using Unity.VisualScripting;
using UnityEngine;

namespace com.ethnicthv.Other.Skill
{
    public class SkillController : MonoBehaviour
    {
        IActivator activator;
        
        private SkillParams _skillParams;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(SkillParams skillParams)
        {
            _skillParams = skillParams;
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
            activator.Activate(_skillParams);
        }
        
        public void Deactivate()
        {
            activator.Deactivate();
            gameObject.SetActive(false);
        }
        
        public void SetInterval(float interval)
        {
            activator.Interval = interval;
        }
        
        public float GetInterval()
        {
            return activator.Interval;
        }
    }
}