namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.meta_planificado")]
    public partial class meta_planificado
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int metaid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ejercicio { get; set; }

        public int? enero_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string enero_string { get; set; }

        public decimal? enero_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? enero_tiempo { get; set; }

        public int? febrero_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string febrero_string { get; set; }

        public decimal? febrero_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? febrero_tiempo { get; set; }

        public int? marzo_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string marzo_string { get; set; }

        public decimal? marzo_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? marzo_tiempo { get; set; }

        public int? abril_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string abril_string { get; set; }

        public decimal? abril_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? abril_tiempo { get; set; }

        public int? mayo_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string mayo_string { get; set; }

        public decimal? mayo_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? mayo_tiempo { get; set; }

        public int? junio_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string junio_string { get; set; }

        public decimal? junio_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? junio_tiempo { get; set; }

        public int? julio_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string julio_string { get; set; }

        public decimal? julio_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? julio_tiempo { get; set; }

        public int? agosto_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string agosto_string { get; set; }

        public decimal? agosto_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? agosto_tiempo { get; set; }

        public int? septiembre_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string septiembre_string { get; set; }

        public decimal? septiembre_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? septiembre_tiempo { get; set; }

        public int? octubre_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string octubre_string { get; set; }

        public decimal? octubre_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? octubre_tiempo { get; set; }

        public int? noviembre_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string noviembre_string { get; set; }

        public decimal? noviembre_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? noviembre_tiempo { get; set; }

        public int? diciembre_entero { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string diciembre_string { get; set; }

        public decimal? diciembre_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? diciembre_tiempo { get; set; }

        public int estado { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_ingreso { get; set; }

        public virtual meta meta { get; set; }
    }
}
