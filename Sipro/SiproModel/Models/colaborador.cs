
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the colaborador table.
    /// </summary>
	[Table("colaborador")]
	public partial class Colaborador
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string pnombre { get; set; }
	    public virtual string snombre { get; set; }
	    public virtual string papellido { get; set; }
	    public virtual string sapellido { get; set; }
	    public virtual long cui { get; set; }
	    [ForeignKey("Unidadejecutora")]
        public virtual int unidad_ejecutoraunidad_ejecutora { get; set; }
	    [ForeignKey("Usuario")]
        public virtual string usuariousuario { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    [ForeignKey("Unidadejecutora")]
        public virtual int entidad { get; set; }
	    [ForeignKey("Unidadejecutora")]
        public virtual int ejercicio { get; set; }
		public virtual Usuario tusuario { get; set; }
		public virtual Unidadejecutora tunidadejecutora { get; set; }
		public virtual IEnumerable<Colaborador> colaboradors { get; set; }
	}
}
