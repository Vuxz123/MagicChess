using com.ethnicthv.Other.Skill.Params;
using UnityEngine;

namespace com.ethnicthv.Other.Skill.S
{
    public class TestSkillImpl: MonoBehaviour, IActivator
    {
        private bool isActivated = false;
        
        private SkillParams skillParams;

        public float Interval { get; set; }

        public void Activate(SkillParams skillParams)
        {
            isActivated = true;
            this.skillParams = skillParams;
        }
        
        public void Deactivate()
        {
            isActivated = false;
        }
        
        private void Update()
        {
            if (!isActivated) return;
        }
    }
}