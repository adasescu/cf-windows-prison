﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.WindowsPrison.Restrictions
{
    class CPU : Rule
    {
        public override void Apply(Prison prison)
        {
            // prison.JobObject.CPUPercentageLimit = prison.Rules.CPUPercentageLimit;
        }

        public override void Destroy(Prison prison)
        {
        }

        public override RuleInstanceInfo[] List()
        {
            return new RuleInstanceInfo[0];
        }

        public override void Init()
        {
        }

        public override RuleTypes RuleType
        {
            get
            {
                return RuleTypes.CPU;
            }
        }
        public override void Recover(Prison prison)
        {
        }
    }
}
