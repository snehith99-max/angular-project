import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
    selector: 'app-pmr-rpt-vendorreport-view',
    templateUrl: './pmr-rpt-vendorreport-view.component.html',
    styleUrls: ['./pmr-rpt-vendorreport-view.component.scss']
  })
  export class PmrRptVendorreportViewComponent {
  vendor_gid: any;
  CreditDetailedForm! : FormGroup;
  responsedata: any;
  CreditorreportView_List: any[] = [];
  CreditorreportVendorView_List: any[] = [];
  CreditorreportOpening_List: any[] = [];
  totalCredit: any;
  totalDebit: any;
  vendor_name: any;
  vendor_code: any;
  remarks: any;
  closing_amount:any;
  totcrd:any;
  totdeb:any;
  closingamount: any;
  opening_balance: any;
  from_date: any = '';
  to_date: any = '';
  maxDate!:string;
  reactiveform : FormGroup | any;
  OutstandingAmount: string = '';
  PaidAmount: string = '';
  InvoiceAmount: string = '';
  
  Creditorvendor : any[]=[]
  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    const today = new Date();
    this.maxDate = today.toISOString().split('T')[0];
    flatpickr('.date-picker', options);
    
    const vendor_gid = this.router.snapshot.paramMap.get('vendor_gid');
    this.vendor_gid = vendor_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.vendor_gid, secretKey).toString(enc.Utf8);
    this.vendor_gid = deencryptedParam;
    // this.GetAccTrnCreditorReportView(deencryptedParam,this.from_date,this.to_date);
     this.GetVendorName(deencryptedParam)
  

   this.CreditDetailedForm = new FormGroup({
    from_date : new FormControl(''),
    to_date: new FormControl(''),
   });
  }

  GetVendorName(vendor_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'PmrRptVendorLedgerreport/GetVendorReportView'
    let param = {
      vendor_gid: vendor_gid,
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Creditorvendor = result.vendorledger_list;
      this.vendor_code = this.Creditorvendor[0].vendor_code
      this.vendor_name = this.Creditorvendor[0].vendor
      this.NgxSpinnerService.hide();
    });
  
 
    var url = 'PmrRptVendorLedgerreport/GetVendorPaymentReportView'  
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.CreditorreportView_List = result.vendorledger_list;

      const thirtytosixtytotalInvoiceAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const thirtytosixtytotalPaidAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.payment_amount.replace(/,/g, '')), 0));
    const thirtytosixtytotalOutstandingAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.InvoiceAmount = this.formatNumber(thirtytosixtytotalInvoiceAmount);
    this.PaidAmount = this.formatNumber(thirtytosixtytotalPaidAmount);
    this.OutstandingAmount = this.formatNumber(thirtytosixtytotalOutstandingAmount);
      });
  }
  
  formatNumber(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  roundToTwoDecimal(value: number): number {
    return Math.round(value * 100) / 100;
  }


  GetAccTrnCreditorReportView(vendor_gid: any, from_date: any, to_date: any) {
    this.NgxSpinnerService.show();
    var url = 'PmrRptVendorLedgerreport/GetCreditorReportViewdate'
    let param = {
      vendor_gid: vendor_gid,
      from_date : from_date,
      to_date : to_date
      
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.CreditorreportView_List = result.vendorledger_list;

      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#CreditorreportView_List').DataTable();
      }, 1);

      // if (this.CreditorreportView_List != null) {
      //   this.totalCredit = this.CreditorreportView_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
      //   this.totalDebit = this.CreditorreportView_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

      //   // Format the totals with commas and .00
      //   const formatter = new Intl.NumberFormat('en-US', {
      //     minimumFractionDigits: 2,
      //     maximumFractionDigits: 2
      //   });

      //   this.totalCredit = formatter.format(this.totalCredit);
      //   this.totalDebit = formatter.format(this.totalDebit);

  
      //   let totcrd = this.parseValue1(this.totalCredit);
      //   let totdeb = this.parseValue1(this.totalDebit);

      //   this.closing_amount = totdeb - totcrd;

      //   this.closingamount = this.formatValue1(this.closing_amount);
      // }
      const thirtytosixtytotalInvoiceAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const thirtytosixtytotalPaidAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.payment_amount.replace(/,/g, '')), 0));
    const thirtytosixtytotalOutstandingAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.InvoiceAmount = this.formatNumber(thirtytosixtytotalInvoiceAmount);
    this.PaidAmount = this.formatNumber(thirtytosixtytotalPaidAmount);
    this.OutstandingAmount = this.formatNumber(thirtytosixtytotalOutstandingAmount);

    });
  }

  parseValue1(value: any): number {
    // Convert the value to a number, removing any non-numeric characters (e.g., commas)
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }

  formatValue1(value: number): string {
    // Format the number as a string with comma separators and two decimal places
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  popmodal(parameter: string) {
    this.remarks = parameter;
  }

  exportExcel(): void {
    const CreditorreportViewList = this.CreditorreportView_List.map(item => ({
      'Transaction Date': item.transaction_date || '',
      'Reference Number': item.journal_refno || '',
      'Transaction Type': item.transaction_type || '',
      'Customer Name': item.customer_name || '',
      'Remarks': item.remarks || '',
      'Opening Balance': item.openingbalance || '',
      'Debit Amount': item.debit_amount || '',
      'Credit Amount': item.credit_amount || '',
      'Closing Balance': item.closingbalance || '',
    }));
    
    // Create a new table element
    const table = document.createElement('table');

    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(CreditorreportViewList[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Add data rows
    CreditorreportViewList.forEach(item => {
      const dataRow = table.insertRow();
      Object.values(item).forEach(value => {
        const cell = dataRow.insertCell();
        cell.textContent = value;
        cell.style.border = '1px solid #000000';
      });
    });

    const totalsRow1 = table.insertRow();
    const numColumns1 = Object.keys(CreditorreportViewList[0]).length;
  
    for (let i = 0; i < numColumns1 - 4; i++) { 
      const cell = totalsRow1.insertCell();
      cell.textContent = '';
      cell.style.border = '1px solid #000000';
    }
  
    const totalLabelCell = totalsRow1.insertCell();
    totalLabelCell.textContent = 'Total:';
    totalLabelCell.style.fontWeight = 'bold';
    totalLabelCell.style.border = '1px solid #000000';
  
  
    const totalDebitCell = totalsRow1.insertCell();
    totalDebitCell.textContent = this.totalDebit || '0';
    totalDebitCell.style.fontWeight = 'bold';
    totalDebitCell.style.border = '1px solid #000000';
  

    const totalCreditCell = totalsRow1.insertCell();
    totalCreditCell.textContent = this.totalCredit || '0';
    totalCreditCell.style.fontWeight = 'bold';
    totalCreditCell.style.border = '1px solid #000000';

    const emptycell = totalsRow1.insertCell();
    emptycell.textContent = ' ';
    emptycell.style.fontWeight = 'bold';
    emptycell.style.border = '1px solid #000000';

    // Convert the table to a data URI
    const tableHtml = table.outerHTML;
    const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

    // Trigger download
    const link = document.createElement('a');
    link.href = dataUri;
    link.download = 'Creditor Account Statement.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }


  onSearchClick(){
    this.NgxSpinnerService.show();
    
    var url = 'PmrRptVendorLedgerreport/GetCreditorReportViewdate'     
    let params = {
      vendor_gid: this.vendor_gid,
      from_date: this.CreditDetailedForm.value.from_date,
      to_date: this.CreditDetailedForm.value.to_date 
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#CreditorreportView_List').DataTable().destroy();
      this.responsedata = result;
      this.CreditorreportView_List = result.vendorledger_list;
      this.NgxSpinnerService.hide();
      const thirtytosixtytotalInvoiceAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
      const thirtytosixtytotalPaidAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.payment_amount.replace(/,/g, '')), 0));
      const thirtytosixtytotalOutstandingAmount = this.roundToTwoDecimal(this.CreditorreportView_List.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));
  
      this.InvoiceAmount = this.formatNumber(thirtytosixtytotalInvoiceAmount);
      this.PaidAmount = this.formatNumber(thirtytosixtytotalPaidAmount);
      this.OutstandingAmount = this.formatNumber(thirtytosixtytotalOutstandingAmount);


      setTimeout(() => {
        $('#CreditorreportView_List').DataTable();
      }, 1);
    });
}
}
