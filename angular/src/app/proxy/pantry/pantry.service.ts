import type { PantryItemDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PantryService {
  apiName = 'Default';
  

  getExpiringItems = (beforeDate: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PantryItemDto[]>({
      method: 'GET',
      url: '/api/app/pantry/expiring-items',
      params: { beforeDate },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
