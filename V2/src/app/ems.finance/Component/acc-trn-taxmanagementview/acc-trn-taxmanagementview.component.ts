import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-acc-trn-taxmanagementview',
  templateUrl: './acc-trn-taxmanagementview.component.html',
  styleUrls: ['./acc-trn-taxmanagementview.component.scss']
})
export class AccTrnTaxmanagementviewComponent {
  taxfiling_gid:any;
  responsedata: any;
  OutputTaxView_List: any[] = [];
  InputTaxView_List: any[] = [];
  CreditNoteTaxView_List: any[] = [];
  lstaxfiling_gid:any;
  TotalTaxView_List: any[] = [];
  TotalCreditTaxView_List: any[] = [];
  
  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router,
    private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService, private datePipe: DatePipe,) {
  }

  ngOnInit(): void {
    const taxfiling_gid = this.router.snapshot.paramMap.get('taxfiling_gid');
    // console.log(termsconditions_gid)
    this.taxfiling_gid = taxfiling_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.taxfiling_gid, secretKey).toString(enc.Utf8);
    this.lstaxfiling_gid = deencryptedParam;

    this.AccTrnOutputTaxsummary();
    this.AccTrnInputTaxsummary();
    this.AccTrnCreditNoteTaxsummary();
    this.AccTrnTotalTax();
    this.AccTrnCreditTotalTax();
  }
 
  AccTrnOutputTaxsummary() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetOutputTaxView'
    let param = {
      taxfiling_gid : this.lstaxfiling_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#OutputTaxView_List').DataTable().destroy();
      this.responsedata = result;
      this.OutputTaxView_List = result.GetOutputTaxView_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnInputTaxsummary() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetInputTaxView'
    let param = {
      taxfiling_gid : this.lstaxfiling_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#InputTaxView_List').DataTable().destroy();
      this.responsedata = result;
      this.InputTaxView_List = result.GetInputTaxView_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnCreditNoteTaxsummary() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetCreditNoteTaxView'
    let param = {
      taxfiling_gid : this.lstaxfiling_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#CreditNoteTaxView_List').DataTable().destroy();
      this.responsedata = result;
      this.CreditNoteTaxView_List = result.GetCreditNoteTaxView_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnTotalTax(){
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetTotalTaxView'
    let param = {
      taxfiling_gid : this.lstaxfiling_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#TotalTaxView_List').DataTable().destroy();
      this.responsedata = result;
      this.TotalTaxView_List = result.GetTotalTaxView_List;
      this.NgxSpinnerService.hide();
    });
  }

  AccTrnCreditTotalTax() {
    this.NgxSpinnerService.show();
    var url = 'TaxManagements/GetTotalCreditTaxView'
    let param = {
      taxfiling_gid : this.lstaxfiling_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#TotalTaxView_List').DataTable().destroy();
      this.responsedata = result;
      this.TotalCreditTaxView_List = result.GetTotalCreditTaxView_List;
      this.NgxSpinnerService.hide();
    });
  }

  back() {
    this.route.navigate(['/finance/AccTrnTaxManagement']);
  }

  exportExcel_output() :void {
    const OutputTaxViewList = this.OutputTaxView_List.map(item => ({
      'IRN': item.irn || '',
      'Invoice Date': item.invoice_date || '',
      'Invoice Ref.No': item.invoice_refno || '',
      'Customer': item.customer || '',
      'GST_No': item.gst_number || '',
      'SGST 0%': item.SGST_0 || '',
      'SGST_25%': item.SGST_25 || '',
      'SGST_6%': item.SGST_6 || '',
      'SGST_9%': item.SGST_9 || '',
      'SGST_14%': item.SGST_14 || '',
      'CGST_0%': item.CGST_0 || '',
      'CGST_25%': item.CGST_25 || '',
      'CGST_6%': item.CGST_6 || '',
      'CGST_9%': item.CGST_9 || '',
      'CGST_14%': item.CGST_14 || '',
      'IGST_0%': item.IGST_0 || '',
      'IGST_5%': item.IGST_5 || '',
      'IGST_12%': item.IGST_12 || '',
      'IGST_18%': item.IGST_18 || '',
      'IGST_28%': item.IGST_28 || '',
      'Invoice Amount': item.invoiceamount || '',
      'Taxable Amount': item.taxableamount || '',
      'Non_Taxable Amount': item.nontaxable_amount || '',     
     }));     
      // this.excelService.exportAsExcelFile(ProductList , 'Product');
       // Create a new table element
  const table = document.createElement('table');

  // Add header row with background color
  const headerRow = table.insertRow();
  Object.keys(OutputTaxViewList[0]).forEach(header => {
    const cell = headerRow.insertCell();
    cell.textContent = header;
    cell.style.backgroundColor = '#00317a'; 
    cell.style.color = '#FFFFFF';
    cell.style.fontWeight = 'bold';
    cell.style.border = '1px solid #000000';
  });

  // Add data rows
  OutputTaxViewList.forEach(item => {
    const dataRow = table.insertRow();
    Object.values(item).forEach(value => {
      const cell = dataRow.insertCell();
      cell.textContent = value;
      cell.style.border = '1px solid #000000';
    });
  });

  // Convert the table to a data URI
  const tableHtml = table.outerHTML;
  const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

  // Trigger download
  const link = document.createElement('a');
  link.href = dataUri;
  link.download = 'Output Tax.xls';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
  }

  exportExcel_input() :void {
    const InputTaxViewList = this.InputTaxView_List.map(item => ({
      'Invoice Gid': item.invoice_gid || '',
      'Invoice Date': item.invoice_date || '',
      'Invoice Ref.No': item.invoice_refno || '',
      'Vendor': item.vendor || '',
      'GST_No': item.gst_number || '',
      'SGST 0%': item.SGST_0 || '',
      'SGST_25%': item.SGST_25 || '',
      'SGST_6%': item.SGST_6 || '',
      'SGST_9%': item.SGST_9 || '',
      'SGST_14%': item.SGST_14 || '',
      'CGST_0%': item.CGST_0 || '',
      'CGST_25%': item.CGST_25 || '',
      'CGST_6%': item.CGST_6 || '',
      'CGST_9%': item.CGST_9 || '',
      'CGST_14%': item.CGST_14 || '',
      'IGST_0%': item.IGST_0 || '',
      'IGST_5%': item.IGST_5 || '',
      'IGST_12%': item.IGST_12 || '',
      'IGST_18%': item.IGST_18 || '',
      'IGST_28%': item.IGST_28 || '',
      'Invoice Amount': item.invoiceamount || '',
      'Taxable Amount': item.taxableamount || '',
      'Non Taxable Amount': item.nontaxable_amount || '',     
     }));     
      // this.excelService.exportAsExcelFile(ProductList , 'Product');
       // Create a new table element
  const table = document.createElement('table');

  // Add header row with background color
  const headerRow = table.insertRow();
  Object.keys(InputTaxViewList[0]).forEach(header => {
    const cell = headerRow.insertCell();
    cell.textContent = header;
    cell.style.backgroundColor = '#00317a'; 
    cell.style.color = '#FFFFFF';
    cell.style.fontWeight = 'bold';
    cell.style.border = '1px solid #000000';
  });

  // Add data rows
  InputTaxViewList.forEach(item => {
    const dataRow = table.insertRow();
    Object.values(item).forEach(value => {
      const cell = dataRow.insertCell();
      cell.textContent = value;
      cell.style.border = '1px solid #000000';
    });
  });

  // Convert the table to a data URI
  const tableHtml = table.outerHTML;
  const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

  // Trigger download
  const link = document.createElement('a');
  link.href = dataUri;
  link.download = 'Input Tax.xls';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
  }

}
