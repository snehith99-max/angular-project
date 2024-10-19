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
  selector: 'app-ims-trn-materialindent-issue',
  templateUrl: './ims-trn-materialindent-issue.component.html',
  styleUrls: ['./ims-trn-materialindent-issue.component.scss']
})
export class ImsTrnMaterialindentIssueComponent {
  materialrequisition_gid:any;
  responsedata: any;
  issueequest_list: any[] = []
  issuerequestform:any;
  productrequestlist:any[]=[]
  temptable: any[] = [];
  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private route: Router, private ToastrService: ToastrService,private router: ActivatedRoute, public service: SocketService) {
   
  }

  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options)
    debugger
    const materialrequisition_gid = this.router.snapshot.paramMap.get('materialrequisition_gid');
    this.materialrequisition_gid = materialrequisition_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.materialrequisition_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam+"materialrequisition_gid");
    this.Getissuerequest(deencryptedParam);
    this.issuerequestform = new FormGroup({
      approver_remarks:new FormControl(''),
      priority: new FormControl(''),
      materialrequisition_gid:new FormControl(''),
      materialrequisition_date:new FormControl(''),
      expected_date:new FormControl(''),
      department_name:new FormControl(''),
      user_firstname:new FormControl(''),
      branch_name:new FormControl(''),
      materialrequisition_remarks:new FormControl(''),
    })
  }
  Getissuerequest(materialrequisition_gid:any){
    debugger
    var url = 'ImsTrnMaterialIndent/GetMIrequest';
    this.NgxSpinnerService.show();
    this.materialrequisition_gid = materialrequisition_gid;
    var params = {
      materialrequisition_gid: materialrequisition_gid
    };
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      debugger;
      this.issueequest_list = this.responsedata.issueequest_list;
      console.log(this.issueequest_list);
    });

    var url = 'ImsTrnMaterialIndent/GetproductMIrequest'
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      this.productrequestlist = this.responsedata.productrequest_list;

      this.issuerequestform.get("materialrequisition_gid")?.setValue(this.issueequest_list[0].materialrequisition_gid);
      this.issuerequestform.get("materialrequisition_date")?.setValue(this.issueequest_list[0].materialrequisition_date);
      this.issuerequestform.get("expected_date")?.setValue(this.issueequest_list[0].expected_date);
      this.issuerequestform.get("priority")?.setValue(this.issueequest_list[0].priority);
      this.issuerequestform.get("user_firstname")?.setValue(this.issueequest_list[0].user_firstname);
      this.issuerequestform.get("department_name")?.setValue(this.issueequest_list[0].department_name);
      this.issuerequestform.get("branch_name")?.setValue(this.issueequest_list[0].branch_name);
      this.issuerequestform.get("materialrequisition_remarks")?.setValue(this.issueequest_list[0].materialrequisition_remarks);
      console.log(this.productrequestlist)
    });
    this.NgxSpinnerService.hide();
  }
  get approver_remarks() {
    return this.issuerequestform.get('approver_remarks')!;
  }

  get materialrequisition_remarks() {
    return this.issuerequestform.get('materialrequisition_remarks')!;
  }
  get materialrequisition_gid1() {
    return this.issuerequestform.get('materialrequisition_gid')!;
  }
  get materialrequisition_date() {
    return this.issuerequestform.get('materialrequisition_date')!;
  }
  get expected_date() {
    return this.issuerequestform.get('expected_date')!;
  }

  get department_name() {
    return this.issuerequestform.get('department_name')!;
  }

  get user_firstname() {
    return this.issuerequestform.get('user_firstname')!;
  }


  submit(){
    debugger
    if(this.productrequestlist[0].issuerequestqty > 0 && this.productrequestlist[0].issuerequestqty<=this.productrequestlist[0].req_qty){
    var params = {    
      productrequest_list: this.productrequestlist,
      remarks:this.issuerequestform.value.approver_remarks
    };
    var api = 'ImsTrnMaterialIndent/MIrequest';
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
      this.route.navigate(['/ims/ImsTrnMaterialindent']);
        window.scrollTo({
    top: 0, 
  });
  }
  });
  }
  else{
    this.ToastrService.warning("Issued Quantity should be greater than 0 and less than or equal to Requested Quantity.")
  }
}

}
