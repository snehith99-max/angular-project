import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface IExpense {
  expense_data: string;
  expense_code: string;
  expense_gid: string;
  expense_desc:string;
}
 

@Component({
  selector: 'app-sys-mst-expensecategorysummary',
  templateUrl: './sys-mst-expensecategorysummary.component.html',
  styleUrls: ['./sys-mst-expensecategorysummary.component.scss']
})
export class SysMstExpensecategorysummaryComponent {
  reactiveForm!: FormGroup;
  responsedata: any;
  expense!: IExpense;
  expensecategory_list: any[] = [];
  parameterValue: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {
    this.expense = {} as IExpense;
  }
  ngOnInit(): void {
    this.GetExpenseCategorySummary();
    
    this.reactiveForm = new FormGroup({
      expense_data: new FormControl(this.expense.expense_data, [Validators.required,]),
      expense_desc: new FormControl(this.expense.expense_desc, [Validators.required,]),

      expense_gid: new FormControl(''),
 
  });
  }

  GetExpenseCategorySummary(){
  
    var url = 'SysMstExpense/GetExpenseCategorySummary'
    this.service.get(url).subscribe((result: any) => {
     
    this.responsedata = result;
    this.expensecategory_list = this.responsedata.expensecategory_listdata;
    
  
    setTimeout(() => {
    $('#expensecategory_list').DataTable();
      }, 1);
    });
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
        this.service.postparams(url, this.reactiveForm.value).subscribe((result: any) => {

          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.GetExpenseCategorySummary();  
          }
          else {

            this.reactiveForm.get("expense_data")?.setValue(null);
            this.reactiveForm.get("expense_code")?.setValue(null);
            this.reactiveForm.get("expense_desc")?.setValue(null);

            this.ToastrService.success(result.message)
            this.GetExpenseCategorySummary();
           

          }

        });

      }
      else {
        this.ToastrService.warning('result.message')
      }
      setTimeout(function() {
        window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
}


onclose(){

}

openModaldelete(parameter: string){
  this.parameterValue = parameter
}

ondelete(){
  debugger;
  console.log(this.parameterValue);
  var url = 'SysMstExpense/DeleteExpense'
  let param = {
    expense_gid : this.parameterValue 
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    if (result.status == false) {
      this.ToastrService.warning(result.message)
    }
    else {
      this.ToastrService.success(result.message)
    }
    this.GetExpenseCategorySummary();

  });
}

}
