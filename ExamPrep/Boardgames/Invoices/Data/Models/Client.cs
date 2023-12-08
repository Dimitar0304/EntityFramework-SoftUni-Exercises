using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
    public class Client
    {
        public Client()
        {
            this.Invoices = new List<Invoice>();
            this.Addresses = new List<Address>();
            this.ProductsClients = new List<ProductClient>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        [StringLength(15)]
        public string NumberVat { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; } = null!;
        public virtual ICollection<Address> Addresses { get; set; } = null!;
        public virtual ICollection<ProductClient> ProductsClients { get; set; } = null!;
    }
}