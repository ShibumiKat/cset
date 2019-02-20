//////////////////////////////// 
// 
//   Copyright 2018 Battelle Energy Alliance, LLC  
// 
// 
//////////////////////////////// 

using DataLayerCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSET_Main.SALS
{
    /// <summary>
    /// I am changing this from a normalized structure to a flat one 
    /// moving between the database and here.  
    /// </summary>
    public class NistSpecialFactor
    {

        /**The idea is you give me the list and I will push it to the properties
         * or you ask for the list and I pull it from the properties. 
         */


        public string Type_Value { get; set; }
        public string Confidentiality_Special_Factor { get; set; }
        public string Integrity_Special_Factor { get; set; }
        public string Availability_Special_Factor { get; set; }
        public SALLevelNIST Confidentiality_Value { get; set; }
        public SALLevelNIST Integrity_Value { get; set; }
        public SALLevelNIST Availability_Value { get; set; }


        public void loadFromDb(int assessmentId, CSET_Context db)
        {
            NistProcessingLogic nistProcessing = new NistProcessingLogic();
            List<CNSS_CIA_JUSTIFICATIONS> ciavalues = db.CNSS_CIA_JUSTIFICATIONS.Where(x => x.Assessment_Id == assessmentId).ToList<CNSS_CIA_JUSTIFICATIONS>();
            foreach (CNSS_CIA_JUSTIFICATIONS cia in ciavalues)
            {
                switch (cia.CIA_Type.ToLower())
                {
                    case "availability":
                        this.Availability_Special_Factor = cia.Justification;
                        this.Availability_Value = nistProcessing.GetWeightPair(cia.DropDownValueLevel.ToLower());
                        break;
                    case "confidentiality":
                        this.Confidentiality_Special_Factor = cia.Justification;
                        this.Confidentiality_Value = nistProcessing.GetWeightPair(cia.DropDownValueLevel.ToLower());
                        break;
                    case "integrity":
                        this.Integrity_Special_Factor = cia.Justification;
                        this.Integrity_Value = nistProcessing.GetWeightPair(cia.DropDownValueLevel.ToLower());
                        break;
                }
            }
        }





        /// <summary>
        /// I don't like how this is not general but 
        /// entity wants to keep the object references.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        public void SaveToDb(int id, CSET_Context db)
        {
            NistProcessingLogic nistProcessing = new NistProcessingLogic();
            var dblist = db.CNSS_CIA_JUSTIFICATIONS.Where(x => x.Assessment_Id == id).AsEnumerable<CNSS_CIA_JUSTIFICATIONS>();
            Dictionary<String, CNSS_CIA_JUSTIFICATIONS> dbValues = dblist.ToDictionary(x => x.CIA_Type.ToLower(), x => x);

            CNSS_CIA_JUSTIFICATIONS cnvalu;

            // Availability
            if (!String.IsNullOrWhiteSpace(this.Availability_Special_Factor))
            {
                cnvalu = getOrCreateNew("availability", id, dbValues, db);
                cnvalu.Justification = this.Availability_Special_Factor == null ? String.Empty : this.Availability_Special_Factor;
                cnvalu.DropDownValueLevel = this.Availability_Value.SALName;
            }

            // Confidentiality
            if (!String.IsNullOrWhiteSpace(this.Confidentiality_Special_Factor))
            {
                cnvalu = getOrCreateNew("confidentiality", id, dbValues, db);
                cnvalu.Justification = this.Confidentiality_Special_Factor == null ? string.Empty : this.Confidentiality_Special_Factor;
                cnvalu.DropDownValueLevel = this.Confidentiality_Value.SALName;
            }

            // Integrity
            if (!String.IsNullOrWhiteSpace(this.Integrity_Special_Factor))
            {
                cnvalu = getOrCreateNew("integrity", id, dbValues, db);
                cnvalu.Justification = this.Integrity_Special_Factor == null ? String.Empty : this.Integrity_Special_Factor;
                cnvalu.DropDownValueLevel = this.Integrity_Value.SALName;
            }

            db.SaveChanges();
            CSETWeb_Api.BusinessLogic.Helpers.AssessmentUtil.TouchAssessment(id);
        }

        private CNSS_CIA_JUSTIFICATIONS getOrCreateNew(String ciaType, int id, Dictionary<String, CNSS_CIA_JUSTIFICATIONS> dbValues, CSET_Context db)
        {
            CNSS_CIA_JUSTIFICATIONS cnvalu;
            if (dbValues.TryGetValue(ciaType, out cnvalu))
            {
                return cnvalu;
            }
            else
            {
                CNSS_CIA_JUSTIFICATIONS rval = new CNSS_CIA_JUSTIFICATIONS() { Assessment_Id = id, CIA_Type = UCF(ciaType) };
                db.CNSS_CIA_JUSTIFICATIONS.Add(rval);
                return rval;
            }
        }

        /// <summary>
        /// Upper Case the First Letter (UCF)
        /// </summary>
        /// <param name="s"></param>
        /// <returns>the string with the first letter capitalized</returns>
        string UCF(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }

}


