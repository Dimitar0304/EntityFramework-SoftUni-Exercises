using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public CategoryType Category { get; set; }
        [Required]
        public DateTime ProductionDate  { get; set; }
        [Required]
        public DateTime ExpiryDate  { get; set; }
        [Required]
        [MaxLength(100)]
        public string Producer { get; set; }
        [Required]
        [ForeignKey(nameof(Pharmacy))]
        public int PharmacyId  { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; } = null!;
    }
}