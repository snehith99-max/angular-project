import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-smr-trn-salesorderapproval',
  templateUrl: './smr-trn-salesorderapproval.component.html',
  styleUrls: ['./smr-trn-salesorderapproval.component.scss']
})
export class SmrTrnSalesorderapprovalComponent {
  approvalsalesordersummary_list: any[] = [];
  approvalsalesorderdetail_list: any[] = [];
  salesordergid: any;
  response_data: any;
  deencryptedParam:any;

  constructor(private formBuilder: FormBuilder,
    public NgxSpinnerService: NgxSpinnerService,
    private route: Router,
    private router: ActivatedRoute,
    private ToastrService: ToastrService,
    public service: SocketService) { }

  ngOnInit(): void {
    debugger
    const salesorder_gid = this.router.snapshot.paramMap.get('salesorder_gid1');

    this.salesordergid = salesorder_gid;

    const secretKey = 'storyboard';
   this.deencryptedParam = AES.decrypt(this.salesordergid, secretKey).toString(enc.Utf8);

    this.GetapprovalSalesOrderSummary(this.deencryptedParam);
    this.Getapprovalsalesorderdetails(this.deencryptedParam);
  }
  GetapprovalSalesOrderSummary(salesorder_gid: any) {
    debugger
    let param = { salesorder_gid: salesorder_gid }
    var summaryapi = 'SmrTrnSalesorder/GetApprovalSalesOrderSummary';
    this.service.getparams(summaryapi, param).subscribe((apiresponse: any) => {
      this.approvalsalesordersummary_list = apiresponse.postsalesorder_list
    });
  }
  Getapprovalsalesorderdetails(salesorder_gid : any){
    var url='SmrTrnSalesorder/Getapprovalsalesorderdetails'
    this.NgxSpinnerService.show()
    let param = {
      salesorder_gid : salesorder_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.response_data=result;
    this.approvalsalesorderdetail_list = result.Getapprovalsalesorder_list;   
    this.NgxSpinnerService.hide()
    });
  }
  onSubmit(){
    var approvalapi = 'SmrTrnSalesorder/Approvalsalesorder';
    this.NgxSpinnerService.show()
    let param = {
        salesorder_gid: this.deencryptedParam       
    }
    this.service.getparams(approvalapi, param).subscribe((result: any) =>{
    if(result == false){
      this.ToastrService.warning(result.message);
    }
    else{
      this.ToastrService.success(result.message);
      this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
    }
    });
    this.NgxSpinnerService.hide()
  }
}
