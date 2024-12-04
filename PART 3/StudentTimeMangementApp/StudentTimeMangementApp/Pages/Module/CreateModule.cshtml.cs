using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentTimeMangementApp.Areas.Identity.Data;
using StudentTimeMangementApp.Data;
using StudentTimeMangementApp.Models;

namespace StudentTimeMangementApp.Pages.Module
{
    [Authorize]
    public class CreateModuleModel : PageModel
    {
        // Private read-only fields for the StudentTimeMangementAppContext and UserManager<RegistrationLoginUser>
        private readonly StudentTimeMangementAppContext _context;
        private readonly UserManager<RegistrationLoginUser> _userManager;

        // Constructor for the CreateModuleModel class that takes a StudentTimeMangementAppContext and a UserManager<RegistrationLoginUser> as parameters
        public CreateModuleModel(StudentTimeMangementAppContext context, UserManager<RegistrationLoginUser> userManager)
        {
            // Assigning the passed in context and userManager to the private fields
            _context = context;
            _userManager = userManager;
        }

        // Method that handles the GET request and returns the page
        public IActionResult OnGet()
        {
            return Page();
        }

        // Property for the Modules object that is bound to the model
        [BindProperty]
        public Modules Modules { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string action)
        {
            // Check if the model state is valid, and if the context and Modules objects are not null
            if (!ModelState.IsValid || _context.Modules == null || Modules == null)
            {
                // If any of the conditions are not met, return the current page
                return Page();
            }

            // Calculate the SelfStudyHr property of the Modules object
            // It is calculated as (ModuleCredits * 10 / Weeks) - ModuleClassHrs
            Modules.SelfStudyHr = (int)(Modules.ModuleCredits * 10 / Modules.Weeks) - Modules.ModuleClassHrs;

            // Set the UsersID property of the Modules object to the current user's ID
            Modules.UsersID = _userManager.GetUserId(this.User);

            // Add the Modules object to the context
            _context.Modules.Add(Modules);
            // Save the changes to the context asynchronously
            await _context.SaveChangesAsync();

            // If the action parameter is "Add More"
            if (action == "Add More")
            {
                // Clear the Modules object and reset the form
                Modules = new Modules();
                // Clear the model state
                ModelState.Clear();
                // Return the current page
                return Page();
            }

            // If the action parameter is not "Add More", redirect to the Index page
            return RedirectToPage("/Index");
        }

    }
}
