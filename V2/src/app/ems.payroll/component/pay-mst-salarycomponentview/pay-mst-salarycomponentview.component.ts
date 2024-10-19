import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-pay-mst-salarycomponentview',
  templateUrl: './pay-mst-salarycomponentview.component.html',
  styleUrls: ['./pay-mst-salarycomponentview.component.scss']
})
export class PayMstSalarycomponentviewComponent {
  salarycomponent_gid: any;
  salarycomponent: any;
  responsedata: any;
  ViewSalaryComponentSummary_list:any [] = [];


  constructor(private formBuilder: FormBuilder,private ToastrService: ToastrService, route:Router,private router:ActivatedRoute,public service :SocketService) {
    
   }

   ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    
    flatpickr('.date-picker', options);

    const salarycomponent_gid =this.router.snapshot.paramMap.get('salarycomponent_gid');
    this.salarycomponent = salarycomponent_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salarycomponent,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.getViewSalaryComponentSummary(deencryptedParam);
    this.salarycomponent_gid=deencryptedParam;
   }

   getViewSalaryComponentSummary(salarycomponent_gid: any) {
    var url='PayMstSalaryComponent/getViewSalaryComponentSummary'
    let param = {
      salarycomponent_gid : salarycomponent_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.ViewSalaryComponentSummary_list = result.getViewSalaryComponentSummary;   
    });
}

  

}
