import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'FormatoMillones'})
export class FormatoMillones implements PipeTransform {
    transform(numero: number, millones: boolean): string {
        if(millones){
            var res = ((numero/1000000).toFixed(2));
	        	return ('Q '+ res.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }
        return ('Q '+ (Number(numero).toFixed(2)).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
    }
}

@Pipe({name: 'FormatoMillonesDolares'})
export class FormatoMillonesDolares implements PipeTransform{
    transform(numero: number, millones: boolean): string {
        if(millones){
            var res = ((numero/1000000).toFixed(2));
	        	return ('$ '+ res.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }
        return ('$ '+ (Number(numero).toFixed(2)).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
    }
}

@Pipe({name: 'FormatoMillonesSinTipo'})
export class FormatoMillonesSinTipo implements PipeTransform{
    transform(numero: number, millones: boolean): string {
        if(millones){
            var res = ((numero/1000000).toFixed(2));
	        	return (res.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }
        return ((Number(numero).toFixed(2)).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
    }
}