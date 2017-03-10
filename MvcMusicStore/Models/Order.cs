using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcMusicStore.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Se requiere el Nombre")]
        [DisplayName("First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Se requiere el Apellido")]
        [DisplayName("Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Se requiere Direccion")]
        [StringLength(70)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Se requiere la Ciudad")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "Se requiere el Estado")]
        [StringLength(40)]
        public string State { get; set; }

        [Required(ErrorMessage = "Se requiere el Codigo de Postal")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Se requiere el País")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Se requiere el Telefono")]
        [StringLength(24)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Se requiere el Correo Electronico")]
        [DisplayName("Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "El correo electronico no es valido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
