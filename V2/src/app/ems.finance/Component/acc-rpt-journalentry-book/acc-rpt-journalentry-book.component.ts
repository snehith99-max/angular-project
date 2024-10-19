import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-acc-rpt-journalentry-book',
  templateUrl: './acc-rpt-journalentry-book.component.html',
  styleUrls: ['./acc-rpt-journalentry-book.component.scss']
})
export class AccRptJournalentryBookComponent {

  GetJournalEntrybook_list: any[]=[];
  filter_list: any[]=[];
  total_list: number=0;
  totalDebit : any;
  totalCredit : any;
  currentPage: number = 1;
  itemsPerPage: number = 200;
  sortBy: string = '';
  sortOrder: boolean = true; 
  itemsPerPageOptions: number[] = [200, 500,1000, 1500];
  noResultsMessage:any;

  constructor(private service: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    private ToastrService : ToastrService
  ){}

  ngOnInit() : void {
    this.getsummary();
  }
  getsummary() {
    this.NgxSpinnerService.show();
    const summaryapi = 'AccTrnBankbooksummary/JournalEntryBookReport';

    
    this.service.get(summaryapi).subscribe({
        next: (result: any) => {
            this.GetJournalEntrybook_list = result.GetJournalEntrybook_list || [];
            this.filter_list = this.GetJournalEntrybook_list;
            this.total_list = this.GetJournalEntrybook_list.length;

            if (this.GetJournalEntrybook_list.length > 0) {
                
                const creditAmounts = this.GetJournalEntrybook_list.map(data => parseFloat((data.credit_amount || '0').replace(',', '')));
                const debitAmounts = this.GetJournalEntrybook_list.map(data => parseFloat((data.debit_amount || '0').replace(',', '')));

                this.totalCredit = creditAmounts.reduce((total, amount) => total + amount, 0);
                this.totalDebit = debitAmounts.reduce((total, amount) => total + amount, 0);

              
                const formatter = new Intl.NumberFormat('en-US', {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });

                this.totalCredit = formatter.format(this.totalCredit);
                this.totalDebit = formatter.format(this.totalDebit);
            } else {
                this.totalCredit = '0.00';
                this.totalDebit = '0.00';
            }

            this.NgxSpinnerService.hide();
        },
        error: (err) => {
            this.totalCredit = '0.00';
            this.totalDebit = '0.00';
            this.NgxSpinnerService.hide();
        }
    });
}

  // getsummary(){
  //   debugger;
  //   this.NgxSpinnerService.show();
  //   var summaryapi = 'AccTrnBankbooksummary/JournalEntryBookReport';
  //   this.service.get(summaryapi).subscribe((result : any) => {      
  //     this.GetJournalEntrybook_list = result.GetJournalEntrybook_list; 
  //     this.filter_list =    this.GetJournalEntrybook_list ;
  //     this.total_list =    this.GetJournalEntrybook_list.length ;
  //     this.NgxSpinnerService.hide();
      
  //     if (this.GetJournalEntrybook_list && Array.isArray(this.GetJournalEntrybook_list)) {
      
  //       this.totalCredit = this.GetJournalEntrybook_list.reduce((total: number, data: any) => {
  //         const creditAmount = parseFloat((data.credit_amount || '0').replace(',', ''));
  //         return total + creditAmount;
  //       }, 0);
      
  //       this.totalDebit = this.GetJournalEntrybook_list.reduce((total: number, data: any) => {
  //         const debitAmount = parseFloat((data.debit_amount || '0').replace(',', ''));
  //         return total + debitAmount;
  //       }, 0);
      
       
  //       const formatter = new Intl.NumberFormat('en-US', {
  //         minimumFractionDigits: 2,
  //         maximumFractionDigits: 2
  //       });
      
  //       this.totalCredit = formatter.format(this.totalCredit);
  //       this.totalDebit = formatter.format(this.totalDebit);
  //     } else {
        
  //       this.totalCredit = '0.00';
  //       this.totalDebit = '0.00';
  //     }
      
  //   });
  // }
  get page_list(): any[] {
    const startIndex =(this.currentPage -1 ) * this.itemsPerPage
    const endIndex = startIndex + this.itemsPerPage;
    return this.filter_list.slice(startIndex, endIndex);
  }
  sort(field: string) {
    if (this.sortBy === field) {
      this.sortOrder = !this.sortOrder; // Toggle sort order
    } else {
      this.sortBy = field;
      this.sortOrder = true; // Default to ascending
    }
    this.filter_list.sort((a: any, b: any) => {
      if (a[field] < b[field]) {
        return this.sortOrder ? -1 : 1;
      } else if (a[field] > b[field]) {
        return this.sortOrder ? 1 : -1;
      } else {
        return 0;
      }
    });
  }
  get startIndex(): number {
    return (this.currentPage - 1) * this.itemsPerPage;
  }
  
  get endIndex(): number {
    return Math.min(this.startIndex + this.itemsPerPage, this.total_list);
  }
  onItemsPerPageChange(): void {
    this.currentPage = 1;
  }
  search(event: Event) {
    const input = event.target as HTMLInputElement;
    const query = input?.value.trim().toLowerCase(); // Trim whitespace and convert to lowercase
    if (!query) {
      this.filter_list = this.GetJournalEntrybook_list;
    } else {
      this.filter_list = this.GetJournalEntrybook_list.filter((item: any) => {
        // Convert all properties to lowercase before comparing
        const account_name = item.account_name.toLowerCase();
        const account_subgroup = item.account_subgroup.toLowerCase();
        const accountgroup_name = item.accountgroup_name.toLowerCase();
        const account_code = item.source_name.toLowerCase();
  
        // Filter items based on the main list properties
        const matchesMainList = 
        account_name.includes(query) ||
        account_subgroup.includes(query) ||
        accountgroup_name.includes(query) ||
        account_code.includes(query);
  

        return matchesMainList;
      });
    }
    this.total_list = this.filter_list.length; 
    this.currentPage = 1; 
    if (this.total_list === 0) {
      
      this.noResultsMessage = "No matching records found";
    } else {
      this.noResultsMessage = "";
    }
  }
  pageChanged(event: any): void {
    this.currentPage = event.page;
  }
}
