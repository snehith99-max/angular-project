import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-crm-trn-leadbankview',
  templateUrl: './crm-trn-leadbankview.component.html',
  styleUrls: ['./crm-trn-leadbankview.component.scss']
})
export class CrmTrnLeadbankviewComponent {
  leadbank: any;
  leadbankedit_list:any;
  lspage: any;
  
  constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,public service :SocketService, private NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
   const leadbank_gid =this.router.snapshot.paramMap.get('leadbank_gid');
   const lspage = this.router.snapshot.paramMap.get('lspage');
    // console.log(termsconditions_gid)
    this.leadbank= leadbank_gid;
    this.lspage = lspage;
    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.leadbank,secretKey).toString(enc.Utf8);
    const deencryptedParam1 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
    
    this.lspage = deencryptedParam1;

    this.GetleadbankviewSummary(deencryptedParam);     
  }
  GetleadbankviewSummary(leadbank_gid: any) {
    debugger
    this.NgxSpinnerService.show();
    var url='Leadbank/GetleadbankviewSummary'
    let param = {
      leadbank_gid : leadbank_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.leadbankedit_list = result.leadbank_list;
    this.NgxSpinnerService.hide();
    //console.log(this.leadbankedit_list)
      
    });
  }
  back()
  {
    if (this.lspage == 'Leadbank') {
     this.route.navigate(['/crm/CrmTrnLeadbanksummary']); 
    }  
    else if (this.lspage == 'Registerlead'){
     this.route.navigate(['/crm/CrmTrnLeadMasterSummary']);
    }
    else if (this.lspage == 'Registerleaddistributor') {
      this.route.navigate(['/crm/CrmTrnLeadMasterSummary']); 
    } 
    else if (this.lspage == 'Registerleadcorporate') {
      this.route.navigate(['/crm/CrmTrnCorporateRegisterLead']); 
    } 
    else if (this.lspage == 'Registerleadretailer') {
      this.route.navigate(['/crm/CrmTrnRetailerRegisterLead']); 
    } 
    else if (this.lspage == 'LeadBankdistributor') {
      this.route.navigate(['/crm/CrmTrnLeadbanksummary']); 
    } 
    else if (this.lspage == 'LeadBankcorporate') {
      this.route.navigate(['/crm/CrmTrnCorporateLeadBank']); 
    } 
    else if (this.lspage == 'LeadBankretailer') {
      this.route.navigate(['/crm/CrmTrnRetailerLeadBank']); 
    } 
}
  
}
