import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
@Component({
  selector: 'app-ims-trn-statustrack',
  templateUrl: './ims-trn-statustrack.component.html',
  styleUrls: ['./ims-trn-statustrack.component.scss']
})
export class ImsTrnStatustrackComponent {


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
    Mitopo_list:any [] = [];
    Potopayment_list:any[]=[];
    customer: any;
    responsedata: any;
  
  
    constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,public service :SocketService) { }
  
    ngOnInit(): void {
      const materialrequisition_gid =this.router.snapshot.paramMap.get('materialrequisition_gid');    
       this.customer= materialrequisition_gid; 
       const secretKey = 'storyboarderp';
       const deencryptedParam = AES.decrypt(this.customer,secretKey).toString(enc.Utf8);
       console.log(deencryptedParam)
       this.GetMaterialIndentView(deencryptedParam);
       this.GetMaterialIndentViewProduct(deencryptedParam);
       this.GetMIToPO(deencryptedParam);
      //  this.GetPRtoPayment(deencryptedParam);
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
    GetMIToPO(materialrequisition_gid: any) {
      var url='ImsTrnMaterialIndent/GetMitoPO'
      let param = {
        materialrequisition_gid : materialrequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.Mitopo_list = result.Mitopo_list;   
      });
    }
  
    GetPRtoPayment(materialrequisition_gid: any) {
      var url='ImsTrnMaterialIndent/GetPRtoPayment'
      let param = {
        materialrequisition_gid : materialrequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.Potopayment_list = result.Potopayment_list;   
      });
    }
  back(){
    this.route.navigate(['/ims/ImsTrnMaterialindent']);
  
  }
  
  
  }