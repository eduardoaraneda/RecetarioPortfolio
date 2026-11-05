import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

export interface Receta {
  id: string;
  nombre: string;
  categoria: string;
  minutos: number;
  porciones: number;
  icono: string;
  descripcion: string;
  ingredientes: string[];
  pasos: string[];
}

@Injectable({ providedIn: 'root' })
export class RecetasApi {
  private readonly http = inject(HttpClient);

  buscar(texto: string, categoria: string, maxMinutos: string) {
    // HttpParams devuelve una nueva instancia en cada llamada a set.
    let params = new HttpParams().set('q', texto.trim()).set('categoria', categoria);
    if (maxMinutos) {
      params = params.set('maxMinutos', maxMinutos);
    }

    return this.http.get<Receta[]>('/api/recetas', { params });
  }
}
