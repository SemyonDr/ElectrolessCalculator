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

            //Getting current solution volume
            float cur_volume = current.TotalVolumeL;

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

            //Creating components list
            Dictionary<CmpType, Component> cs = new Dictionary<CmpType, Component>();


            //Creating components and adding them to the list
            ComponentFactory cf = new ComponentFactory();

            //Nickel sulfate
            Component ni_cmp = CreateCurrentComponentByAnalize(CmpType.NickelSulfate, NiSaltAn, cur_volume, cf);
            cs.Add(CmpType.NickelSulfate, ni_cmp);

            //Sodium hypophosphite
            Component hp_cmp;
            if (current.UseHPAnalize)
                hp_cmp = CreateCurrentComponentByAnalize(CmpType.SodiumHypophosphite, current.HypophosphiteAnalize, cur_volume, cf);
            else {
                hp_cmp = CreateCurrentComponentByProportion(CmpType.SodiumHypophosphite, proportion, cur_volume, target, cf);
            }
            cs.Add(CmpType.SodiumHypophosphite, hp_cmp);

            //Sodium Acetate
            Component sa_cmp = CreateCurrentComponentByProportion(CmpType.SodiumAcetate, proportion, cur_volume, target, cf);
            cs.Add(CmpType.SodiumAcetate, sa_cmp);

            //Succinic Acid
            Component sca_cmp = CreateCurrentComponentByProportion(CmpType.SuccinicAcid, proportion, cur_volume, target, cf);
            cs.Add(CmpType.SuccinicAcid, sca_cmp);

            //Lactic Acid
            Component la_cmp = CreateCurrentComponentByProportion(CmpType.LacticAcid, proportion, cur_volume, target, cf);
            cs.Add(CmpType.LacticAcid, la_cmp);

            //Returning the list
            return cs;
        }

        private Component CreateCurrentComponentByProportion(CmpType Type, float Proportion, float CurrentVolume, TargetSolution Target, ComponentFactory CFactory) {
            //Calculating current concentration of the component as fraction of target concentration
            float cur_conc = Target.GetConcentration(Type) * Proportion;
            //Calculating weigth in kg of the component in the current solution
            float cur_weigth = cur_conc * CurrentVolume / 1000.0f;
            //Creating component
            return CFactory.CreateComponent(Type, cur_weigth);
        }

        private Component CreateCurrentComponentByAnalize(CmpType Type, float Analize, float CurrentVolume, ComponentFactory CFactory) {
            //Calculating weigth in kg of the component in the current solution
            float cur_weigth = Analize * CurrentVolume / 1000.0f;
            //Creating component
            return CFactory.CreateComponent(Type, cur_weigth);
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
