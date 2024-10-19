import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';


export class IEmployeeassign {
  GetPfEmployee_list: string[] = [];
}

@Component({
  selector: 'app-pay-trn-pfemployeeassign',
  templateUrl: './pay-trn-pfemployeeassign.component.html',
  styleUrls: ['./pay-trn-pfemployeeassign.component.scss']
})
export class PayTrnPfemployeeassignComponent {

  PfManagement!: IEmployeeassign;
  employee_assign: any[] = [];
  selection = new SelectionModel<IEmployeeassign>(true, []);
  CurObj: IEmployeeassign = new IEmployeeassign();
  pick: Array<any> = [];
  reactiveFormadd: FormGroup | any;
  responsedata: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService, public service: SocketService) {
    this.PfManagement = {} as IEmployeeassign;
  }


  ngOnInit(): void {

    var url = 'PayTrnPfManagement/GetPfEmployeeSummary';
    this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.employee_assign = this.responsedata.GetPfEmployee_list;
      setTimeout(() => {
        $('#employee_assign').DataTable();
        }, );
  });

  }
  submit() { 
debugger
    this.pick = this.selection.selected;
    this.CurObj.GetPfEmployee_list = this.pick;
    if ( this.CurObj.GetPfEmployee_list.length === 0) {
      this.ToastrService.warning("Select Atleast one Shift Assign to Added");
      return;
    } 
   
    var url = 'PayTrnPfManagement/EmployeeAssignSubmit'
    this.NgxSpinnerService.show();
      this.service.post(url,this.CurObj).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.router.navigate(['/payroll/PayTrnPfManagement']);
       }
       else{
        this.ToastrService.success(result.message)
        this.router.navigate(['/payroll/PayTrnPfManagement']);
       }  
      });

  }


  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.employee_assign.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.employee_assign.forEach((row: IEmployeeassign) => this.selection.select(row));
  }

}
