import type { EntityDto } from '@abp/ng.core';

export interface PantryItemDto extends EntityDto<string> {
  userId?: string;
  productId?: string;
  product: ProductDto;
  expirationDate?: string;
}

export interface ProductDto extends EntityDto<string> {
  barCode?: string;
  name?: string;
}
