using System;
using System.Collections.Generic;
using com.ethnicthv.Other.Skill.Params;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.ethnicthv.Other.Skill
{
    public class SkillMain : ISkillEmitter, ISkillRegister
    {
        private Dictionary<int, SkillObject> _skillObjects = new();
        

        public void Init()
        {
            
        }

        public void EmitSkill(int skillId, Vector3 position, Quaternion rotation, SkillParams skillParams)
        {
            var skill = _skillObjects[skillId];
            var skillPrefab = skill.Prefab;
            var skillObject = Object.Instantiate(skillPrefab, position, rotation);
            var controller = skillObject.GetComponent<SkillController>();
            controller.Initialize(skillParams);
            
        }

        public void RegisterSkill(int skillId, SkillParamsSchema skillParamsSchema, GameObject skillPrefab)
        {
            if(_skillObjects.ContainsKey(skillId))
                throw new Exception("Skill with id " + skillId + " already registered");
            _skillObjects.Add(skillId, new SkillObject(skillParamsSchema, skillPrefab));
        }

        public void UnregisterSkill(int skillId)
        {
            _skillObjects.Remove(skillId);
        }
    }
}