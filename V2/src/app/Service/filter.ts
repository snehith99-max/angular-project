import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(items: any[], searchText: string, productname: string): any[] {
    if (!items) return [];
    if (!searchText && !productname) return items;

    return items.filter(item => {
      const matchesSearchText = !searchText || Object.keys(item).some(key => {
        return String(item[key]).toLowerCase().includes(searchText.toLowerCase());
      });

      const matchesProductName = !productname || item.product_name.toLowerCase().includes(productname.toLowerCase());

      return matchesSearchText && matchesProductName;
    });
  }
}
