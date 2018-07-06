import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';

@Component({
  selector: 'button-view',
  template: `
    <div class="col-sm-12" align="center"><mat-icon matTooltip="Descargar" matTooltipPosition="above" style="cursor: pointer; color: #d9534f;" (click)="onClick($event)">file_download</mat-icon></div>
  `,
})
export class ButtonDownloadComponent implements ViewCell, OnInit {
  renderValue: string;

  @Input() value: string | number;
  @Input() rowData: any;

  @Output() actionEmitter: EventEmitter<Event> = new EventEmitter();

  ngOnInit() {
    this.renderValue = this.value.toString().toUpperCase();
  }

  onClick() {
    this.actionEmitter.emit(this.rowData);
  }
}