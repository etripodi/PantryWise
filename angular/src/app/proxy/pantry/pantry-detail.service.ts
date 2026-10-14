import type { PantryItemDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PantryDetailService {
  apiName = 'Default';
  

  getExpiringWithProduct = (beforeDate: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PantryItemDto[]>({
      method: 'GET',
      url: '/api/app/pantry-detail/expiring-with-product',
      params: { beforeDate },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
