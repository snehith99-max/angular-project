import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { jsPDF, jsPDFOptions } from 'jspdf';
import "jspdf-autotable";
import html2canvas from "html2canvas";
import { DomSanitizer } from '@angular/platform-browser';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { Options } from 'html2canvas';
interface CustomOptions extends Options {
  dpi?: number;
}

@Component({
  selector: 'app-acc-rpt-profileandlossdetails',
  templateUrl: './acc-rpt-profileandlossdetails.component.html',
  styleUrls: ['./acc-rpt-profileandlossdetails.component.scss']
})
export class AccRptProfileandlossdetailsComponent {
  account_gid:any;
  lspage:any;
  responsedata: any;
  GetPlDetails_list: any[] = [];
  account_name: any;
  overal_credit: any;
  overal_debit: any;
  GetPlDetailspdf_list:any;
  accountgidvalue:any;
  lspagevalue:any;
  @ViewChild('contentToConvert1') contentToConvert1!: ElementRef;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {

  }

  ngOnInit(): void {
    const accountgid = this.route.snapshot.paramMap.get('account_gid');
    const secretKey = 'storyboarderp';
    this.accountgidvalue = accountgid;
    const deencryptedParam = AES.decrypt(this.accountgidvalue,secretKey).toString(enc.Utf8);
    this.account_gid =deencryptedParam;
    const ls_page = this.route.snapshot.paramMap.get('lspage');
    this.lspagevalue = ls_page;
    const deencryptedParam1 = AES.decrypt(this.lspagevalue,secretKey).toString(enc.Utf8);
   
    this.lspage = deencryptedParam1;
   // console.log(this.account_gid )
   // console.log(this.lspage )
    let param = {
      account_gid: this.account_gid 
      }
    var url2 = 'ProfitLossReport/GetProfitandlosslDetails'
    this.service.getparams(url2, param).subscribe((result: any) => {

      this.responsedata = result;
      this.GetPlDetails_list = this.responsedata.GetPlDetails_list;
      if (this.GetPlDetails_list !=null)
      {
        this.account_name =this.GetPlDetails_list[0].account_name;
        // this.overal_debit =this.GetPlDetails_list[0].overal_debit;
        // this.overal_credit =this.GetPlDetails_list[0].overal_credit;
        const totalCredit = this.GetPlDetails_list.reduce((sum, item) => {
          return sum + this.parseCurrency(item.credit_amount);
        }, 0);
    
        const totalDebit = this.GetPlDetails_list.reduce((sum, item) => {
          return sum + this.parseCurrency(item.debit_amount);
        }, 0);
    
        this.overal_credit = this.formatCurrency(totalCredit);
        this.overal_debit = this.formatCurrency(totalDebit);
      }
    
    //this.bankmaster_list.reverse();
      //console.log(this.GetPlDetails_list)
      setTimeout(() => {
        $('#GetPlDetails_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 200, // Number of rows to display per page
            "lengthMenu": [200, 500, 1000, 1500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  parseCurrency(value: string): number {
    // Remove any non-numeric characters except for the decimal point
    return parseFloat(value.replace(/[^0-9.-]+/g,""));
  }

  // formatCurrency(amount: number): string {
  //   return amount.toLocaleString('en-IN', { currency: 'INR' });
  // }
  formatCurrency(amount: number): string {
    // Replace commas with an empty string to remove them
    const amountWithoutCommas = amount.toString().replace(/,/g, '');
    // Use Intl.NumberFormat to format the number
    const formatter = new Intl.NumberFormat('en-IN', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    // Format the number without commas
    const formattedNumber = formatter.format(parseFloat(amountWithoutCommas));
    // Split the formatted number into integer and fraction parts
    const parts = formattedNumber.split('.');
    // Add commas back to the integer part
    const integerPartWithCommas = parts[0].replace(/\B(?=(\d{2})+(?!\d))/g, ',');
    // Join the integer part with the fraction part and return
    return integerPartWithCommas + '.' + parts[1];
}


  
 // Helper function to format date from dd-mm-yyyy to dd/mm/yyyy
formatDate(dateStr: string): string {
  if (!dateStr) return ''; // Handle empty or undefined date string

  // Split the input date string by '-'
  const dateParts = dateStr.split('-');

  // Ensure there are exactly three parts (day, month, year)
  if (dateParts.length !== 3) {
    return ''; // Invalid date format, return empty string
  }

  const [day, month, year] = dateParts;

  // Construct the formatted date string in dd/mm/yyyy format
  const formattedDate = `${day}/${month}/${year}`;

  return formattedDate;
}

exportExcel(): void {
  const GetPlDetailslist = this.GetPlDetails_list.map(item => ({
    'Transaction Date': item.transaction_date || '',
    'Reference Number': item.journal_refno || '',
    'Remarks': item.remarks || '',
    'Debit Amount': item.debit_amount || '',
    'Credit Amount': item.credit_amount || '',   
  }));
  
  // Create a new table element
  const table = document.createElement('table');

  // Add account name row
  const accountNameRow = table.insertRow();
  const accountNameCell = accountNameRow.insertCell();
  accountNameCell.textContent = this.account_name || 'Account Name';
  accountNameCell.colSpan = Object.keys(GetPlDetailslist[0]).length;
  accountNameCell.style.textAlign = 'center';
  accountNameCell.style.fontWeight = 'bold';
  accountNameCell.style.border = '1px solid #000000';

  // Add header row with background color
  const headerRow = table.insertRow();
  Object.keys(GetPlDetailslist[0]).forEach(header => {
    const cell = headerRow.insertCell();
    cell.textContent = header;
    cell.style.backgroundColor = '#00317a';
    cell.style.color = '#FFFFFF';
    cell.style.fontWeight = 'bold';
    cell.style.border = '1px solid #000000';
  });

  // Add data rows
  GetPlDetailslist.forEach(item => {
    const dataRow = table.insertRow();
    Object.values(item).forEach(value => {
      const cell = dataRow.insertCell();
      cell.textContent = value;
      cell.style.border = '1px solid #000000';
    });
  });

  // Add totals row
  const totalsRow1 = table.insertRow();
  const numColumns1 = Object.keys(GetPlDetailslist[0]).length;

  for (let i = 0; i < numColumns1 - 3; i++) { 
    const cell = totalsRow1.insertCell();
    cell.textContent = '';
    cell.style.border = '1px solid #000000';
  }

  const totalLabelCell = totalsRow1.insertCell();
  totalLabelCell.textContent = 'Total:';
  totalLabelCell.style.fontWeight = 'bold';
  totalLabelCell.style.border = '1px solid #000000';

  const totalDebitCell = totalsRow1.insertCell();
  totalDebitCell.textContent = this.overal_debit || '0';
  totalDebitCell.style.fontWeight = 'bold';
  totalDebitCell.style.border = '1px solid #000000';

  const totalCreditCell = totalsRow1.insertCell();
  totalCreditCell.textContent = this.overal_credit || '0';
  totalCreditCell.style.fontWeight = 'bold';
  totalCreditCell.style.border = '1px solid #000000';

  // Convert the table to a data URI
  const tableHtml = table.outerHTML;
  const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

  // Trigger download
  const link = document.createElement('a');
  link.href = dataUri;
  link.download = 'Account Statement.xls';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}


  onback()
  {
    if(this.lspage == "PL")
    {
      this.router.navigate(['/finance/AccRptProfitandLost'])
    }
    else if(this.lspage == "OB")
    {
      this.router.navigate(['/finance/AccRptOpenBalanceReport'])
    }
    else if(this.lspage == "TB")
    {
      this.router.navigate(['/finance/AccRptTrailBalance'])
    }
    else if(this.lspage == "BS")
    {
      this.router.navigate(['/finance/AccRptBalanceSheeet'])
    }
  }
}
