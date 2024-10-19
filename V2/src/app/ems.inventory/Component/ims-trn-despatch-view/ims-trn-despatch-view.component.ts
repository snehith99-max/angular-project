import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-ims-trn-despatch-view',
  templateUrl: './ims-trn-despatch-view.component.html',
  styleUrls: ['./ims-trn-despatch-view.component.scss']
})
export class ImsTrnDespatchViewComponent {

  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '25rem',
    minHeight: '5rem',
    width: '1230px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  Viewsalesordersummary_list:any [] = [];
  customer: any;
  responsedata: any;
  leadbank_gid:any;
  lspage:any;
  lead2campaign_gid:any;
  gid_list: any[] = [];
  Viewsalesordersummarydetail_list: any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,private route:Router,private router:ActivatedRoute,public service :SocketService) { }

  ngOnInit(): void {
    const salesorder_gid =this.router.snapshot.paramMap.get('salesorder_gid');    
     this.customer= salesorder_gid; 
  
     const secretKey = 'storyboarderp';
     const deencryptedParam = AES.decrypt(this.customer,secretKey).toString(enc.Utf8);
     
   
  
     console.log(deencryptedParam)
  
    
     this.GetViewsalesorderSummary(deencryptedParam);
     this.GetViewsalesorderdetails(deencryptedParam);
  }

  GetViewsalesorderSummary(salesorder_gid: any) {
    var url='SmrTrnSalesorder/GetViewsalesorderSummary'
    this.NgxSpinnerService.show()
    let param = {
      salesorder_gid : salesorder_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.Viewsalesordersummary_list = result.postsalesorder_list;   
    this.NgxSpinnerService.hide()
    });
  }

  GetViewsalesorderdetails(salesorder_gid: any) {
    var url='SmrTrnSalesorder/GetViewsalesorderdetails'
    this.NgxSpinnerService.show()
    let param = {
      salesorder_gid : salesorder_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.Viewsalesordersummarydetail_list = result.postsalesorderdetails_list;   
    this.NgxSpinnerService.hide()
    });
  }

back(){
  this.route.navigate(['/ims/ImsTrnAddDeliveryorder']);

}


}
