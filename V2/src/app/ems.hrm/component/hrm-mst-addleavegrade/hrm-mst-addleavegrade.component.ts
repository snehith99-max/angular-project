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
import { NgxSpinnerService } from 'ngx-spinner';

export class Ileavetype {
  leavegradecode_list:any;
  leavegrade_gid: any;
  leavegrade_code:any;
  leavegrade_code_manual:any;
  leavegrade_name:any;
  Code_Generation:any;
  total_leavecount:any;
  available_leavecount:any;
  leave_limit:any;
 

}
@Component({
  selector: 'app-hrm-mst-addleavegrade',
  templateUrl: './hrm-mst-addleavegrade.component.html',
})

export class HrmMstAddleavegradeComponent {
  leavegradecode_list: any[] = [];
  reactiveFormadd!: FormGroup;
  responsedata:any
  selection = new SelectionModel<Ileavetype>(true, []);
  pick:Array<any> = [];
  CurObj: Ileavetype = new Ileavetype();
  leavegrade_gid: any;
  Total:any;
  showInputField: boolean | undefined;
  Code_Generation: any;
 
  available:any;
  limitper_month:any;

  constructor(private SocketService: SocketService,private NgxSpinnerService: NgxSpinnerService,private route:Router,private router:ActivatedRoute,public service: SocketService,private ToastrService: ToastrService,private FormBuilder: FormBuilder) {
    this.reactiveFormadd = new FormGroup({
   
      leavegrade_name :new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      Total :new FormControl('',[Validators.required]),
      available :new FormControl('',[Validators.required]),
      limitper_month:new FormControl('',[Validators.required]),
      leavegrade_code_auto: new FormControl(''),
      leavegrade_code_manual: new FormControl('', [Validators.required,]),
      Code_Generation: new FormControl('Y'),
  });
}

ngOnInit(): void {
  var url = 'LeaveGrade/Getleavegradecodesummary'
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.leavegradecode_list = this.responsedata.leavegradecode_list;
  });
}
// submit(){
//   this.NgxSpinnerService.show();
//   var url = 'LeaveGrade/LeaveGradeSubmit';
//   this.SocketService.post(url, this.reactiveFormadd.value).subscribe((result:any) => {
//     if(result.status == true){
//       this.NgxSpinnerService.hide();
//       this.ToastrService.success(result.message);
//       this.reactiveFormadd.reset();
//     }
//     else {
          
//       this.ToastrService.warning(result.message);
//       this.NgxSpinnerService.hide();
//       this.reactiveFormadd.reset();

      
//     }
//     this.ngOnInit();
//   })
    
      

// }

submit(){
  debugger;
    this.pick = this.selection.selected  
    this.CurObj.leavegradecode_list = this.pick
    this.CurObj.leavegrade_code_manual = this.reactiveFormadd.value.leavegrade_code_manual
    this.CurObj. leavegrade_name = this.reactiveFormadd.value.leavegrade_name
    this.CurObj. Code_Generation = this.reactiveFormadd.value.Code_Generation
    this.CurObj. total_leavecount = this.reactiveFormadd.value.Total
    this.CurObj. available_leavecount = this.reactiveFormadd.value.available
    this.CurObj. leave_limit = this.reactiveFormadd.value.limitper_month
      if (this.CurObj.leavegradecode_list.length === 0) {
      this.ToastrService.warning("Select Atleast one Employee to assign");
      return;
    } 

    debugger
 
      var url = 'LeaveGrade/LeaveGradeSubmit';  
      this.service.post(url, this.CurObj).subscribe((result: any) => {
        if (result.status === false) {
          this.ToastrService.warning(result.message);
          
        } else {
          this.ToastrService.success(result.message);
          this.route.navigate(['/hrm/HrmMstLeaveGrade'])
          
        }
      });
    
   
    this.selection.clear();

}
// submit1(){
//   debugger;
//   const selectedData = this.selection.selected; // Get the selected items
//   if (selectedData.length === 0) {
//     this.ToastrService.warning("Select Atleast one leave grade to Added");
//     return;
//   } 
  
//   for (const data of selectedData) {
//     this.leavegradecode_list.push(data);
// }

//   debugger;
//   var params={ 
//     leavegradecode_list : this.leavegradecode_list,
//     leavegrade_code : this.reactiveFormadd.value.leavegrade_code,
//     leavegrade_name : this.reactiveFormadd.value.leavegrade_name,
//     total_leavecount : this.reactiveFormadd.value.Total,
//     available_leavecount : this.reactiveFormadd.value.available,
//     leave_limit : this.reactiveFormadd.value.limitper_month,
     
//   }
//   console.log(params)
  
//   var url = 'LeaveGrade/LeaveGradeSubmit'
//   this.NgxSpinnerService.show();
//     this.service.postparams(url,params).subscribe((result: any) => {
//       this.NgxSpinnerService.hide();
//       if (result.status == false) {
//         this.ToastrService.warning(result.message)
//         this.route.navigate(['/hrm/HrmMstLeaveGrade']);

//      }
//      else{
//       this.ToastrService.success(result.message)
//       this.route.navigate(['/hrm/HrmMstLeaveGrade']);
//      }

//     });

// }

//Add popup validtion//
get leavegrade_name() {
  return this.reactiveFormadd.get('leavegrade_name')!;
   }
   get leavegrade_code_manual() {
    return this.reactiveFormadd.get('leavegrade_code_manual')!;
     }

isAllSelected() {
  const numSelected = this.selection.selected.length;
  const numRows = this.leavegradecode_list.length;
  return numSelected === numRows;
}
masterToggle() {
  this.isAllSelected() ?
    this.selection.clear() :
    this.leavegradecode_list.forEach((row: Ileavetype) => this.selection.select(row));
}
toggleInputField() {
  this.showInputField = this.Code_Generation === 'N'; 

}
} 




