import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

interface IExpense {
  expense_data: string;
  expense_code: string;
  expense_gid: string;
  expense_desc:string;
}
 

@Component({
  selector: 'app-otl-mst-expensecategorysummary',
  templateUrl: './otl-mst-expensecategorysummary.component.html',
  styleUrls: ['./otl-mst-expensecategorysummary.component.scss']
})
export class OtlMstExpensecategorysummaryComponent {

  reactiveForm!: FormGroup;
  responsedata: any;
  expense!: IExpense;
  expensecategory_list: any[] = [];
  parameterValue: any;
  showOptionsDivId: any;
  rows: any[] = [];

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService) {
    this.expense = {} as IExpense;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetExpenseCategorySummary();
  
    this.reactiveForm = new FormGroup({
      expense_data: new FormControl(this.expense.expense_data, [Validators.required,
    ]),
      expense_desc: new FormControl(this.expense.expense_desc, [Validators.required,
    ]),
      expense_gid: new FormControl(''),
  });
  }

  GetExpenseCategorySummary(){
  debugger
    var url = 'SysMstExpense/GetExpenseCategorySummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
     
    this.responsedata = result;
    this.expensecategory_list = this.responsedata.expensecategory_listdata;
    
  
    setTimeout(() => {
    $('#expensecategory_list').DataTable();
      }, 1);
    });
    this.NgxSpinnerService.hide()
    }

    
get expense_data() {
  return this.reactiveForm.get('expense_data')!;
}
get expense_code() {
  return this.reactiveForm.get('expense_code')!;
}
get expense_desc() {
  return this.reactiveForm.get('expense_desc')!;
}

public onsubmit(): void {
  debugger;

  if (this.reactiveForm.value.expense_data != null && this.reactiveForm.value.expense_data != '')
    {

        this.reactiveForm.value;
        var url = 'SysMstExpense/PostExpense'
        this.NgxSpinnerService.show()
        this.service.postparams(url, this.reactiveForm.value).subscribe((result: any) => {

          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.GetExpenseCategorySummary();  
            this.reactiveForm.reset();
            this.NgxSpinnerService.hide()
          }
          else {

            this.reactiveForm.get("expense_data")?.setValue(null);
            this.reactiveForm.get("expense_code")?.setValue(null);
            this.reactiveForm.get("expense_desc")?.setValue(null);

            this.ToastrService.success(result.message)
            this.GetExpenseCategorySummary();
            this.reactiveForm.reset();
            this.NgxSpinnerService.hide()
           

          }

        });

      }
      else {
        this.ToastrService.warning('result.message')
      }
}


onclose(){
  this.reactiveForm.reset();

}

openModaldelete(parameter: string){
  this.parameterValue = parameter
}
toggleOptions(account_gid: any) {
  if (this.showOptionsDivId === account_gid) {
    this.showOptionsDivId = null;
  } else {
    this.showOptionsDivId = account_gid;
  }
}

ondelete(){
  debugger;
  console.log(this.parameterValue);
  var url = 'SysMstExpense/DeleteExpense'
  this.NgxSpinnerService.show()
  let param = {
    expense_gid : this.parameterValue 
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    if (result.status == false) {
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide()
    }
    else {
      this.ToastrService.success(result.message)
    }
    this.GetExpenseCategorySummary();
    this.NgxSpinnerService.hide()

  });
}

}
