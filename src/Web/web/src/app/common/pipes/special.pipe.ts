import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'special'
})
export class SpecialPipe implements PipeTransform {
  transform(value: string): string {
    return '~' + value.toUpperCase() + '~';
  }
}
