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

@Component({
  selector: 'app-pay-rpt-pfreport',
  templateUrl: './pay-rpt-pfreport.component.html',
  styleUrls: ['./pay-rpt-pfreport.component.scss']
})
export class PayRptPfreportComponent {
  pf_list : any[] = [];
  reactiveForm!: FormGroup;
  month: any;
  sal_year: any;
  monthsal_year: any;
  responsedata: any;
  Pfassign_type : any[] = [];

  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    
       )
  {}

  ngOnInit(): void { 
    debugger;

    const monthsal_year = this.router.snapshot.paramMap.get('monthsal_year');
    this.monthsal_year = monthsal_year;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.monthsal_year, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    debugger;
    const [month, sal_year] = deencryptedParam.split('+');
    this.month = month;
    this.sal_year = sal_year;
   

    this.Pfassign(month, sal_year);
    this.reactiveForm = this.formBuilder.group({
     
    });


   


  }

  GetPFSummary(month: any,year: any) {
    debugger;
  var url = 'PayTrnRptPFandESIFormat/GetPFSummary'
  let param = {
    month: month,
    year: year,
  };
  this.service.getparams(url, param).subscribe((result: any) => {
   
    this.responsedata = result;
    this.pf_list = this.responsedata.pf_listdata;
    setTimeout(() => {
      $('#pfformat_list').DataTable()
    }, 1); 
    

  });
}

  Pfassign(month: any,sal_year: any) {
    debugger;
    var url = 'PayTrnRptPFandESIFormat/Pfassign';
    let param = {
      month: month,
      sal_year: sal_year,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      this.Pfassign_type = result.Pfassign_type;
    });
  }


}
