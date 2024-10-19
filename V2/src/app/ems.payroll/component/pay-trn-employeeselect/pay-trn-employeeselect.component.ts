import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';

export class IEmployee {
  employeeselect_list: string[] = [];
  employee_gid:any;
}

@Component({
  selector: 'app-pay-trn-employeeselect',
  templateUrl: './pay-trn-employeeselect.component.html',
  styleUrls: ['./pay-trn-employeeselect.component.scss']
})

export class PayTrnEmployeeselectComponent {
  employeeselect_list: any[] = [];
  select_list: any[] = [];
  detailsdtl_list: any[] = [];
  reactiveForm!: FormGroup;
  selection = new SelectionModel<IEmployee>(true, []);
  monthyear: any;
  month:any;
  year:any;
  working_days: any;

  
  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    
       )
   { }
   ngOnInit(): void {
    debugger;
    const monthyear = this.router.snapshot.paramMap.get('monthyear');
    this.monthyear = monthyear;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.monthyear, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    debugger;
    const [month, year,working_days] = deencryptedParam.split('+');
    this.month = month;
    this.year = year;
    this.working_days = working_days;




    this.Getemployeeselect(month, year);
    this.reactiveForm = this.formBuilder.group({
     
    });
   
    

  }
  Getemployeeselect(month: any,year: any) {
    var url = 'PayTrnSalaryManagement/GetEmployeeSelect';
    let param = {
      month: month,
      year: year,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      this.employeeselect_list = result.GetEmployeeSelect;
    });
  }
  submit() {
    const selectedData = this.selection.selected; // Get the selected items
    if (selectedData.length === 0) {
      this.ToastrService.warning("Select Atleast one Employee to payrun");
      return;
    } 
    

    for (const data of selectedData) {
      this.detailsdtl_list.push(data);
    }
    console.log(this.detailsdtl_list)
    debugger
 
      var url = 'PayTrnSalaryManagement/Postforpayrun';
      const param = {        
        month: this.month, 
        year: this.year,
        detailsdtl_list: this.detailsdtl_list     
      }; 
      
      this.service.post(url, param).subscribe((result: any) => {
        if (result.status === false) {
          this.ToastrService.warning(result.message);
          
        } else {
          this.ToastrService.success(result.message);
          this.route.navigate(['/payroll/PayTrnSalaryManagement'])
          
        }
      });
    
   
    this.selection.clear();
  }
  
   isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.employeeselect_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.employeeselect_list.forEach((row: IEmployee) => this.selection.select(row));
  }
}