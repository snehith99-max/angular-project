import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';

@Component({
  selector: 'app-crm-trn-tcontactmanagement',
  templateUrl: './crm-trn-tcontactmanagement.component.html',
  styleUrls: ['./crm-trn-tcontactmanagement.component.scss']
})
export class CrmTrnTcontactmanagementComponent {
  parametervalue: any;
  parametervalues: any;
  service: any;
  contact_gid: any;
  contact_list: any;
  parameterValue1: any;
  leadbank_name: any;
  remarks: any;

  constructor(private SocketService: SocketService,private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService,private Router:Router){}
  ngOnInit(): void {
    this.NgxSpinnerService.show();
    var url= 'ContactManagement/ContactSummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      if(result.contact_list != null){
        $('#contactsummary').DataTable().destroy();
        this.contact_list = result.contactsummary;  
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#contactsummary').DataTable();
        }, 1);
      }
      else{
        this.contact_list = result.contactsummary; 
        setTimeout(()=>{   
          $('#contactsummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#contactsummary').DataTable().destroy()
      }
      

    });
  }
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  Tag(contact_gid:any){
    const parameter1 = `${contact_gid}`;
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
    var url = '/cmn/CmnMstTagcontact?hash=' + encodeURIComponent (encryptedParam);
    this.Router.navigateByUrl(url)
  }
  // viewContact(params:any){
    
  //   const parameter1 = `${params}`;
  //   const secretKey = 'storyboarderp';
  //   const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
  //   var url = '/cmn/CmnMstContactManagementView?hash=' + encodeURIComponent (encryptedParam);
  //   this.Router.navigateByUrl(url)
   
  //  }
  viewContact(contact_type: string,contact_gid:any) {
    if (contact_type === 'Individual') {
      const secretKey = 'storyboarderp';
      const param = (contact_gid);
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      this.Router.navigate(['/crm/CrmTrnContactIndividualView', encryptedParam])
    } else if (contact_type === 'Corporate') {
      const secretKey = 'storyboarderp';
      const param = (contact_gid);
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      this.Router.navigate(['/crm/CrmTrnContactCorporateView', encryptedParam])
    }

}

   editContact(contact_type:string,contact_gid:any){
    if (contact_type === 'Individual') { 
        const secretKey = 'storyboarderp';
        const param = (contact_gid);
        const encryptedParam = AES.encrypt(param, secretKey).toString();
        this.Router.navigate(['/crm/CrmTrnContactIndividualedit', encryptedParam])
      } else if (contact_type === 'Corporate') {
        const secretKey = 'storyboarderp';
        const param = (contact_gid);
        const encryptedParam = AES.encrypt(param, secretKey).toString();
        this.Router.navigate(['/crm/CrmTrnContactCorporateedit', encryptedParam])
      }
   }
   popmodal(parameter: string, parameter1: string) {
    this.parameterValue1 = parameter;
    this.remarks = parameter;
    this.leadbank_name = parameter1;
  }

   //delete
delete(contact_gid:any,contact_type:any){debugger
  this.parametervalue = contact_type 
  this.parametervalues = contact_gid 

}
ondelete(){

  var params = {
    contact_type: this.parametervalue,
    contact_gid: this.parametervalues
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
