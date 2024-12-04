using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTimeMangementApp.Models
{
    // The DisplayValues class represents a model for displaying summarized information
    // about the updated values of a captured academic module.

    public class DisplayValues
    {
        // Unique identifier for each display value.
        public int Id { get; set; }

        // Foreign key to associate display values with specific users.
        [Column(TypeName = "nvarchar(50)")]
        public string? UsersID { get; set; }

        // Display name for the module, required field.
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Module Name")]
        public string ModuleName { get; set; }

        // Date when the module information was last updated, required field.
        [Required(ErrorMessage = "This Field is Required")]
        public string UpdateDate { get; set; }

        // Updated self-study hours associated with the module, required field.
        [Required(ErrorMessage = "This Field is Required")]
        public int UpdatedSelfStudyHr { get; set; }

        // Total captured hours for the module, required field.
        [Required(ErrorMessage = "This Field is Required")]
        public int CaptureHrs { get; set; }

        // Week number associated with the module.
        public int WeekNumber { get; set; }
    }

}


