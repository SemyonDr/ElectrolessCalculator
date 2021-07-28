using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class is used to calculate current composition and components to add.
    /// </summary>
    public class Calculator
    {
        public Dictionary<CmpType, Component> GetCurrentSolution(TargetSolution target, CurrentSolution current)
        {
            //Current bath composition is estimated by assuming that:
            //1) Bath was initialy assembled correctly (i.e. with exact target composition values)
            //2) Every component depletes at the same % rate as the others (i.e. if nickel depleted by 10%, so every other component is also depleted by 10%)

            //Nickel analize value shows metallic nickel concentration (Ni g/l)
            //Converting in to nickel salt concentration (Ni2So4 g/l)
            float NiSaltAn = NickelConverter.ConvertMetalToSalt(current.NickelAnalize);
            //Relation between measured concentration and targer concentration of Nickel Sulfate
            float proportionNi = NiSaltAn / target.GetConcentration(CmpType.NickelSulfate);
            //Relation between measured concentration and targer concentration of Sodium Hypophosphite
            float proportionHP = 1;
            if (current.UseHPAnalize)
                proportionHP = current.HypophosphiteAnalize / target.GetConcentration(CmpType.SodiumHypophosphite);

            //While current Nickel and Hypophosphite are defined directly by analize result
            //other components current concentrations are calculated using average between analize results
            float proportion;
            if (current.UseHPAnalize)
                //Use both analizes
                proportion = (proportionNi + proportionHP) / 2;
            else
                //Or only Nickel analize
                proportion = proportionNi;

            //Current solution components list
            Dictionary<CmpType, Component> cs = new Dictionary<CmpType, Component>();
            ComponentFactory cf = new ComponentFactory();

            //Nickel Sulfate
            //Concentration is converted to absolute units (kg)
            Component ni_cmp = cf.CreateComponent(CmpType.NickelSulfate, NiSaltAn * current.TotalVolumeL / 1000);
            cs.Add(CmpType.NickelSulfate, ni_cmp);

            //Sodium Hypophosphite
            Component hp_cmp;
            if (current.UseHPAnalize)
                //Concentration converted to absolute units (kg)
                hp_cmp = cf.CreateComponent(CmpType.SodiumHypophosphite, current.HypophosphiteAnalize * current.TotalVolumeL / 1000);
            else
                hp_cmp = cf.CreateComponent(CmpType.SodiumHypophosphite, target.Components[CmpType.SodiumHypophosphite].WeigthKg*proportion);
            cs.Add(CmpType.SodiumHypophosphite, hp_cmp);

            //Sodium Acetate
            Component sa_cmp = cf.CreateComponent(CmpType.SodiumAcetate, target.Components[CmpType.SodiumAcetate].WeigthKg * proportion);
            cs.Add(CmpType.SodiumAcetate, sa_cmp);

            //Succinic Acid
            Component sca_cmp = cf.CreateComponent(CmpType.SuccinicAcid, target.Components[CmpType.SuccinicAcid].WeigthKg * proportion);
            cs.Add(CmpType.SuccinicAcid, sca_cmp);

            //Lactic Acid
            Component la_cmp = cf.CreateComponent(CmpType.LacticAcid, target.Components[CmpType.LacticAcid].WeigthKg * proportion);
            cs.Add(CmpType.LacticAcid, la_cmp);

            return cs;
        }

        public Dictionary<CmpType, Component> GetRequiredMaterials(TargetSolution target, CurrentSolution current) {
            //Amount of required materials consists of two parts:
            //1) Materials to add to the current volume to make its concentrations equal to target concentrations, which is calculated by:
            //(target concentration - current concentration)*current volume
            //2) Materials to make up volume of solution which to be added to current volume to reach target total volume, wihch is calculated by:
            //target concentration * (target volume - current volume)
            //Final calculation is:
            //target weigth - current weigth

            //Required materials list
            Dictionary<CmpType, Component> required = new Dictionary<CmpType, Component>();
            ComponentFactory cmpFactory = new ComponentFactory();

            //Nickel Sulfate
            Component ni_cmp = cmpFactory.CreateComponent(CmpType.NickelSulfate, 
                target.Components[CmpType.NickelSulfate].WeigthKg - current.Components[CmpType.NickelSulfate].WeigthKg);
            required.Add(CmpType.NickelSulfate, ni_cmp);

            //Sodium Hypophosphite
            Component hp_cmp = cmpFactory.CreateComponent(CmpType.SodiumHypophosphite,
                target.Components[CmpType.SodiumHypophosphite].WeigthKg - current.Components[CmpType.SodiumHypophosphite].WeigthKg);
            required.Add(CmpType.SodiumHypophosphite, hp_cmp);

            //Sodium Acetate
            Component sa_cmp = cmpFactory.CreateComponent(CmpType.SodiumAcetate,
                target.Components[CmpType.SodiumAcetate].WeigthKg - current.Components[CmpType.SodiumAcetate].WeigthKg);
            required.Add(CmpType.SodiumAcetate, sa_cmp);

            //Succinic Acid
            Component sca_cmp = cmpFactory.CreateComponent(CmpType.SuccinicAcid,
                target.Components[CmpType.SuccinicAcid].WeigthKg - current.Components[CmpType.SuccinicAcid].WeigthKg);
            required.Add(CmpType.SuccinicAcid, sca_cmp);

            //Lactic Acid
            Component la_cmp = cmpFactory.CreateComponent(CmpType.LacticAcid,
                target.Components[CmpType.LacticAcid].WeigthKg - current.Components[CmpType.LacticAcid].WeigthKg);
            required.Add(CmpType.LacticAcid, la_cmp);

            return required;
        }
    }

}
