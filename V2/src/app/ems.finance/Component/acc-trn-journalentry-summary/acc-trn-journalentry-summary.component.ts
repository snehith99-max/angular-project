import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { debounce } from 'rxjs';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-acc-trn-journalentry-summary',
  templateUrl: './acc-trn-journalentry-summary.component.html',
  styleUrls: ['./acc-trn-journalentry-summary.component.scss']
})
export class AccTrnJournalentrySummaryComponent {
  noResultsMessage:any;
  filteredItems: any[]=[];
  searchQuery: string= '';
  GetJournalTransaction_list: any;
  parameterValue: any;
  loading: boolean = true;
  totalCredit: any;
  totalDebit: any;
  bankmaster_list: any;
  parameterValue1: any;
  remarks: any;
  transaction_type: any;
  accountgroup_list: any;
  responsedata: any;
  formattedTotalCredit: string = '';
  formattedTotalDebit: string = '';
  GetJournalEntry_lists: any;
  currentPage: number = 1; // Current page number
  itemsPerPage: number = 200; // Number of items to display per page
  totalItems: number = 0; 
  itemsPerPageOptions: number[] = [200, 500,1000, 1500];
  sortBy: string = '';
  sortOrder: boolean = true; 
  maxSize: number = 5;
rotate: boolean = true;
ellipses: boolean = true;
showOptionsDivId:any;
  constructor(public service: SocketService,private cdr: ChangeDetectorRef, private NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService) {
  }

  ngOnInit(): void {

    this.getsummary();
  
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

  }
 
  
// Function to track by unique identifier to improve rendering performance
trackByFn(index: number, item: any): any {
  return item.journal_gid || index; // or use a unique property like 'id'
}
getsummary() {
  this.NgxSpinnerService.show();
  const url = 'JournalEntry/GetJournalEntrySummarys';
  this.service.get(url).subscribe(
    (result: any) => {
      this.GetJournalEntry_lists = result.GetJournalEntry_lists;
      this.filteredItems = this.GetJournalEntry_lists;
      this.totalItems = this.GetJournalEntry_lists.length;
      this.NgxSpinnerService.hide();
    },
    (error) => {
      this.NgxSpinnerService.hide();
      console.error('Error fetching data', error);
    }
  );
}
search(event: Event) {
  const input = event.target as HTMLInputElement;
  const query = input?.value.trim(); // Trim whitespace from the query
  if (!query) {
    this.filteredItems = this.GetJournalEntry_lists;
  } else {
    this.filteredItems = this.GetJournalEntry_lists.filter((item: any) => {
      // Filter items based on the main list properties
      const matchesMainList = 
        item.transaction_date.includes(query) ||
        item.journal_refno.includes(query) ||
        item.transaction_type.includes(query);
      
      // Filter items based on the nested list properties
      const matchesNestedList = item.GetJournalTransactions_list.some((data: any) =>
        data.voucher_type.toLowerCase().includes(query.toLowerCase()) ||
        data.remarks.toLowerCase().includes(query.toLowerCase())
      );
      
      // Return true if any of the conditions match
      return matchesMainList || matchesNestedList;
    });
  }
  this.totalItems = this.filteredItems.length; // Update totalItems based on filtered results
  this.currentPage = 1; // Reset currentPage to 1 when performing a new search
  if (this.totalItems === 0) {
    // Display "No matching records found" message if no items match the query
    this.noResultsMessage = "No matching records found";
    
  } else {
    this.noResultsMessage = "";
  }
}


get pagedItems(): any[] {
  const startIndex = (this.currentPage - 1) * this.itemsPerPage;
  const endIndex = startIndex + this.itemsPerPage;
  return this.filteredItems.slice(startIndex, endIndex);
}

sort(field: string) {
  if (this.sortBy === field) {
    this.sortOrder = !this.sortOrder; // Toggle sort order
  } else {
    this.sortBy = field;
    this.sortOrder = true; // Default to ascending
  }
  this.filteredItems.sort((a: any, b: any) => {
    if (a[field] < b[field]) {
      return this.sortOrder ? -1 : 1;
    } else if (a[field] > b[field]) {
      return this.sortOrder ? 1 : -1;
    } else {
      return 0;
    }
  });
}

onItemsPerPageChange(): void {
  this.currentPage = 1; // Reset to the first page when items per page changes
}

pageChanged(event: any): void {
  this.currentPage = event.page;
}


get startIndex(): number {
  return (this.currentPage - 1) * this.itemsPerPage;
}

get endIndex(): number {
  return Math.min(this.startIndex + this.itemsPerPage, this.totalItems);
}


  onadd() {
    this.router.navigate(['/finance/AccTrnJournalentryAdd'])
  }
  popmodal(parameter: string, parameter1: string) {
    this.parameterValue1 = parameter;
    this.remarks = parameter;
    this.transaction_type = parameter1;
  }
  poptransaction(parameter: string, parameter1: string) {
    // console.log(parameter)
    this.parameterValue1 = parameter1;
    const journal_gid = parameter
    this.NgxSpinnerService.show();
    var url = 'JournalEntry/GetJournalEntryTransaction'
    let param = {
      journal_gid: journal_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // this.responsedata=result;
      this.NgxSpinnerService.hide();
      this.GetJournalTransaction_list = result.GetJournalTransaction_list;
      // this.totalCredit = this.GetJournalTransaction_list.reduce((total:any, data:any) => total + parseFloat(data.credit_amount), 2);
      // this.totalDebit = this.GetJournalTransaction_list.reduce((total:any, data:any) => total + parseFloat(data.debit_amount), 2);

      // Sum up credit and debit amounts without formatting
      this.totalCredit = this.GetJournalTransaction_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
      this.totalDebit = this.GetJournalTransaction_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

      // Format the totals with commas and .00
      const formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      });

      this.totalCredit = formatter.format(this.totalCredit);
      this.totalDebit = formatter.format(this.totalDebit);

    });
    // this.parameterValue1 = parameter;
    // this.remarks = parameter;
    // this.transaction_type = parameter1;
  }
  // Custom formatting function
  formatNumber(value: number): string {
    if (value >= 1000 && value < 10000) {
      return (value / 1000).toFixed(2) + 'K';
    } else if (value >= 10000 && value < 1000000) {
      return (value / 1000).toFixed(0) + 'K';
    } else if (value >= 1000000 && value < 10000000) {
      return (value / 1000000).toFixed(2) + 'M';
    } else if (value >= 10000000 && value < 1000000000) {
      return (value / 1000000).toFixed(0) + 'M';
    } else if (value >= 1000000000 && value < 10000000000) {
      return (value / 1000000000).toFixed(2) + 'B';
    } else if (value >= 10000000000) {
      return (value / 1000000000).toFixed(0) + 'B';
    } else {
      return value.toString();
    }
  }
  onedit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/finance/AccTrnJournalEntryEdit', encryptedParam])
  }
  downloaddocument(data: any) {
    if (data.document_path && data.document_path.trim() !== '') {
      const pathParts = data.document_path.split('/');
      const filenameWithExtension = pathParts[pathParts.length - 1];
      const filename = filenameWithExtension.split('.')[0];
  
      let link = document.createElement("a");
      link.download = filename;
      link.href = data.document_path;
  
      // Add event listener to handle download completion or failure
      link.addEventListener('error', (event) => {
        console.error('Error occurred during file download:', event);
        // Handle error (e.g., show error message to the user)
        this.ToastrService.error('Failed to download the file');
      });
  
      // Trigger download
      link.click();
    } else {
      // Scroll to top of the page (optional)
      window.scrollTo({ top: 0 });
  
      // Display warning message when no file path is provided
      this.ToastrService.warning('No File Found');
    }
  }
  
    openModaldelete(parameter: string) {
      this.parameterValue = parameter
    }
    ondelete ()
    {
      this.NgxSpinnerService.show();
      var url = 'JournalEntry/DeleteJounralEntry'
      let param = {
        journal_gid: this.parameterValue
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
        }
        else {
          this.NgxSpinnerService.hide();
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });        
          this.getsummary();
          this.ToastrService.success(result.message)
        }

      });

  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }
  
}
