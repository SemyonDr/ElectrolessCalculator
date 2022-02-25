using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class represents settings-holding object for application.
    /// It holds settings and sets object values from settings.
    /// Also reads and saves settings values to file.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Name of the file that stores settings on disc.
        /// File of type .xml, but saved as .dat to avoid accidental editing by user.
        /// </summary>
        [XmlIgnore]
        public const string FileName = "Settings.dat";
        /// <summary>
        /// Holds errors during setting file reding.
        /// </summary>
        // Errors class is defined as subclass of this class.
        private DeserializationErrors deserializationErrors;

        #region SETTINGS LIST
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        public float Volume;
        public float NickelSulfate;
        public float SodiumHypophosphite;
        public float SodiumAcetate;
        public float SuccinicAcid;
        public float LacticAcid;

        #endregion


        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor creates settings object with default settings values.
        /// </summary>
        public Settings()
        {
            //Default values
            Volume = 300;
            NickelSulfate = 9.3993f;
            SodiumHypophosphite = 6.9f;
            SodiumAcetate = 20f;
            SuccinicAcid = 20f;
            LacticAcid = 20f;

            deserializationErrors = new DeserializationErrors();
        }

        #endregion


        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Reads values from target solution object and stores them in this Setting object.
        /// </summary>
        /// <param name="target">Target solution to read from.</param>
        public void UpdateFromObject(TargetSolution target) {
            Volume = target.TotalVolumeL;

            NickelSulfate = target.Components[CmpType.NickelSulfate].WeigthKg;
            SodiumHypophosphite = target.Components[CmpType.SodiumHypophosphite].WeigthKg;
            SodiumAcetate = target.Components[CmpType.SodiumAcetate].WeigthKg;
            SuccinicAcid = target.Components[CmpType.SuccinicAcid].WeigthKg;
            LacticAcid = target.Components[CmpType.LacticAcid].WeigthKg;
        }

        /// <summary>
        /// Set values stored in Settings class on target solution object.
        /// </summary>
        /// <param name="target"></param>
        public void SetObject(TargetSolution target) {
            target.TotalVolumeL = Volume;

            target.Components[CmpType.NickelSulfate].WeigthKg = NickelSulfate;
            target.Components[CmpType.SodiumHypophosphite].WeigthKg = SodiumHypophosphite;
            target.Components[CmpType.SodiumAcetate].WeigthKg = SodiumAcetate;
            target.Components[CmpType.SuccinicAcid].WeigthKg = SuccinicAcid;
            target.Components[CmpType.LacticAcid].WeigthKg = LacticAcid;
        }

        #endregion


        #region FILE HANDLING LOGIC
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Writes setting to file using XML serializer.
        /// </summary>
        /// <returns>Message describing saving results.</returns>
        public string SaveToFile() {
            //Getting full path to the file
            string fullPath = Path.GetFullPath(FileName);
            string directoryName = Path.GetDirectoryName(fullPath);

            //Creating settings file if it doesn't exist
            bool exists = File.Exists(FileName);
            if (!exists) {
                FileStream new_file = File.Create(FileName);
                new_file.Close();
                if (new_file == null)
                    return "Cannot create the setting file";
            }

            //Checking if file is accessible for application for writing.
            
            FileIOPermission file_write_permission = new FileIOPermission(FileIOPermissionAccess.Write, fullPath);
            try {
                file_write_permission.Demand();
            }
            catch {
                return "Writing access to the settings file denied";
            }

            //Trying to open file in mode that clears file content
            FileStream fs = new FileStream(FileName, FileMode.Truncate);
            if (fs == null)
                return "Cannot open the settings file";

            //Writing
            StreamWriter writer = new StreamWriter(fs);
            XmlSerializer ser = new XmlSerializer(typeof(Settings));

            ser.Serialize(writer, this);
            writer.Close();

            return "Settings saved";
        }


        /// <summary>
        /// Reads settings from file.
        /// </summary>
        /// <returns>Message describing reading results.</returns>
        public string UpdateFromFile() {
            //Resetting deserealization errors
            deserializationErrors.Reset();

            //Getting full path to the file
            string fullPath = Path.GetFullPath(FileName);

            //Checking if file exists
            bool exists = File.Exists(FileName);
            if (!exists)
                return "The settings file not found";

            //Checking if file is accessible for application for reading
            FileIOPermission file_read_permission = new FileIOPermission(FileIOPermissionAccess.Read, fullPath);
            try {
                file_read_permission.Demand();
            }
            catch {
                return "Reading access to the settings file denied";
            }

            //Trying to open file for reading
            FileStream fs = new FileStream(FileName, FileMode.Open);
            if (fs == null)
                return "Failed to open file";

            //Setting xml serializer
            XmlSerializer ser = new XmlSerializer(typeof(Settings));
            ser.UnknownAttribute += DeserialUnknownAtributeHandler;
            ser.UnknownElement += DeserialUnknownElementHandler;
            ser.UnknownNode += DeserialUnknownNodeHandler;
            ser.UnreferencedObject += DeserealUnreferencedObjectHandler;

            //Reading the file
            Settings new_set = new Settings();
            bool xml_error = false;
            try {
                new_set = (Settings)ser.Deserialize(fs);
            }
            catch {
                xml_error = true;
            }

            fs.Close();

            //Checking for errors
            if (deserializationErrors.HasError || xml_error) {
                //Trying to reset the file
                string reset_message = SaveToFile();

                if (reset_message == "Settings saved")
                    return "File format is incorrect. File reset";
                else
                    return "File format is incorrect. Failed to reset the file";
            }
            else {
                Volume = new_set.Volume;
                NickelSulfate = new_set.NickelSulfate;
                SodiumHypophosphite = new_set.SodiumHypophosphite;
                SodiumAcetate = new_set.SodiumAcetate;
                SuccinicAcid = new_set.SuccinicAcid;
                LacticAcid = new_set.LacticAcid;

                return "Settings loaded";
            }
        }

        private void DeserealUnreferencedObjectHandler(object sender, UnreferencedObjectEventArgs e) {
            deserializationErrors.UnreferencedObject = true;
        }

        private void DeserialUnknownNodeHandler(object sender, XmlNodeEventArgs e) {
            deserializationErrors.UnknownNode = true;
        }

        private void DeserialUnknownElementHandler(object sender, XmlElementEventArgs e) {
            deserializationErrors.UnknownElement = true;
        }

        private void DeserialUnknownAtributeHandler(object sender, XmlAttributeEventArgs e) {
            deserializationErrors.UnknownAttributeError = true;
        }


        /// <summary>
        /// Class that reflects deserialization errors.
        /// </summary>
        private class DeserializationErrors {
            public bool UnknownAttributeError = false;
            public bool UnknownElement = false;
            public bool UnknownNode = false;
            public bool UnreferencedObject = false;


            public bool HasError {
                get {
                    return UnknownAttributeError || UnknownElement || UnknownNode || UnreferencedObject;
                }
            }

            public void Reset() {
                UnknownAttributeError = false;
                UnknownElement = false;
                UnknownNode = false;
                UnreferencedObject = false;
            }
        }
        #endregion
    }
}
