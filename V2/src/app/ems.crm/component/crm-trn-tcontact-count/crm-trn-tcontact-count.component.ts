import { Component,Input } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-crm-trn-tcontact-count',
  templateUrl: './crm-trn-tcontact-count.component.html',
  styleUrls: ['./crm-trn-tcontact-count.component.scss']
})
export class CrmTrnTcontactCountComponent {
  Individual_count: any;
  Corporate_count: any;
  constructor( private SocketService:SocketService){

  }
  @Input() number: number = 1;
  total_count: any;
  getColor(buttonNumber: number): any {
    if ((buttonNumber === 2) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to right, #ffffff, #d1fbd1)'};
    } else if ((buttonNumber === 1) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to right, #ffffff, #f8ceab)'};;
    } else if ((buttonNumber === 3) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to right, #ffffff, #f6ccf6)'};;
    }
  }
  ngOnInit(){
    // var url= 'CmnMstContactManagement/ContactSummary';
    // this.SocketService.get(url).subscribe((result:any)=>{
    //   if(result.total_count != null){
    //     this.total_count = result.contactsummary[0].total_count; 
    //   }
    //   else{
    //     this.total_count = result.contactsummary[0].total_count; 
    //   }
    // });
    // var url= 'CmnMstContactManagement/ContactIndividualSummary';
    // this.SocketService.get(url).subscribe((result:any)=>{
    //   if(result.Individual_count != null){
    //     this.Individual_count = result.contactIndividualsummary[0].Individual_count; 
    //   }
    //   else{
    //     this.Individual_count = result.contactIndividualsummary[0].Individual_count; 
    //   }
    // });
    // var url= 'CmnMstContactManagement/ContactCorporateSummary';
    // this.SocketService.get(url).subscribe((result:any)=>{
    //   if(result.Corporate_count != null){
    //     this.Corporate_count = result.contactCorporatesummary[0].Corporate_count; 
    //   }
    //   else{
    //     this.Corporate_count = result.contactCorporatesummary[0].Corporate_count; 
    //   }
    // });
  }
}
