


import { Component } from '@angular/core';
import { FormBuilder,FormGroup,FormControl,Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-smr-trn-renewalsalesorderselect',
  templateUrl: './smr-trn-renewalsalesorderselect.component.html',
  styleUrls: ['./smr-trn-renewalsalesorderselect.component.scss']
})
export class SmrTrnRenewalsalesorderselectComponent {
  mdlBranchName:any;
 
  renewalorderaddform : FormGroup | any;
  branch_list : any[]=[];
  salesorder_list:any[]=[];
  responsedata: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: ActivatedRoute,private router:Router) 
  {    
  this.renewalorderaddform = new FormGroup({
    branch_name: new FormControl('', Validators.required),
    branch_gid: new FormControl(''),
  });
    
}

  
    ngOnInit(): void {

       var api = 'SmrTrnRenewalsummary/GetRenewalsalesorderselect';
       this.service.get(api).subscribe((result: any) => {
        this.salesorder_list = result.GetSalesOrder_list;
       setTimeout(() => {
        $('#salesorder_list').DataTable();
      }, 1);

    });

    }
    

  
 

  selectAllChecked = false;

selectAll(event: any) {
  this.selectAllChecked = event.target.checked;
  this.salesorder_list.forEach(data => {
    data.selected = this.selectAllChecked;
  });
}

onRowSelect(data: any) {
  data.selected = !data.selected;
  this.selectAllChecked = this.salesorder_list.every(item => item.selected);
}

getSelectedGids(): number[] {
  return this.salesorder_list.filter(data => data.selected).map(data => data.salesorder_gid);
}


selectrenewal(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/smr/SmrTrnRenewaladd',encryptedParam]) 
}
}