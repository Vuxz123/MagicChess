using com.ethnicthv.Other.Skill.Params;
using UnityEngine;

namespace com.ethnicthv.Other.Skill
{
    public class SkillObject
    {
        public SkillObject(SkillParamsSchema skillParamsSchema, GameObject skillPrefab)
        {
            this.Schema = skillParamsSchema;
            this.Prefab = skillPrefab;
        }
        
        public SkillParamsSchema Schema { get; }

        public GameObject Prefab { get; }
    }
}