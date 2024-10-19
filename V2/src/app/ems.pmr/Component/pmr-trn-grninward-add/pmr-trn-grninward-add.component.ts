import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {

 
}

@Component({
  selector: 'app-pmr-trn-grninward-add',
  templateUrl: './pmr-trn-grninward-add.component.html',
  styleUrls: ['./pmr-trn-grninward-add.component.scss']
})
export class PmrTrnGrninwardAddComponent {
  grn_list: any[] = [];
  employee: IEmployee;
  reactiveFormReset: any;
  reactiveFormUpdateUserCode: any;
  responsedata: any;
  employee_list: any;

  constructor(public service :SocketService,private route:Router,public NgxSpinnerService:NgxSpinnerService) {
    this.employee = {} as IEmployee;
  }
 
  ngOnInit(): void {
    this.reactiveFormReset = new FormGroup({
    });
    this.reactiveFormUpdateUserCode = new FormGroup({

    });
   this.GetGrninwardSummary();
  } 
  GetGrninwardSummary(){
  var api='PmrTrnGrn/GetGrninwardSummary'
  this.NgxSpinnerService.show()
    
  this.service.get(api).subscribe((result:any)=>{
 
    this.responsedata=result;
    this.grn_list = this.responsedata.Getgrn_lists;  
   console.log(this.grn_list)
    setTimeout(()=>{   
      $('#grn_list').DataTable();
    }, 1);
  this.NgxSpinnerService.hide()
});
}
Onclickgrn(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/pmr/PmrTrnGrninwardsubmit',encryptedParam]) 
}
}
