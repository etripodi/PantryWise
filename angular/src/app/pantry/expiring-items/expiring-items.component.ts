import { Component, inject, Injectable, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LocalizationPipe } from '@abp/ng.core';
import {
  NgbDateParserFormatter,
  NgbDatepickerModule,
  NgbDateStruct,
} from '@ng-bootstrap/ng-bootstrap';
import JsBarcode from 'jsbarcode';
import { PantryDetailService, PantryItemDto } from '../../proxy/pantry';

const pad = (n: number) => String(n).padStart(2, '0');

// Formato fijo dd/MM/yyyy en el input, sin depender de la cultura.
@Injectable()
class DmyDateFormatter extends NgbDateParserFormatter {
  parse(value: string): NgbDateStruct | null {
    const m = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/.exec(value?.trim() ?? '');
    return m ? { day: +m[1], month: +m[2], year: +m[3] } : null;
  }
  format(date: NgbDateStruct | null): string {
    return date ? `${pad(date.day)}/${pad(date.month)}/${date.year}` : '';
  }
}

@Component({
  selector: 'app-expiring-items',
  templateUrl: './expiring-items.component.html',
  imports: [FormsModule, DatePipe, LocalizationPipe, NgbDatepickerModule],
  providers: [{ provide: NgbDateParserFormatter, useClass: DmyDateFormatter }],
})
export class ExpiringItemsComponent implements OnInit {
  // ponytail: la API pantry (genérica) devuelve Product en null; solo pantry-detail lo incluye.
  private pantry = inject(PantryDetailService);

  beforeDate: NgbDateStruct | null = (() => {
    const t = new Date();
    return { year: t.getFullYear(), month: t.getMonth() + 1, day: t.getDate() };
  })();
  items: PantryItemDto[] = [];
  loading = false;

  ngOnInit() {
    this.load();
  }

  load() {
    const d = this.beforeDate;
    if (!d || typeof d !== 'object') return;
    this.loading = true;
    this.pantry.getExpiringWithProduct(`${d.year}-${pad(d.month)}-${pad(d.day)}`).subscribe({
      next: items => {
        this.items = items;
        this.loading = false;
        // espera al render de la tabla para dibujar los <svg class="barcode">
        setTimeout(() => JsBarcode('.barcode').init());
      },
      error: () => (this.loading = false),
    });
  }
}
