namespace Sipro.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<actividad> actividad { get; set; }
        public virtual DbSet<actividad_propiedad> actividad_propiedad { get; set; }
        public virtual DbSet<actividad_propiedad_valor> actividad_propiedad_valor { get; set; }
        public virtual DbSet<actividad_tipo> actividad_tipo { get; set; }
        public virtual DbSet<actividad_usuario> actividad_usuario { get; set; }
        public virtual DbSet<acumulacion_costo> acumulacion_costo { get; set; }
        public virtual DbSet<asignacion_raci> asignacion_raci { get; set; }
        public virtual DbSet<atipo_propiedad> atipo_propiedad { get; set; }
        public virtual DbSet<autorizacion_tipo> autorizacion_tipo { get; set; }
        public virtual DbSet<categoria_adquisicion> categoria_adquisicion { get; set; }
        public virtual DbSet<colaborador> colaborador { get; set; }
        public virtual DbSet<componente> componente { get; set; }
        public virtual DbSet<componente_propiedad> componente_propiedad { get; set; }
        public virtual DbSet<componente_propiedad_valor> componente_propiedad_valor { get; set; }
        public virtual DbSet<componente_sigade> componente_sigade { get; set; }
        public virtual DbSet<componente_tipo> componente_tipo { get; set; }
        public virtual DbSet<componente_usuario> componente_usuario { get; set; }
        public virtual DbSet<cooperante> cooperante { get; set; }
        public virtual DbSet<ctipo_propiedad> ctipo_propiedad { get; set; }
        public virtual DbSet<dato_tipo> dato_tipo { get; set; }
        public virtual DbSet<desembolso> desembolso { get; set; }
        public virtual DbSet<desembolso_tipo> desembolso_tipo { get; set; }
        public virtual DbSet<documento> documento { get; set; }
        public virtual DbSet<ejecucion_estado> ejecucion_estado { get; set; }
        public virtual DbSet<entidad> entidad { get; set; }
        public virtual DbSet<estado> estado { get; set; }
        public virtual DbSet<estado_informe> estado_informe { get; set; }
        public virtual DbSet<estado_tabla> estado_tabla { get; set; }
        public virtual DbSet<etiqueta> etiqueta { get; set; }
        public virtual DbSet<formulario> formulario { get; set; }
        public virtual DbSet<formulario_item> formulario_item { get; set; }
        public virtual DbSet<formulario_item_opcion> formulario_item_opcion { get; set; }
        public virtual DbSet<formulario_item_tipo> formulario_item_tipo { get; set; }
        public virtual DbSet<formulario_item_valor> formulario_item_valor { get; set; }
        public virtual DbSet<formulario_tipo> formulario_tipo { get; set; }
        public virtual DbSet<hito> hito { get; set; }
        public virtual DbSet<hito_resultado> hito_resultado { get; set; }
        public virtual DbSet<hito_tipo> hito_tipo { get; set; }
        public virtual DbSet<informe_presupuesto> informe_presupuesto { get; set; }
        public virtual DbSet<interes_tipo> interes_tipo { get; set; }
        public virtual DbSet<linea_base> linea_base { get; set; }
        public virtual DbSet<meta> meta { get; set; }
        public virtual DbSet<meta_avance> meta_avance { get; set; }
        public virtual DbSet<meta_planificado> meta_planificado { get; set; }
        public virtual DbSet<meta_tipo> meta_tipo { get; set; }
        public virtual DbSet<meta_unidad_medida> meta_unidad_medida { get; set; }
        public virtual DbSet<objeto_formulario> objeto_formulario { get; set; }
        public virtual DbSet<objeto_prestamo> objeto_prestamo { get; set; }
        public virtual DbSet<objeto_recurso> objeto_recurso { get; set; }
        public virtual DbSet<objeto_riesgo> objeto_riesgo { get; set; }
        public virtual DbSet<pago_planificado> pago_planificado { get; set; }
        public virtual DbSet<pep_detalle> pep_detalle { get; set; }
        public virtual DbSet<permiso> permiso { get; set; }
        public virtual DbSet<plan_adquisicion> plan_adquisicion { get; set; }
        public virtual DbSet<plan_adquisicion_pago> plan_adquisicion_pago { get; set; }
        public virtual DbSet<prestamo> prestamo { get; set; }
        public virtual DbSet<prestamo_tipo> prestamo_tipo { get; set; }
        public virtual DbSet<prestamo_tipo_prestamo> prestamo_tipo_prestamo { get; set; }
        public virtual DbSet<prestamo_usuario> prestamo_usuario { get; set; }
        public virtual DbSet<prodtipo_propiedad> prodtipo_propiedad { get; set; }
        public virtual DbSet<producto> producto { get; set; }
        public virtual DbSet<producto_propiedad> producto_propiedad { get; set; }
        public virtual DbSet<producto_propiedad_valor> producto_propiedad_valor { get; set; }
        public virtual DbSet<producto_tipo> producto_tipo { get; set; }
        public virtual DbSet<producto_usuario> producto_usuario { get; set; }
        public virtual DbSet<programa> programa { get; set; }
        public virtual DbSet<programa_propiedad> programa_propiedad { get; set; }
        public virtual DbSet<programa_propiedad_valor> programa_propiedad_valor { get; set; }
        public virtual DbSet<programa_proyecto> programa_proyecto { get; set; }
        public virtual DbSet<programa_tipo> programa_tipo { get; set; }
        public virtual DbSet<progtipo_propiedad> progtipo_propiedad { get; set; }
        public virtual DbSet<proyecto> proyecto { get; set; }
        public virtual DbSet<proyecto_impacto> proyecto_impacto { get; set; }
        public virtual DbSet<proyecto_miembro> proyecto_miembro { get; set; }
        public virtual DbSet<proyecto_propiedad> proyecto_propiedad { get; set; }
        public virtual DbSet<proyecto_propiedad_valor> proyecto_propiedad_valor { get; set; }
        public virtual DbSet<proyecto_rol_colaborador> proyecto_rol_colaborador { get; set; }
        public virtual DbSet<proyecto_tipo> proyecto_tipo { get; set; }
        public virtual DbSet<proyecto_usuario> proyecto_usuario { get; set; }
        public virtual DbSet<ptipo_propiedad> ptipo_propiedad { get; set; }
        public virtual DbSet<rectipo_propiedad> rectipo_propiedad { get; set; }
        public virtual DbSet<recurso> recurso { get; set; }
        public virtual DbSet<recurso_propiedad> recurso_propiedad { get; set; }
        public virtual DbSet<recurso_tipo> recurso_tipo { get; set; }
        public virtual DbSet<recurso_unidad_medida> recurso_unidad_medida { get; set; }
        public virtual DbSet<riesgo> riesgo { get; set; }
        public virtual DbSet<riesgo_propiedad> riesgo_propiedad { get; set; }
        public virtual DbSet<riesgo_propiedad_valor> riesgo_propiedad_valor { get; set; }
        public virtual DbSet<riesgo_tipo> riesgo_tipo { get; set; }
        public virtual DbSet<rol> rol { get; set; }
        public virtual DbSet<rol_permiso> rol_permiso { get; set; }
        public virtual DbSet<rol_unidad_ejecutora> rol_unidad_ejecutora { get; set; }
        public virtual DbSet<rol_usuario_proyecto> rol_usuario_proyecto { get; set; }
        public virtual DbSet<rtipo_propiedad> rtipo_propiedad { get; set; }
        public virtual DbSet<sctipo_propiedad> sctipo_propiedad { get; set; }
        public virtual DbSet<subcomponente> subcomponente { get; set; }
        public virtual DbSet<subcomponente_propiedad> subcomponente_propiedad { get; set; }
        public virtual DbSet<subcomponente_propiedad_valor> subcomponente_propiedad_valor { get; set; }
        public virtual DbSet<subcomponente_tipo> subcomponente_tipo { get; set; }
        public virtual DbSet<subcomponente_usuario> subcomponente_usuario { get; set; }
        public virtual DbSet<subprodtipo_propiedad> subprodtipo_propiedad { get; set; }
        public virtual DbSet<subproducto> subproducto { get; set; }
        public virtual DbSet<subproducto_propiedad> subproducto_propiedad { get; set; }
        public virtual DbSet<subproducto_propiedad_valor> subproducto_propiedad_valor { get; set; }
        public virtual DbSet<subproducto_tipo> subproducto_tipo { get; set; }
        public virtual DbSet<subproducto_usuario> subproducto_usuario { get; set; }
        public virtual DbSet<tipo_adquisicion> tipo_adquisicion { get; set; }
        public virtual DbSet<tipo_moneda> tipo_moneda { get; set; }
        public virtual DbSet<unidad_ejecutora> unidad_ejecutora { get; set; }
        public virtual DbSet<unidad_medida> unidad_medida { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
        public virtual DbSet<usuario_log> usuario_log { get; set; }
        public virtual DbSet<usuario_permiso> usuario_permiso { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<actividad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .Property(e => e.duracion_dimension)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .Property(e => e.latitud)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .Property(e => e.longitud)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .Property(e => e.treePath)
                .IsUnicode(false);

            modelBuilder.Entity<actividad>()
                .HasMany(e => e.actividad_propiedad_valor)
                .WithRequired(e => e.actividad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<actividad>()
                .HasMany(e => e.actividad_usuario)
                .WithRequired(e => e.actividad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<actividad_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_propiedad>()
                .HasMany(e => e.actividad_propiedad_valor)
                .WithRequired(e => e.actividad_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<actividad_propiedad>()
                .HasMany(e => e.atipo_propiedad)
                .WithRequired(e => e.actividad_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<actividad_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_tipo>()
                .HasMany(e => e.actividad)
                .WithRequired(e => e.actividad_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<actividad_tipo>()
                .HasMany(e => e.atipo_propiedad)
                .WithRequired(e => e.actividad_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<actividad_usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<actividad_usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<acumulacion_costo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<acumulacion_costo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<acumulacion_costo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<acumulacion_costo>()
                .HasMany(e => e.actividad)
                .WithOptional(e => e.acumulacion_costo1)
                .HasForeignKey(e => e.acumulacion_costo);

            modelBuilder.Entity<asignacion_raci>()
                .Property(e => e.rol_raci)
                .IsUnicode(false);

            modelBuilder.Entity<asignacion_raci>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<asignacion_raci>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<atipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<atipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<autorizacion_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<autorizacion_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<categoria_adquisicion>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<categoria_adquisicion>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<categoria_adquisicion>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<categoria_adquisicion>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<categoria_adquisicion>()
                .HasMany(e => e.plan_adquisicion)
                .WithOptional(e => e.categoria_adquisicion1)
                .HasForeignKey(e => e.categoria_adquisicion);

            modelBuilder.Entity<colaborador>()
                .Property(e => e.pnombre)
                .IsUnicode(false);

            modelBuilder.Entity<colaborador>()
                .Property(e => e.snombre)
                .IsUnicode(false);

            modelBuilder.Entity<colaborador>()
                .Property(e => e.papellido)
                .IsUnicode(false);

            modelBuilder.Entity<colaborador>()
                .Property(e => e.sapellido)
                .IsUnicode(false);

            modelBuilder.Entity<colaborador>()
                .Property(e => e.usuariousuario)
                .IsUnicode(false);

            modelBuilder.Entity<colaborador>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<colaborador>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<colaborador>()
                .HasMany(e => e.asignacion_raci)
                .WithRequired(e => e.colaborador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<colaborador>()
                .HasMany(e => e.proyecto_rol_colaborador)
                .WithRequired(e => e.colaborador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<colaborador>()
                .HasMany(e => e.proyecto_miembro)
                .WithRequired(e => e.colaborador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<colaborador>()
                .HasMany(e => e.proyecto)
                .WithOptional(e => e.colaborador)
                .HasForeignKey(e => e.director_proyecto);

            modelBuilder.Entity<componente>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .Property(e => e.latitud)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .Property(e => e.longitud)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .Property(e => e.duracion_dimension)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .Property(e => e.treePath)
                .IsUnicode(false);

            modelBuilder.Entity<componente>()
                .HasMany(e => e.componente_usuario)
                .WithRequired(e => e.componente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<componente>()
                .HasMany(e => e.componente_propiedad_valor)
                .WithRequired(e => e.componente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<componente>()
                .HasMany(e => e.subcomponente)
                .WithRequired(e => e.componente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<componente_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<componente_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<componente_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_propiedad>()
                .HasMany(e => e.componente_propiedad_valor)
                .WithRequired(e => e.componente_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<componente_propiedad>()
                .HasMany(e => e.ctipo_propiedad)
                .WithRequired(e => e.componente_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<componente_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<componente_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_sigade>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<componente_sigade>()
                .Property(e => e.codigo_presupuestario)
                .IsUnicode(false);

            modelBuilder.Entity<componente_sigade>()
                .Property(e => e.usuaraio_creo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_sigade>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<componente_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<componente_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_tipo>()
                .HasMany(e => e.componente)
                .WithRequired(e => e.componente_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<componente_tipo>()
                .HasMany(e => e.ctipo_propiedad)
                .WithRequired(e => e.componente_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<componente_usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<componente_usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<componente_usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<cooperante>()
                .Property(e => e.siglas)
                .IsUnicode(false);

            modelBuilder.Entity<cooperante>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<cooperante>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<cooperante>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<cooperante>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<cooperante>()
                .HasMany(e => e.prestamo)
                .WithRequired(e => e.cooperante)
                .HasForeignKey(e => new { e.cooperantecodigo, e.cooperanteejercicio })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ctipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<ctipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<dato_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<dato_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.actividad_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.componente_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.formulario_item_tipo)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.hito_tipo)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.meta)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.producto_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.programa_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.proyecto_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.recurso_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.riesgo_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.subcomponente_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dato_tipo>()
                .HasMany(e => e.subproducto_propiedad)
                .WithRequired(e => e.dato_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<desembolso>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<desembolso>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<desembolso_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<desembolso_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<desembolso_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<desembolso_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<desembolso_tipo>()
                .HasMany(e => e.desembolso)
                .WithRequired(e => e.desembolso_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<documento>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<documento>()
                .Property(e => e.extension)
                .IsUnicode(false);

            modelBuilder.Entity<documento>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<documento>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<ejecucion_estado>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<ejecucion_estado>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<entidad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<entidad>()
                .Property(e => e.abreviatura)
                .IsUnicode(false);

            modelBuilder.Entity<entidad>()
                .HasMany(e => e.proyecto_impacto)
                .WithRequired(e => e.entidad)
                .HasForeignKey(e => new { e.entidadentidad, e.ejercicio })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<entidad>()
                .HasMany(e => e.unidad_ejecutora)
                .WithRequired(e => e.entidad)
                .HasForeignKey(e => new { e.entidadentidad, e.ejercicio })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<estado>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<estado_informe>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<estado_informe>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<estado_informe>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<estado_informe>()
                .HasMany(e => e.informe_presupuesto)
                .WithOptional(e => e.estado_informe)
                .HasForeignKey(e => e.tipo_presupuesto);

            modelBuilder.Entity<estado_tabla>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<estado_tabla>()
                .Property(e => e.tabla)
                .IsUnicode(false);

            modelBuilder.Entity<estado_tabla>()
                .Property(e => e.valores)
                .IsUnicode(false);

            modelBuilder.Entity<etiqueta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<etiqueta>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<etiqueta>()
                .Property(e => e.proyecto)
                .IsUnicode(false);

            modelBuilder.Entity<etiqueta>()
                .Property(e => e.color_principal)
                .IsUnicode(false);

            modelBuilder.Entity<etiqueta>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<etiqueta>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<etiqueta>()
                .HasMany(e => e.proyecto1)
                .WithRequired(e => e.etiqueta)
                .HasForeignKey(e => e.proyecto_clase)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<formulario>()
                .Property(e => e.codigo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<formulario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario>()
                .HasMany(e => e.formulario_item)
                .WithRequired(e => e.formulario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<formulario>()
                .HasMany(e => e.objeto_formulario)
                .WithRequired(e => e.formulario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<formulario_item>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item>()
                .HasMany(e => e.formulario_item_valor)
                .WithRequired(e => e.formulario_item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<formulario_item>()
                .HasMany(e => e.formulario_item_opcion)
                .WithRequired(e => e.formulario_item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<formulario_item_opcion>()
                .Property(e => e.etiqueta)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_opcion>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_opcion>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_tipo>()
                .Property(e => e.usuario_actualizacion)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_tipo>()
                .HasMany(e => e.formulario_item)
                .WithRequired(e => e.formulario_item_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<formulario_item_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_item_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_tipo>()
                .Property(e => e.usario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<formulario_tipo>()
                .HasMany(e => e.formulario)
                .WithRequired(e => e.formulario_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<hito>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<hito>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<hito>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<hito>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<hito>()
                .HasMany(e => e.hito_resultado)
                .WithRequired(e => e.hito)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<hito_resultado>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<hito_resultado>()
                .Property(e => e.comentario)
                .IsUnicode(false);

            modelBuilder.Entity<hito_resultado>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<hito_resultado>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<hito_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<hito_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<hito_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<hito_tipo>()
                .Property(e => e.usuario_actualizacion)
                .IsUnicode(false);

            modelBuilder.Entity<hito_tipo>()
                .HasMany(e => e.hito)
                .WithRequired(e => e.hito_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<informe_presupuesto>()
                .Property(e => e.objeto_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<informe_presupuesto>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<informe_presupuesto>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<interes_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<interes_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<linea_base>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<linea_base>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<linea_base>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<meta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<meta>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<meta>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<meta>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<meta>()
                .Property(e => e.meta_final_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta>()
                .HasMany(e => e.meta_avance)
                .WithRequired(e => e.meta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<meta>()
                .HasMany(e => e.meta_planificado)
                .WithRequired(e => e.meta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<meta_avance>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<meta_avance>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.enero_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.febrero_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.marzo_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.abril_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.mayo_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.junio_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.julio_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.agosto_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.septiembre_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.octubre_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.noviembre_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.diciembre_string)
                .IsUnicode(false);

            modelBuilder.Entity<meta_planificado>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<meta_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<meta_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<meta_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<meta_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<meta_unidad_medida>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<meta_unidad_medida>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<meta_unidad_medida>()
                .Property(e => e.simbolo)
                .IsUnicode(false);

            modelBuilder.Entity<meta_unidad_medida>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<meta_unidad_medida>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<meta_unidad_medida>()
                .HasMany(e => e.meta)
                .WithRequired(e => e.meta_unidad_medida)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<objeto_formulario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<objeto_formulario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<objeto_formulario>()
                .HasMany(e => e.formulario_item_valor)
                .WithRequired(e => e.objeto_formulario)
                .HasForeignKey(e => new { e.objeto_formularioformularioid, e.objeto_formularioobjeto_tipoid, e.objeto_formularioobjeto_id })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<objeto_prestamo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<objeto_recurso>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<objeto_recurso>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<objeto_recurso>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<objeto_riesgo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<objeto_riesgo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<pago_planificado>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<pago_planificado>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<pep_detalle>()
                .Property(e => e.observaciones)
                .IsUnicode(false);

            modelBuilder.Entity<pep_detalle>()
                .Property(e => e.alertivos)
                .IsUnicode(false);

            modelBuilder.Entity<pep_detalle>()
                .Property(e => e.elaborado)
                .IsUnicode(false);

            modelBuilder.Entity<pep_detalle>()
                .Property(e => e.aprobado)
                .IsUnicode(false);

            modelBuilder.Entity<pep_detalle>()
                .Property(e => e.autoridad)
                .IsUnicode(false);

            modelBuilder.Entity<pep_detalle>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<pep_detalle>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<permiso>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<permiso>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<permiso>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<permiso>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<permiso>()
                .HasMany(e => e.rol_permiso)
                .WithRequired(e => e.permiso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<permiso>()
                .HasMany(e => e.usuario_permiso)
                .WithRequired(e => e.permiso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<plan_adquisicion>()
                .Property(e => e.unidad_medida)
                .IsUnicode(false);

            modelBuilder.Entity<plan_adquisicion>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<plan_adquisicion>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<plan_adquisicion>()
                .Property(e => e.numero_contrato)
                .IsUnicode(false);

            modelBuilder.Entity<plan_adquisicion>()
                .HasMany(e => e.plan_adquisicion_pago)
                .WithRequired(e => e.plan_adquisicion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<plan_adquisicion_pago>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<plan_adquisicion_pago>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<plan_adquisicion_pago>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.numero_prestamo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.destino)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.sector_economico)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.numero_autorizacion)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.proyecto_programa)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.objetivo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .Property(e => e.objetivo_especifico)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo>()
                .HasMany(e => e.objeto_prestamo)
                .WithRequired(e => e.prestamo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<prestamo>()
                .HasMany(e => e.prestamo_usuario)
                .WithRequired(e => e.prestamo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<prestamo>()
                .HasMany(e => e.prestamo_tipo_prestamo)
                .WithRequired(e => e.prestamo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<prestamo_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_tipo>()
                .HasMany(e => e.prestamo_tipo_prestamo)
                .WithRequired(e => e.prestamo_tipo)
                .HasForeignKey(e => e.tipoPrestamoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<prestamo_tipo_prestamo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_tipo_prestamo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<prestamo_usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<prodtipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<prodtipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.latitud)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.longitud)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.duracion_dimension)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .Property(e => e.treePath)
                .IsUnicode(false);

            modelBuilder.Entity<producto>()
                .HasMany(e => e.producto_propiedad_valor)
                .WithRequired(e => e.producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<producto>()
                .HasMany(e => e.producto_usuario)
                .WithRequired(e => e.producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<producto>()
                .HasMany(e => e.subproducto)
                .WithRequired(e => e.producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<producto_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<producto_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<producto_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<producto_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<producto_propiedad>()
                .HasMany(e => e.prodtipo_propiedad)
                .WithRequired(e => e.producto_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<producto_propiedad>()
                .HasMany(e => e.producto_propiedad_valor)
                .WithRequired(e => e.producto_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<producto_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<producto_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<producto_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<producto_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<producto_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<producto_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<producto_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<producto_tipo>()
                .HasMany(e => e.prodtipo_propiedad)
                .WithRequired(e => e.producto_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<producto_tipo>()
                .HasMany(e => e.producto)
                .WithRequired(e => e.producto_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<producto_usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<producto_usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<producto_usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<programa>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<programa>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<programa>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<programa>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<programa>()
                .HasMany(e => e.programa_proyecto)
                .WithRequired(e => e.programa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<programa>()
                .HasMany(e => e.programa_propiedad_valor)
                .WithRequired(e => e.programa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<programa_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<programa_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<programa_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_propiedad>()
                .HasMany(e => e.programa_propiedad_valor)
                .WithRequired(e => e.programa_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<programa_propiedad>()
                .HasMany(e => e.progtipo_propiedad)
                .WithRequired(e => e.programa_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<programa_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<programa_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_proyecto>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_proyecto>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<programa_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<programa_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<programa_tipo>()
                .HasMany(e => e.programa)
                .WithRequired(e => e.programa_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<programa_tipo>()
                .HasMany(e => e.progtipo_propiedad)
                .WithRequired(e => e.programa_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<progtipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<progtipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.latitud)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.longitud)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.objetivo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.enunciado_alcance)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.objetivo_especifico)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.vision_general)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.duracion_dimension)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.treePath)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .Property(e => e.observaciones)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.componente)
                .WithRequired(e => e.proyecto1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.desembolso)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.hito)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.linea_base)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasOptional(e => e.pep_detalle)
                .WithRequired(e => e.proyecto);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.programa_proyecto)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.proyecto_rol_colaborador)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.proyecto_impacto)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.proyecto_miembro)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.proyecto_propiedad_valor)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto>()
                .HasMany(e => e.proyecto_usuario)
                .WithRequired(e => e.proyecto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto_impacto>()
                .Property(e => e.impacto)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_impacto>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_impacto>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_miembro>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_miembro>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_propiedad>()
                .HasMany(e => e.proyecto_propiedad_valor)
                .WithRequired(e => e.proyecto_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto_propiedad>()
                .HasMany(e => e.ptipo_propiedad)
                .WithRequired(e => e.proyecto_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_rol_colaborador>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_rol_colaborador>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_tipo>()
                .Property(e => e.usario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_tipo>()
                .HasMany(e => e.proyecto)
                .WithRequired(e => e.proyecto_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto_tipo>()
                .HasMany(e => e.ptipo_propiedad)
                .WithRequired(e => e.proyecto_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proyecto_usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<proyecto_usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<ptipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<ptipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<rectipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<rectipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<recurso>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<recurso>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso>()
                .HasMany(e => e.objeto_recurso)
                .WithRequired(e => e.recurso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<recurso_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_propiedad>()
                .HasMany(e => e.rectipo_propiedad)
                .WithRequired(e => e.recurso_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<recurso_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_tipo>()
                .Property(e => e.usuario_actualizacion)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_tipo>()
                .HasMany(e => e.rectipo_propiedad)
                .WithRequired(e => e.recurso_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<recurso_tipo>()
                .HasMany(e => e.recurso)
                .WithRequired(e => e.recurso_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<recurso_unidad_medida>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_unidad_medida>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_unidad_medida>()
                .Property(e => e.simbolo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_unidad_medida>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_unidad_medida>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<recurso_unidad_medida>()
                .HasMany(e => e.recurso)
                .WithRequired(e => e.recurso_unidad_medida)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.gatillo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.consecuencia)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.solucion)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.riesgos_segundarios)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.resultado)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.observaciones)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo>()
                .HasMany(e => e.objeto_riesgo)
                .WithRequired(e => e.riesgo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<riesgo>()
                .HasMany(e => e.riesgo_propiedad_valor)
                .WithRequired(e => e.riesgo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<riesgo_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_propiedad>()
                .HasMany(e => e.riesgo_propiedad_valor)
                .WithRequired(e => e.riesgo_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<riesgo_propiedad>()
                .HasMany(e => e.rtipo_propiedad)
                .WithRequired(e => e.riesgo_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<riesgo_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<riesgo_tipo>()
                .HasMany(e => e.riesgo)
                .WithRequired(e => e.riesgo_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<riesgo_tipo>()
                .HasMany(e => e.rtipo_propiedad)
                .WithRequired(e => e.riesgo_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<rol>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<rol>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<rol>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<rol>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<rol>()
                .HasMany(e => e.rol_permiso)
                .WithRequired(e => e.rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<rol_permiso>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<rol_permiso>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<rol_unidad_ejecutora>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<rol_unidad_ejecutora>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<rol_unidad_ejecutora>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<rol_unidad_ejecutora>()
                .HasMany(e => e.proyecto_rol_colaborador)
                .WithRequired(e => e.rol_unidad_ejecutora)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<rol_usuario_proyecto>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<rtipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<rtipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<sctipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<sctipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.latitud)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.longitud)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.duracion_dimension)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .Property(e => e.treePath)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente>()
                .HasMany(e => e.subcomponente_usuario)
                .WithRequired(e => e.subcomponente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subcomponente>()
                .HasMany(e => e.subcomponente_propiedad_valor)
                .WithRequired(e => e.subcomponente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subcomponente_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_propiedad>()
                .HasMany(e => e.sctipo_propiedad)
                .WithRequired(e => e.subcomponente_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subcomponente_propiedad>()
                .HasMany(e => e.subcomponente_propiedad_valor)
                .WithRequired(e => e.subcomponente_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subcomponente_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_tipo>()
                .HasMany(e => e.sctipo_propiedad)
                .WithRequired(e => e.subcomponente_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subcomponente_tipo>()
                .HasMany(e => e.subcomponente)
                .WithRequired(e => e.subcomponente_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subcomponente_usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subcomponente_usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subprodtipo_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subprodtipo_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.latitud)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.longitud)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.duracion_dimension)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .Property(e => e.treePath)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto>()
                .HasMany(e => e.subproducto_usuario)
                .WithRequired(e => e.subproducto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subproducto>()
                .HasMany(e => e.subproducto_propiedad_valor)
                .WithRequired(e => e.subproducto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subproducto_propiedad>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_propiedad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_propiedad>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_propiedad>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_propiedad>()
                .HasMany(e => e.subprodtipo_propiedad)
                .WithRequired(e => e.subproducto_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subproducto_propiedad>()
                .HasMany(e => e.subproducto_propiedad_valor)
                .WithRequired(e => e.subproducto_propiedad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subproducto_propiedad_valor>()
                .Property(e => e.valor_string)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_propiedad_valor>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_propiedad_valor>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_tipo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_tipo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_tipo>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_tipo>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_tipo>()
                .HasMany(e => e.subprodtipo_propiedad)
                .WithRequired(e => e.subproducto_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subproducto_tipo>()
                .HasMany(e => e.subproducto)
                .WithRequired(e => e.subproducto_tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subproducto_usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<subproducto_usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<tipo_adquisicion>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<tipo_adquisicion>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<tipo_adquisicion>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<tipo_adquisicion>()
                .HasMany(e => e.plan_adquisicion)
                .WithRequired(e => e.tipo_adquisicion1)
                .HasForeignKey(e => e.tipo_adquisicion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tipo_moneda>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<tipo_moneda>()
                .Property(e => e.simbolo)
                .IsUnicode(false);

            modelBuilder.Entity<tipo_moneda>()
                .HasMany(e => e.desembolso)
                .WithRequired(e => e.tipo_moneda)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tipo_moneda>()
                .HasMany(e => e.prestamo)
                .WithRequired(e => e.tipo_moneda)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<unidad_ejecutora>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<unidad_ejecutora>()
                .HasMany(e => e.colaborador)
                .WithRequired(e => e.unidad_ejecutora)
                .HasForeignKey(e => new { e.unidad_ejecutoraunidad_ejecutora, e.entidad, e.ejercicio })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<unidad_ejecutora>()
                .HasMany(e => e.componente)
                .WithOptional(e => e.unidad_ejecutora)
                .HasForeignKey(e => new { e.unidad_ejecutoraunidad_ejecutora, e.entidad, e.ejercicio });

            modelBuilder.Entity<unidad_ejecutora>()
                .HasMany(e => e.prestamo)
                .WithOptional(e => e.unidad_ejecutora)
                .HasForeignKey(e => new { e.unidad_ejecutoraunidad_ejecutora, e.entidad, e.ejercicio });

            modelBuilder.Entity<unidad_ejecutora>()
                .HasMany(e => e.producto)
                .WithOptional(e => e.unidad_ejecutora)
                .HasForeignKey(e => new { e.unidad_ejecutoraunidad_ejecutora, e.entidad, e.ejercicio });

            modelBuilder.Entity<unidad_ejecutora>()
                .HasMany(e => e.proyecto)
                .WithOptional(e => e.unidad_ejecutora)
                .HasForeignKey(e => new { e.unidad_ejecutoraunidad_ejecutora, e.entidad, e.ejercicio });

            modelBuilder.Entity<unidad_ejecutora>()
                .HasMany(e => e.subcomponente)
                .WithOptional(e => e.unidad_ejecutora)
                .HasForeignKey(e => new { e.unidad_ejecutoraunidad_ejecutora, e.entidad, e.ejercicio });

            modelBuilder.Entity<unidad_ejecutora>()
                .HasMany(e => e.subproducto)
                .WithOptional(e => e.unidad_ejecutora)
                .HasForeignKey(e => new { e.unidad_ejecutoraunidad_ejecutora, e.entidad, e.ejercicio });

            modelBuilder.Entity<unidad_medida>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<unidad_medida>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<unidad_medida>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<unidad_medida>()
                .Property(e => e.usuario_actualizacion)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.usuario1)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.salt)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .HasMany(e => e.colaborador)
                .WithOptional(e => e.usuario)
                .HasForeignKey(e => e.usuariousuario);

            modelBuilder.Entity<usuario>()
                .HasMany(e => e.componente_usuario)
                .WithRequired(e => e.usuario1)
                .HasForeignKey(e => e.usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usuario>()
                .HasMany(e => e.prestamo_usuario)
                .WithRequired(e => e.usuario1)
                .HasForeignKey(e => e.usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usuario>()
                .HasMany(e => e.producto_usuario)
                .WithRequired(e => e.usuario1)
                .HasForeignKey(e => e.usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usuario>()
                .HasMany(e => e.proyecto_usuario)
                .WithRequired(e => e.usuario1)
                .HasForeignKey(e => e.usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usuario>()
                .HasMany(e => e.subproducto_usuario)
                .WithRequired(e => e.usuario1)
                .HasForeignKey(e => e.usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usuario>()
                .HasMany(e => e.usuario_permiso)
                .WithRequired(e => e.usuario)
                .HasForeignKey(e => e.usuariousuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usuario_log>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<usuario_permiso>()
                .Property(e => e.usuariousuario)
                .IsUnicode(false);

            modelBuilder.Entity<usuario_permiso>()
                .Property(e => e.usuario_creo)
                .IsUnicode(false);

            modelBuilder.Entity<usuario_permiso>()
                .Property(e => e.usuario_actualizo)
                .IsUnicode(false);
        }
    }
}
