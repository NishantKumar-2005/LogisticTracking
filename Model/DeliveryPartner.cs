using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticTracking.Model;
public class DeliveryPartner
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeliveryPartnerId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string ContactNumber { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PayPerDelivery { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPayment { get; set; }

    [Required]
    public PartnerStatus Status { get; set; }

    public List<int> DeliveriesId { get; set; }
}