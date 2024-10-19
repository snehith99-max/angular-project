import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-ims-trn-issuematerial',
  templateUrl: './ims-trn-issuematerial.component.html',
  styleUrls: ['./ims-trn-issuematerial.component.scss']
})
export class ImsTrnIssuematerialComponent {
  materialrequisition_gid:any;
  responsedata: any;
  issueequest_list: any[] = []
  issuerequestform:any;
  productrequestlist:any[]=[]
  temptable: any[] = [];
  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private route: Router, private ToastrService: ToastrService,private router: ActivatedRoute, public service: SocketService) {
   
  }

  ngOnInit(): void {
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
      this.issuerequestform.get("approver_remarks")?.setValue(this.issueequest_list[0].approver_remarks);
      this.issuerequestform.get("priority")?.setValue(this.issueequest_list[0].priority);
      console.log(this.issueequest_list);
    });

    var url = 'ImsTrnMaterialIndent/GetproductMIrequest'
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      this.productrequestlist = this.responsedata.productrequest_list;
      console.log(this.productrequestlist)
    });
    this.NgxSpinnerService.hide();
  }
  get approver_remarks() {
    return this.issuerequestform.get('approver_remarks')!;
  }
  submit(){
    debugger
    var params = {    
      productrequest_list: this.productrequestlist,
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
        window.scrollTo({
    top: 0, 
  });
    }
  });
  }

}
