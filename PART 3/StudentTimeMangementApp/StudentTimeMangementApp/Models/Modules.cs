using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTimeMangementApp.Models
{// The Modules class represents the model for capturing and managing academic modules.

    public class Modules
    {
        // Unique identifier for each module.
        public int Id { get; set; }

        // Foreign key to associate modules with specific users.
        [Column(TypeName = "nvarchar(50)")]
        public string? UsersID { get; set; }

        // Display name for the module, required field.
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Module Name")]
        public string? ModuleName { get; set; }

        // Code assigned to the module, required field.
        [Column(TypeName = "nvarchar(20)")]
        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Module Code")]
        public string? ModuleCode { get; set; }

        // Credits associated with the module, required field.
        [Required(ErrorMessage = "This Field is Required")]
        public int? ModuleCredits { get; set; }

        // Total class hours for the module, required field.
        [Required(ErrorMessage = "This Field is Required")]
        public int ModuleClassHrs { get; set; }

        // Number of weeks the module spans, required field.
        [Required(ErrorMessage = "This Field is Required")]
        public int Weeks { get; set; }

        // String representation of the date for the module.
        public string Date { get; set; }

        // Converted DateTime property for better handling of dates, required field.
        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Date)]
        public DateTime? DateString
        {
            get
            {
                DateTime date;
                return DateTime.TryParse(Date, out date) ? date : (DateTime?)null;
            }
        }

        // Self-study hours associated with the module.
        public int SelfStudyHr { get; set; }
    }

}
