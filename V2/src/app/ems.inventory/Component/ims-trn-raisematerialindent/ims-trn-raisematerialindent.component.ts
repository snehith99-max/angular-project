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
  selector: 'app-ims-trn-raisematerialindent',
  templateUrl: './ims-trn-raisematerialindent.component.html',
  styleUrls: ['./ims-trn-raisematerialindent.component.scss']
})
export class ImsTrnRaisematerialindentComponent {
  materialrequisition_gid:any;
  responsedata: any;
  issueequest_list: any[] = []
  issuerequestform!:FormGroup ;
  productrequestform!:FormGroup;
  productrequestlist:any[]=[]
  temptable: any[] = [];
  qty_req:any;
  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private route: Router, private ToastrService: ToastrService,private router: ActivatedRoute, public service: SocketService) {
   
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options)
    this.issuerequestform = new FormGroup({
      priority_remarks:new FormControl(''),
      priority: new FormControl(''),
      qty_requested: new FormControl(''),
      materialrequisition_date:new FormControl(''),
      materialrequisition_gid:new FormControl(''),
      materialrequisition_remarks:new FormControl(''),
      branch_name:new FormControl(''),
      user_firstname:new FormControl(''),
      department_name:new FormControl(''),
      expected_date : new FormControl(this.getCurrentDate())
    })
    const materialrequisition_gid = this.router.snapshot.paramMap.get('materialrequisition_gid');
    this.materialrequisition_gid = materialrequisition_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.materialrequisition_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam+"materialrequisition_gid");
    this.Getissuerequest(deencryptedParam);
  
  }
  Getissuerequest(materialrequisition_gid:any){
    debugger;
    var url = 'ImsTrnPendingMaterialIssue/GetRaiseMaterialIndent';
    this.NgxSpinnerService.show();
    this.materialrequisition_gid = materialrequisition_gid;
    var params = {
      materialrequisition_gid: materialrequisition_gid
    };
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      this.issueequest_list = result.raisematerialindent_list;
      this.issuerequestform.get("branch_name")?.setValue(this.issueequest_list[0].branch_name);
      this.issuerequestform.get("department_name")?.setValue(this.issueequest_list[0].department_name);
      this.issuerequestform.get("user_firstname")?.setValue(this.issueequest_list[0].user_firstname);
      this.issuerequestform.get("materialrequisition_gid")?.setValue(this.issueequest_list[0].materialrequisition_gid);
      this.issuerequestform.get("priority_remarks")?.setValue(this.issueequest_list[0].priority_remarks);
      this.issuerequestform.get("priority")?.setValue(this.issueequest_list[0].priority);
      this.issuerequestform.get("materialrequisition_date")?.setValue(this.issueequest_list[0].materialrequisition_date);
      this.issuerequestform.get("materialrequisition_remarks")?.setValue(this.issueequest_list[0].materialrequisition_remarks);
      console.log(this.issueequest_list);
    });

    var url = 'ImsTrnPendingMaterialIssue/GetRaiseMaterialIndentProduct'
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      this.productrequestlist = this.responsedata.materialindentproduct_list;
      for (let i = 0; i < this.productrequestlist.length; i++) {
      this.issuerequestform.addControl(`qty_requested_${i}`, new FormControl(this.productrequestlist[i].qty_requested));
      }
    });
    this.NgxSpinnerService.hide();
  }
  get priority_remarks() {
    return this.issuerequestform.get('priority_remarks')!;
  }
  get materialrequisition_date(){
    return this.issuerequestform.get('materialrequisition_date')!;
  }
  get expected_date(){
    return this.issuerequestform.get('expected_date')!;
  }
  get qty_requested(){
    return this.issuerequestform.get('qty_requested')!;
  }

  submit(materialrequisition_gid:any){
    debugger;
    this.materialrequisition_gid = materialrequisition_gid;
    var params = {    
      product_requestlist: this.productrequestlist,
      materialrequisition_gid: materialrequisition_gid,
      priority_remarks: this.issuerequestform.value.priority_remarks,
      priority: this.issuerequestform.value.priority,
      qty_requested: this.issuerequestform.value.qty_requested,
      materialrequisition_date: this.issuerequestform.value.materialrequisition_date,
      expected_date: this.issuerequestform.value.expected_date,
      materialindentproduct_list: this.temptable,
    };
    var api = 'ImsTrnPendingMaterialIssue/PostRaisePRSubmit';
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
        window.scrollTo({
        top: 0, 
      });
      this.ToastrService.success(result.message)
      this.route.navigate(['/ims/ImsTrnPendingMaterialIssue']);
    }
  });
  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
   
    return dd + '-' + mm + '-' + yyyy;
  }

  qytrecived(index: number, qtyOrdered: number, value: any): void {
    debugger
    let inputValue = value ? parseInt(value, 10) : 0;

    if (inputValue > qtyOrdered) {
      this.issuerequestform.addControl(`qty_requested_${index}`, new FormControl(qtyOrdered));
     
    } 

}
}
