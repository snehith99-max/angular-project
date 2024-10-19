import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-smr-trn-salesorderview',
  templateUrl: './smr-trn-salesorderview.component.html',
  styleUrls: ['./smr-trn-salesorderview.component.scss']
})
export class SmrTrnSalesorderviewComponent {
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
  Viewsalesorderdetail_list: any;

  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,private route:Router,private router:ActivatedRoute,public service :SocketService) { }

  ngOnInit(): void {
    debugger
    const salesorder_gid =this.router.snapshot.paramMap.get('salesorder_gid');
    const leadbank_gid =this.router.snapshot.paramMap.get('leadbank_gid');
    const lspage =this.router.snapshot.paramMap.get('lspage');
  
     this.customer= salesorder_gid;
     this.leadbank_gid= leadbank_gid;
     this.lspage= lspage;
  
     const secretKey = 'storyboarderp';
     const deencryptedParam = AES.decrypt(this.customer,secretKey).toString(enc.Utf8);
     const deencryptedParam1 = AES.decrypt(this.leadbank_gid,secretKey).toString(enc.Utf8);
     const deencryptedParam2 = AES.decrypt(this.lspage,secretKey).toString(enc.Utf8);
    
     this.lspage = deencryptedParam2;
  
     console.log(deencryptedParam)
     console.log(deencryptedParam1);
    
     this.GetViewsalesorderSummary(deencryptedParam);
     this.GetViewsalesorderdetails(deencryptedParam);
     this.GetGiddetails(deencryptedParam);




  }

  GetViewsalesorderSummary(salesorder_gid: any) {
    var url='SmrTrnSalesorder/GetViewsalesorderSummary'
    let param = {
      salesorder_gid : salesorder_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.Viewsalesordersummary_list = result.postsalesorder_list;   
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
    this.Viewsalesorderdetail_list = result.postsalesorderdetails_list;   
    this.NgxSpinnerService.hide()
    });
  }


//GetGidDetails
GetGiddetails(leadbank_gid: any){
  debugger
  var url = 'Leadbank360/GetGidDetails'
  let param = {
    leadbank_gid: leadbank_gid
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    // $('#leadorderdetails_list').DataTable().destroy();
    this.responsedata = result;
    this.gid_list = this.responsedata.gid_list;
 
    this.leadbank_gid = this.gid_list[0].leadbank_gid;
    this.lead2campaign_gid = this.gid_list[0].lead2campaign_gid;
    //this.salesorder_gid = this.gid_list[0].salesorder_gid;
 
    setTimeout(() => {
      $('#leadorderdetails_list').DataTable();
    }, 1);
    // console.log(this.responsedata.leadorderdetailslist,'leadorderdetails_list');
  });
 
}
 
 
  onback() {
    const secretKey = 'storyboarderp';
    const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
    const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid,secretKey).toString();
    const lspage1 = AES.encrypt(this.lspage,secretKey).toString();
 
   if (this.lspage == 'MM-Total') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-Upcoming') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-Lapsed') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-Longest') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-New') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-Prospect') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-Potential') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-mtd') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-ytd') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-Customer') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'MM-Drop') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-Today') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-New') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-Prospect') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-Potential') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-Customer') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-Drop') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-All') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else if (this.lspage == 'My-Upcoming') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,lead2campaign_gid,lspage1 ]);
   }
   else {
     this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
   }
 }
 
}