using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticTracking.Model;

public class DeliveryModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeliveryId { get; set; }

    [Required]
    public string ParcelDetails { get; set; }

    [Required]
    public string SenderAddress { get; set; }

    [Required]
    public string ReceiverAddress { get; set; }

    [Required]
    public DateTime PickupTime { get; set; }

    public DateTime? DeliveryTime { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Payment { get; set; }

    [Required]
    public DeliveryStatus Status { get; set; }

    [Required]
    public int DeliveryPartnerId { get; set; }

    [ForeignKey("DeliveryPartnerId")]
    public string EmergencyDetails { get; set; } = string.Empty;
}