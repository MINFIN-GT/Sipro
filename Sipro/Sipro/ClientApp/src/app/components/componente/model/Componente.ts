export class Componente{
    id: number;
    nombre : string;
    descripcion : string = null;
    usuarioCreo : string;
    usuarioActualizo : string;
    fechaCreacion : string;
    fechaActualizacion : string;
    componenteTipoid: number;
    componentetiponombre : string;
    estado: number;
    snip: number = null;
    programa: number = null;
    subprograma: number = null;
    proyecto: number = null;
    obra: number = null;
    actividad: number = null;
    renglon: number = null;
    ubicacionGeografica: number = null;
    duracion: number;
    duracionDimension : string;
    fechaInicio : Date;
    fechaFin : string;
    ueunidadEjecutora: number;
    ejercicio: number;
    entidad: number;
    unidadejecutoranombre : string;
    entidadnombre : string;
    latitud : string;
    longitud : string;
    costo: number;
    acumulacionCostoid: number = 3;
    acumulacionCostoNombre : string;
    fuentePrestamo: number = null;
    fuenteDonacion: number = null;
    fuenteNacional: number = null;
    tieneHijos: boolean;
    esDeSigade: number = null;
    prestamoId: number;
    fechaInicioReal : string = null;
    fechaFinReal : string = null;
    inversionNueva: number = 0;
    proyectoid: number;
    orden: number;
    treepath: string;
    nivel: number;
    componenteSigadeid: number;

    pagosPlanificados: string;
    camposDinamicos: string;
}