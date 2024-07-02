using com.ethnicthv.Other.Skill.Params;

namespace com.ethnicthv.Other.Skill
{
    public interface IActivator
    {
        float Interval { get; set;}

        void Activate(SkillParams skillParams);
        
        void Deactivate();
    }
}