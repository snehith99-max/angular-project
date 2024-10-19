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

export class Ileavereport {
  leavereport_list: string[] = [];
  employee_gid:any;
  
}


@Component({
  selector: 'app-pay-trn-payrunleavereport',
  templateUrl: './pay-trn-payrunleavereport.component.html',
  styleUrls: ['./pay-trn-payrunleavereport.component.scss']
})


export class PayTrnPayrunleavereportComponent {

  leavereportForm!: FormGroup;
  leavereport_list : any[] = [];
  selection = new SelectionModel<Ileavereport>(true, []);
  monthyear: any;
  month:any;
  year:any;
  LeaveGeneratingFor:any
  date: any;
  responsedata: any;


  constructor(    
    private renderer: Renderer2,private el: ElementRef,public service: SocketService,private ToastrService: ToastrService,private route: Router,private router: ActivatedRoute,private formBuilder: FormBuilder,public NgxSpinnerService:NgxSpinnerService,
  ){ }


  ngOnInit(): void {

    
    debugger;
    const monthyear = this.router.snapshot.paramMap.get('monthyear');
    this.monthyear = monthyear;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.monthyear, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    const [month, year] = deencryptedParam.split('+');
    this.month = month;
    this.year = year;
    this.LeaveGeneratingFor=month+' '+year;
  
    this.Getleavereport(month, year);

    this.leavereportForm = new FormGroup({
      holidaycount: new FormControl(''),
      leavecount: new FormControl(''),
      absent: new FormControl(''),
      late: new FormControl(''),
      weekoff_days: new FormControl(''),
      actual_lop: new FormControl(''),
      adjusted_lop: new FormControl(''),
      permission: new FormControl(''),
      salary_days: new FormControl(''),
      lop: new FormControl(''),
      total_days: new FormControl(''),
      // leavereport_list:  this.formBuilder.array([]),
    });

       const currentDate = new Date();
       this.date = currentDate.toDateString();
  }
  Getleavereport(month: any,year: any) {     
    var url = 'PayTrnSalaryManagement/GetLeaveReport';
    let param = {
      month: month,
      year: year,
    };
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.leavereport_list = result.leavereport_list;
      this.NgxSpinnerService.hide(); 
      setTimeout(() => {
        $('#leavereport_list').DataTable();
      },1);
    
    });
  }


onKeyPress(event: any) {
    // Get the pressed key
    const key = event.key;
  
    if (!/^[0-9.]$/.test(key)) {
        // If not a number or dot, prevent the default action (key input)
        event.preventDefault();
    }
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.leavereport_list.forEach((row: Ileavereport) => this.selection.select(row));
  }


  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.leavereport_list.length;
    return numSelected === numRows;
  }

update(){

}

}
