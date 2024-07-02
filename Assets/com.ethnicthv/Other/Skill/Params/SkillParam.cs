namespace com.ethnicthv.Other.Skill.Params
{
    public abstract class SkillParam
    {
        string name;
        
        public SkillParam(string name)
        {
            this.name = name;
        }

        public string Name => name;
        
        
    }
}