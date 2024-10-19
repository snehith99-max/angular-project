import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
@Component({
  selector: 'app-ims-trn-storerequisitionview',
  templateUrl: './ims-trn-storerequisitionview.component.html',
  styleUrls: ['./ims-trn-storerequisitionview.component.scss']
})
export class ImsTrnStorerequisitionviewComponent {


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
  
  
    constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,public service :SocketService) { }
  
    ngOnInit(): void {
      const storerequisition_gid =this.router.snapshot.paramMap.get('storerequisition_gid');    
       this.customer= storerequisition_gid; 
       const secretKey = 'storyboarderp';
       const deencryptedParam = AES.decrypt(this.customer,secretKey).toString(enc.Utf8);
       console.log(deencryptedParam)
       this.GetSRView(deencryptedParam);
       this.GetSRViewProduct(deencryptedParam);
    }
  
    GetSRView(storerequisition_gid: any) {
      var url='ImsTrnStoreRequisition/GetStoreView'
      let param = {
        storerequisition_gid : storerequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.materialindentview_list = result.GetStoreView_list;   
      });
    }
    GetSRViewProduct(storerequisition_gid: any) {
      var url='ImsTrnStoreRequisition/GetstoreViewProduct'
      let param = {
        storerequisition_gid : storerequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.materialindentviewproduct_list = result.GetstoreViewProduct_list;   
      });
    }
  
  back(){
    this.route.navigate(['/ims/ImsTrnStorerequisition']);
  
  }
  
  
  }