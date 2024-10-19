import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-srm-trn-newquotationview',
  templateUrl: './srm-trn-newquotationview.component.html',
  styleUrls: ['./srm-trn-newquotationview.component.scss']
})
export class SrmTrnNewquotationviewComponent {
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
  quotationsummary_list:any [] = [];
  Viewquotationdetail_list:any [] = [];
  quotation: any;
  responsedata: any;
  QAproductlist: any [] = [];
  lspage: any;
  leadbank_gid: any;
  lead2campaign_gid: any;

  constructor(private formBuilder: FormBuilder,public NgxSpinnerService:NgxSpinnerService,private route:Router,private router:ActivatedRoute,public service :SocketService) { }

  ngOnInit(): void {
   const quotation_gid =this.router.snapshot.paramMap.get('quotation_gid');
    this.quotation= quotation_gid;
    const lspage =this.router.snapshot.paramMap.get('lspage');
    this.lspage= lspage;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.quotation,secretKey).toString(enc.Utf8);
    const deencryptedParam2 = AES.decrypt(this.lspage,secretKey).toString(enc.Utf8);
    this.lspage = deencryptedParam2;
    console.log(deencryptedParam);
    
    
    this.GetViewQuotationSummary(deencryptedParam);
    this.GetViewquotationdetails(deencryptedParam);

  }

 


//view
GetViewQuotationSummary(quotation_gid: any) {
  var url='SmrTrnQuotation/GetViewQuotationSummary'
  let param = {
    quotation_gid : quotation_gid 
  }
  this.service.getparams(url,param).subscribe((result:any)=>{
  this.responsedata=result;
  this.quotationsummary_list = result.SO_list;   
  });
}


  GetViewquotationdetails(quotation_gid: any) {
    var url='SmrTrnQuotation/GetViewquotationdetails'
    this.NgxSpinnerService.show()
    let param = {
      quotation_gid : quotation_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.Viewquotationdetail_list = result.Sq_list;   
    this.NgxSpinnerService.hide()
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
     this.route.navigate(['/smr/SmrTrnQuotationSummary']);
   }
 }

}
