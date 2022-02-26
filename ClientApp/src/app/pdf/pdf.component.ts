import { Component, Input, OnInit } from '@angular/core';
import { aggregateBy } from '@progress/kendo-data-query';
import { InvoiceRow } from './incoice-row';
import { invoiceData } from "./invoice-data";

@Component({
  selector: 'app-pdf',
  templateUrl: './pdf.component.html',
  styleUrls: ['./pdf.component.css']
})
export class PdfComponent {
  @Input()
  public data: InvoiceRow[] = invoiceData;

  constructor() { }

  public rightAlign = {
    "text-align": "right",
  };

  private aggregates: any[] = [
    {
      field: "qty",
      aggregate: "sum",
    },
    {
      field: "total",
      aggregate: "sum",
    },
  ];

  public get totals(): any {
    return aggregateBy(this.data, this.aggregates) || {};
  }

}