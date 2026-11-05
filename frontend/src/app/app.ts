import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, map, of, Subject, switchMap } from 'rxjs';
import { Receta, RecetasApi } from './recetas-api';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
})
export class App {
  private readonly api = inject(RecetasApi);
  private readonly consultas = new Subject<{ texto: string; categoria: string; maxMinutos: string }>();

  readonly recetas = signal<Receta[]>([]);
  readonly estado = signal<'cargando' | 'listo' | 'error'>('cargando');
  readonly seleccionada = signal<Receta | null>(null);

  constructor() {
    this.consultas.pipe(
      // Una búsqueda nueva sustituye cualquier petición anterior.
      switchMap(({ texto, categoria, maxMinutos }) => this.api.buscar(texto, categoria, maxMinutos).pipe(
        map(recetas => ({ recetas, error: false })),
        catchError(() => of({ recetas: [], error: true })),
      )),
      takeUntilDestroyed(),
    ).subscribe(({ recetas, error }) => {
      this.recetas.set(recetas);
      this.estado.set(error ? 'error' : 'listo');
    });

    this.buscar('', '', '');
  }

  buscar(texto: string, categoria: string, maxMinutos: string) {
    this.estado.set('cargando');
    this.consultas.next({ texto, categoria, maxMinutos });
  }

  abrir(receta: Receta, dialogo: HTMLDialogElement) {
    this.seleccionada.set(receta);
    dialogo.showModal();
  }
}
