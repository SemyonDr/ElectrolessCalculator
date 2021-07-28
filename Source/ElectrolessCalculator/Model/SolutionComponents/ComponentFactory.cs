using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    //Creates components by Type
    public class ComponentFactory
    {
        public Component CreateComponent(CmpType Type, float Weigth) {
            switch (Type)
            {
                case CmpType.NickelSulfate:
                    return new NickelSulfateCmp(Weigth);
                case CmpType.SodiumHypophosphite:
                    return new SodiumHypophosphiteCmp(Weigth);
                case CmpType.SodiumAcetate:
                    return new SodiumAcetateCmp(Weigth);
                case CmpType.SuccinicAcid:
                    return new SuccinicAcidCmp(Weigth);
                case CmpType.LacticAcid:
                    return new LacticAcidCmp(Weigth);
                default:
                    return null;
            }
        }
    }
}
