import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-acc-trn-sundryexpense',
  templateUrl: './acc-trn-sundryexpense.component.html',
  styleUrls: ['./acc-trn-sundryexpense.component.scss']
})
export class AccTrnSundryexpenseComponent {


  expenseform : FormGroup | any;
  expense : any[]=[];
  responsedata : any;
  parameterValue: any;
  constructor(
    private formBuilder: FormBuilder,
    private route: Router,
    private ToastrService: ToastrService,
    public service: SocketService,
    public NgxSpinnerService:NgxSpinnerService
    ){}
    ngOnInit(): void {
       this.GetProductunitSummary();
    
    }
  GetProductunitSummary() {
    var url = 'AccTrnSundryExpenses/GetSundryExpenseSummary';
    this.service.get(url).subscribe((result: any) => {
      $('#expense').DataTable().destroy();
      this.responsedata = result;
      this.expense = this.responsedata.expense_list;
      setTimeout(() => {
        $('#expense').DataTable();
      }, 1);
    });
  }

  onview(params: any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/finance/AccTrnViewsundryexpenses', encryptedParam])
  }

  onedit(params: any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/finance/AccTrnEditsundryexpenses', encryptedParam])
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }
  Ondelete() {
    var url = 'AccTrnSundryExpenses/SummaryProductDelete'
    this.NgxSpinnerService.show();
    let param = {
      expense_gid : this.parameterValue 
    }
    this.service.post(url,param).subscribe((result: any) => {
      if(result.status ==false){
       this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else{
       this.NgxSpinnerService.hide();
       this.ToastrService.success(result.message)
      }
      this.GetProductunitSummary();
    });
  }
}
