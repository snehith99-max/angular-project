import { Component } from '@angular/core';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-ims-trn-miapprovalreview',
  templateUrl: './ims-trn-miapprovalreview.component.html',
  styleUrls: ['./ims-trn-miapprovalreview.component.scss']
})
export class ImsTrnMIapprovalreviewComponent {


    config: AngularEditorConfig = {
      editable: false,
      spellcheck: false,
      height: '25rem',
      minHeight: '5rem',
      width: '1000px',
      placeholder: 'Enter text here...',
      translate: 'no',
      defaultParagraphSeparator: 'p',
      defaultFontName: 'Arial',
    };
    materialindentview_list:any [] = [];
    materialindentviewproduct_list:any [] = [];
    customer: any;
    responsedata: any;
    materialrequisition_gid: any;

  
    constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,private ToastrService: ToastrService,public service :SocketService) { }
  
    ngOnInit(): void {
      const materialrequisition_gid =this.router.snapshot.paramMap.get('materialrequisition_gid');    
       this.customer= materialrequisition_gid; 
       const secretKey = 'storyboarderp';
       const deencryptedParam = AES.decrypt(this.customer,secretKey).toString(enc.Utf8);
       console.log(deencryptedParam)
       this.GetMaterialIndentView(deencryptedParam);
       this.GetMaterialIndentViewProduct(deencryptedParam);
    }
  
    GetMaterialIndentView(materialrequisition_gid: any) {
      var url='ImsTrnMaterialIndent/GetMaterialIndentView'
      let param = {
        materialrequisition_gid : materialrequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.materialindentview_list = result.materialindentview_list;   
      });
    }
    GetMaterialIndentViewProduct(materialrequisition_gid: any) {
      var url='ImsTrnMaterialIndent/GetMaterialIndentViewProduct'
      let param = {
        materialrequisition_gid : materialrequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.materialindentviewproduct_list = result.materialindentviewproduct_list;   
      });
    }
  
  back(){
    this.route.navigate(['/ims/ImsTrnMIapproval']);
  
  }

 
  

  onSubmit() {
    debugger;
    var approvalapi = 'ImsTrnMaterialIndent/MaterialIndentApprove';
    let param = {
      materialrequisition_gid: this.materialindentview_list[0].materialrequisition_gid,
    }
    this.service.getparams(approvalapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      }
      else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/ims/ImsTrnMIapproval']);
      }
    });
  }
  onReject(){
    debugger;
    var rejectapi = 'ImsTrnMaterialIndent/MaterialIndentReject';
    let param = {
      materialrequisition_gid: this.materialindentview_list[0].materialrequisition_gid,
    }
    this.service.getparams(rejectapi, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      }
      else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/ims/ImsTrnMIapproval']);
      }
    });
  }
  
  }