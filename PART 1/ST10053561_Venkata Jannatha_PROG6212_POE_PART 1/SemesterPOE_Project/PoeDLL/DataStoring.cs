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
            ListOfModules listOfModules = new ListOfModules();
            listOfModules.GetValuesCalculate(modulesList, semesterList);
        }



        public SemesterDatesWeeks saveSemesterValues(int SemesterWeeks, string SemesterDate)
        {
            //Creating a new Semesterdates to store values in a constructor
            SemesterDatesWeeks semesterDates = new SemesterDatesWeeks(SemesterWeeks, SemesterDate);

            //return the semester dates.
            return semesterDates;
        }
    }
}