using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentTimeMangementApp.Areas.Identity.Data;
using StudentTimeMangementApp.Data;
using StudentTimeMangementApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace StudentTimeMangementApp.Pages.Display
{
    // The Authorize attribute specifies that access to this class is restricted to authorized users
    [Authorize]
    public class ModulesTempInfoModel : PageModel
    {
        // Property for the DisplayValues object that is bound to the model
        // It is initialized with default values
        [BindProperty]
        public DisplayValues DisplayValues { get; set; } = new DisplayValues();

        // List of Modules objects
        public IList<Modules> Modules { get; set; }

        // Private read-only fields for the StudentTimeMangementAppContext and UserManager<RegistrationLoginUser>
        private readonly StudentTimeMangementAppContext _context;
        private readonly UserManager<RegistrationLoginUser> _userManager;

        // Constructor for the ModulesTempInfoModel class that takes a StudentTimeMangementAppContext and a UserManager<RegistrationLoginUser> as parameters
        public ModulesTempInfoModel(StudentTimeMangementAppContext context, UserManager<RegistrationLoginUser> userManager)
        {
            // Assigning the passed in context and userManager to the private fields
            _context = context;
            _userManager = userManager;
            Modules = new List<Modules>();  
        }

        // Method that handles the GET request asynchronously
        // It is used for adding the values to the Bar Graph
        public async Task<IActionResult> OnGetAsync()
        {
            // Get the user's ID
            var userId = _userManager.GetUserId(User);

            // Query the database to get the modules where the UsersID matches the current user's ID
            // Select only the ModuleName, Weeks, and SelfStudyHr fields
            // Convert the result to a list
            Modules = await _context.Modules.Where(m => m.UsersID == userId).Select(m => new Modules
            {
                ModuleName = m.ModuleName,
                Weeks = m.Weeks,
                SelfStudyHr = m.SelfStudyHr
            }).ToListAsync();

            return Page();
        }



        // Method that handles the POST request asynchronously
        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // If not, return the page
                return Page();
            }

            // Get the user's ID
            var userId = _userManager.GetUserId(User);

            // Query the database to get the first module where the ModuleName matches the DisplayValues.ModuleName and the UsersID matches the current user's ID
            var module = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleName == DisplayValues.ModuleName && m.UsersID == userId);
            if (module != null)
            {
                // Calculate the updated self-study hours
                DisplayValues.UpdatedSelfStudyHr = module.SelfStudyHr - DisplayValues.CaptureHrs;

                // Convert the string dates to DateTime objects
                DateTime moduleDate = DateTime.ParseExact(module.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime updateDate = DateTime.ParseExact(DisplayValues.UpdateDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                // Determine the week number based on the chosen date
                int weekNumber = GetWeekNumber(moduleDate, updateDate, module.Weeks);

                // Create a new DisplayValues object and set its properties
                var displayValues = new DisplayValues
                {
                    UsersID = userId,
                    ModuleName = DisplayValues.ModuleName,
                    UpdateDate = DisplayValues.UpdateDate,
                    UpdatedSelfStudyHr = DisplayValues.UpdatedSelfStudyHr,
                    CaptureHrs = DisplayValues.CaptureHrs,
                    WeekNumber = weekNumber
                };

                // Add the new DisplayValues object to the context
                _context.DisplayValues.Add(displayValues);

                // Update the SelfStudyHr of the module
                module.SelfStudyHr = DisplayValues.UpdatedSelfStudyHr;
                _context.Attach(module).State = EntityState.Modified;

                try
                {
                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If the module does not exist, return NotFound
                    // Otherwise, rethrow the exception
                    if (!ModuleExists(module.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Redirect to the Index page
            return RedirectToPage("/Index");
        }



        private int GetWeekNumber(DateTime startDate, DateTime chosenDate, int weeks)
        {
            // Calculate the end date based on the start date and number of weeks
            DateTime endDate = startDate.AddDays(weeks * 7);

            // Check if chosenDate is within the range
            if (chosenDate < startDate || chosenDate > endDate)
            {
                // Throw an exception if chosenDate is outside the valid range
                throw new ArgumentException("Chosen date is out of the range between start date and end date.");
            }

            // Calculate the week number
            int weekNumber = (chosenDate - startDate).Days / 7 + 1;

            // Return the calculated week number
            return weekNumber;
        }



        public string DateHandling(string input)
        {
            // Try to parse the input as a date in the format 'yyyy-MM-dd'
            if (!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                // If parsing fails, construct an error message and throw an ArgumentException
                string errorMessage = $"Please provide the date in the format 'yyyy-MM-dd', such as '2023-06-27'";
                throw new ArgumentException(errorMessage);
            }
            // If parsing is successful, return the original input
            return input;
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }


    }
}
