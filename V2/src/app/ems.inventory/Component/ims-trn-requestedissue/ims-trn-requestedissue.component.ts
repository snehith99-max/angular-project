import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
@Component({
  selector: 'app-ims-trn-requestedissue',
  templateUrl: './ims-trn-requestedissue.component.html',
  styleUrls: ['./ims-trn-requestedissue.component.scss']
})
export class ImsTrnRequestedissueComponent {
  materialrequisition_gid:any;
  issueequest_list: any[] = []
  issuerequestform:any;
  responsedata: any;
  productrequestlist:any[]=[]



  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private route: Router, private ToastrService: ToastrService,private router: ActivatedRoute, public service: SocketService) {
   
  }
  
  ngOnInit(): void{
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options)
    const materialrequisition_gid = this.router.snapshot.paramMap.get('materialrequisition_gid');
    this.materialrequisition_gid = materialrequisition_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.materialrequisition_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam+"materialrequisition_gid");
    this.Getissuerequest(deencryptedParam);
    this.Getissuerequestproduct(deencryptedParam);
    this.issuerequestform = new FormGroup({
      branch_name:new FormControl(''),
      department_name: new FormControl(''),
      materialrequisition_gid: new FormControl(''),
      materialrequisition_date: new FormControl(''),
      materialrequisition_remarks: new FormControl(''),
      approver_remarks: new FormControl(''),
      expected_date: new FormControl(''),
      priority: new FormControl(''),
      issue_remarks: new FormControl(''),
      IS_date: new FormControl(this.getCurrentDate()),
    })
  }
  Getissuerequest(materialrequisition_gid:any){
    debugger
    this.materialrequisition_gid = materialrequisition_gid;
    var params = {
      materialrequisition_gid: materialrequisition_gid
    };
    var url = 'ImsTrnIssueMaterial/GetMIissuedetails';
    this.NgxSpinnerService.show();
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      debugger;
      this.issueequest_list = this.responsedata.GetMIissuedetails_list;
      this.issuerequestform.get("branch_name")?.setValue(this.issueequest_list[0].branch_name);
      this.issuerequestform.get("department_name")?.setValue(this.issueequest_list[0].department_name);
      this.issuerequestform.get("materialrequisition_gid")?.setValue(this.issueequest_list[0].materialrequisition_gid);
      this.issuerequestform.get("materialrequisition_date")?.setValue(this.issueequest_list[0].materialrequisition_date);
      this.issuerequestform.get("materialrequisition_remarks")?.setValue(this.issueequest_list[0].materialrequisition_remarks);
      this.issuerequestform.get("approver_remarks")?.setValue(this.issueequest_list[0].approver_remarks);
      this.issuerequestform.get("expected_date")?.setValue(this.issueequest_list[0].expected_date);
      this.issuerequestform.get("priority")?.setValue(this.issueequest_list[0].priority);
      console.log(this.issueequest_list);
    });

    // var url = 'ImsTrnMaterialIndent/GetproductMIrequest'
    // this.service.getparams(url,params).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.productrequestlist = this.responsedata.productrequest_list;
    //   console.log(this.productrequestlist)
    // });
    this.NgxSpinnerService.hide();
  }
  Getissuerequestproduct(materialrequisition_gid:any){
    this.NgxSpinnerService.show();
    debugger
    this.materialrequisition_gid = materialrequisition_gid;
    var params = {
      materialrequisition_gid: materialrequisition_gid
    };
    var url = 'ImsTrnIssueMaterial/GetMIissuedetailproduct';
    this.NgxSpinnerService.show();
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      debugger;
      this.productrequestlist = this.responsedata.GetMIissueproduct_list;
      console.log(this.productrequestlist);
    });
    this.NgxSpinnerService.hide();
  }
  submit(){
    debugger
    if(this.productrequestlist[0].stock_quantity!=0){

    if(this.productrequestlist[0].issuerequestqty !=0 && this.productrequestlist[0].issuerequestqty <= this.productrequestlist[0].req_qty )
    {
    var params = {    
      GetMIissueproduct_list: this.productrequestlist,
      issue_remarks: this.issuerequestform.value.issue_remarks,
      materialrequisition_gid: this.issuerequestform.value.materialrequisition_gid,
      department_name: this.issuerequestform.value.department_name,

    }; 
    var api = 'ImsTrnIssueMaterial/Postissue';
    this.NgxSpinnerService.show()
    this.service.postparams(api, params).subscribe((result: any) => {
    if (result.status == false) {
      this.NgxSpinnerService.hide()
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning(result.message)
    }
    else {
      this.NgxSpinnerService.hide()
      this.ToastrService.success(result.message)
      this.route.navigate(['/ims/ImsTrnIssue']);
        window.scrollTo({
    top: 0, 
  });
    }
  });
  }
  else{
    this.ToastrService.warning("Issued Qty is greater than Request Qty")
  }
}
else{
  this.ToastrService.warning("Atleast One value Must Be Added!")
}
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }


}
