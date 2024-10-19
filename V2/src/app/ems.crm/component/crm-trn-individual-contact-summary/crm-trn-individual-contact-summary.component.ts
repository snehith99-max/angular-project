import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';

@Component({
  selector: 'app-crm-trn-individual-contact-summary',
  templateUrl: './crm-trn-individual-contact-summary.component.html',
  styleUrls: ['./crm-trn-individual-contact-summary.component.scss']
})
export class CrmTrnIndividualContactSummaryComponent {
  parametervalue: any;
  contact_gid: any;
  contact_list: any;
  responsedata:any;
  showOptionsDivId: any;
  parameterValue1: any;
  remarks: any;
  leadbank_name: any;


  constructor(private SocketService: SocketService,private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService,private Router:Router){}
  
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
       this.GetcontactIndividualsummary();
  
  }
  GetcontactIndividualsummary(){
    this.NgxSpinnerService.show();
  var url = 'ContactManagement/ContactIndividualSummary'
  this.SocketService.get(url).subscribe((result: any) => {
    $('#contactsummary').DataTable().destroy();
    this.responsedata = result;
    this.contact_list = this.responsedata.contactIndividualsummary;
    this.NgxSpinnerService.hide();
    setTimeout(() => {
      $('#contactsummary').DataTable();
    }, 1);


  });
}
toggleOptions(contact_gid: any) {
  if (this.showOptionsDivId === contact_gid) {
    this.showOptionsDivId = null;
  } else {
    this.showOptionsDivId = contact_gid;
  }
}
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  Tag(leadbank_gid:any){
    const secretKey = 'storyboarderp';
  const param = (leadbank_gid);
  const encryptedParam = AES.encrypt(param, secretKey).toString();
  this.Router.navigate(['/crm/CrmTrnContactIndividualedit', encryptedParam])
  }
  popmodal(parameter: string, parameter1: string) {
    this.parameterValue1 = parameter;
    this.remarks = parameter;
    this.leadbank_name = parameter1;
  }
  viewContact(leadbank_gid:any){
    
    const secretKey = 'storyboarderp';
  const param = (leadbank_gid);
  const encryptedParam = AES.encrypt(param, secretKey).toString();
  this.Router.navigate(['/crm/CrmTrnContactIndividualView', encryptedParam])
   
   }
   editContact(leadbank_gid:any){
    
    const secretKey = 'storyboarderp';
    const param = (leadbank_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.Router.navigate(['/crm/CrmTrnContactIndividualedit', encryptedParam])
   }

   //delete
   delete(contact_gid:any){debugger
    this.parametervalue = contact_gid 
  
  }
  ondelete(){
  
    var params = {
      contact_type: 'Individual',
      contact_gid: this.parametervalue
  }
    var url = 'ContactManagement/DeleteContact';
    this.SocketService.getparams(url, params).subscribe((result:any) => {
      if(result.status == true){
        this.ToastrService.success(result.message);
        window.scrollTo({
          top: 0,
        });
        this.ngOnInit();
          }
          else {
            this.ToastrService.warning(result.message);
          }   
        
        });
  }

}
