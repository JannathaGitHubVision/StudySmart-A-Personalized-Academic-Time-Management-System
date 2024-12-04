using PoeDLL;
using System.Collections.Generic;

namespace SemesterPOE_Project
{
    public class DataStoring
    {
        // List to store ModulesInfo objects
        private List<ModulesInfo> modulesList;
        private List<SemesterDatesWeeks> semesterList;


        // Method to receive the RecipeIngredients list
        public void SetModulesData(List<ModulesInfo> modules, List<SemesterDatesWeeks> semesterDatesWeeks)
        {
            modulesList = modules;
            semesterList = semesterDatesWeeks;
            //SelfStudy listOfModules = new SelfStudy();
            //listOfModules.GetValuesCalculate(modulesList, semesterList);
        }


    }
}