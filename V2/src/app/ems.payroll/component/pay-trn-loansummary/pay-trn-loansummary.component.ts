import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface ILoan {
}

@Component({
  selector: 'app-pay-trn-loansummary',
  templateUrl: './pay-trn-loansummary.component.html',
  styleUrls: ['./pay-trn-loansummary.component.scss'],
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})
export class PayTrnLoansummaryComponent {
  showOptionsDivId: any;
  responsedata: any;
  
  loan_list: any[] = [];
  loan!: ILoan;
  
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
  this.loan = {} as ILoan;
  }

  ngOnInit(): void {
  
   //// Summary Grid//////
      
      var url = 'PayTrnLoanSummary/GetLoanSummary'
      this.service.get(url).subscribe((result: any) => {
  
        this.responsedata = result;
        this.loan_list = this.responsedata.loanlist;
        setTimeout(() => {
          $('#loan_list').DataTable();
        }, );
  
  
      });
    }

    addloan() {
      this.router.navigate(['/payroll/PayTrnLoanadd'])
    }
    
     openModalview(params:any) {
      debugger;
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/payroll/PayTrnLoanview',encryptedParam])
    }

     openModaledit(params:any) {
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/payroll/PayTrnLoanedit',encryptedParam]) 
    }

    
  }

  

