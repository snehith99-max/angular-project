import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
interface ITask {
  customer_gid: string;
  customer_id: string;
  customer_name: string;
  contact_details: string;
  region_name: string;
  internal_notes: string;

}

@Component({
  selector: 'app-smr-trn-completed',
  templateUrl: './smr-trn-completed.component.html',
  styleUrls: ['./smr-trn-completed.component.scss']
})
export class SmrTrnCompletedComponent {
  new_list: any;
  MyTodayTaskCount_List :any;
  MyenquiryCount_list: any[] = [];
  task_list:any;
  prospect_list : any;
  Potential_list : any;
  All_list :any;
  Completed_list :any;
  Drop_list : any;
  task : ITask;
  responsedata : any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private router: ActivatedRoute, private route: Router, public service: SocketService) {
    this.task = {} as ITask;
    
  }
  ngOnInit(): void {
    //this.GetSmrTrnMyEnquiry();
    // this.GetSmrTrnMyEnquirynew();
    // this.GetSmrTrnMyEnquiryProspect();
    // this.GetSmrTrnMyEnquiryPotential();
     this.GetSmrTrnMyEnquiryCompleted();
    // this.GetSmrTrnMyEnquiryDrop();
    // this.GetSmrTrnMyEnquiryAll();
    
    var api7 = 'SmrTrnMyEnquiry/GetMyenquiryCount';
    this.service.get(api7).subscribe((result:any) => {
    this.responsedata = result;
    this.MyenquiryCount_list = result.MyenquiryCount_list; 
    console.log(this.MyenquiryCount_list,'testdata');
    }); 
}

  //// Summary Grid//////
  GetSmrTrnMyEnquiry() {
  var url = 'SmrTrnMyEnquiry/GetSmrTrnMyEnquiry'
  this.service.get(url).subscribe((result: any) => {
    $('#task_list').DataTable().destroy();
    this.responsedata = result;
    this.task_list = result.task_list;
    setTimeout(() => {
      $('#task_list').DataTable();
    }, 1);


  })
  
  
}
GetSmrTrnMyEnquirynew() {
  // var url = 'SmrTrnMyEnquiry/GetSmrTrnMyEnquirynew'
  // this.service.get(url).subscribe((result: any) => {
  //   $('#new_list').DataTable().destroy();
  //   this.responsedata = result;
  //   this.new_list = result.new_list;
  //   setTimeout(() => {
  //     $('#new_list').DataTable();
  //   }, 1);


  // })
  
  
}
GetSmrTrnMyEnquiryProspect() {
  var url = 'SmrTrnMyEnquiry/GetSmrTrnMyEnquiryProspect'
  this.service.get(url).subscribe((result: any) => {
    $('#prospect_list').DataTable().destroy();
    this.responsedata = result;
    this.prospect_list = result.prospect_list;
    setTimeout(() => {
      $('#prospect_list').DataTable();
    }, 1);


  })
  
  
}

GetSmrTrnMyEnquiryPotential() {
  var url = 'SmrTrnMyEnquiry/GetSmrTrnMyEnquiryPotential'
  this.service.get(url).subscribe((result: any) => {
    $('#Potential_list').DataTable().destroy();
    this.responsedata = result;
    this.Potential_list = result.Potential_list;
    setTimeout(() => {
      $('#Potential_list').DataTable();
    }, 1);


  })
  
  
}

GetSmrTrnMyEnquiryCompleted() {
  var url = 'SmrTrnMyEnquiry/GetSmrTrnMyEnquiryCompleted'
  this.service.get(url).subscribe((result: any) => {
    $('#Completed_list').DataTable().destroy();
    this.responsedata = result;
    this.Completed_list = result.Completed_list;
    setTimeout(() => {
      $('#Completed_list').DataTable();
    }, 1);


  })
  
  
}
GetSmrTrnMyEnquiryDrop() {
  var url = 'SmrTrnMyEnquiry/GetSmrTrnMyEnquiryDrop'
  this.service.get(url).subscribe((result: any) => {
    $('#Drop_list').DataTable().destroy();
    this.responsedata = result;
    this.Drop_list = result.Drop_list;
    setTimeout(() => {
      $('#Drop_list').DataTable();
    }, 1);


  })
  
  
}
GetSmrTrnMyEnquiryAll() {
  var url = 'SmrTrnMyEnquiry/GetSmrTrnMyEnquiryAll'
  this.service.get(url).subscribe((result: any) => {
    $('#All_list').DataTable().destroy();
    this.responsedata = result;
    this.All_list = result.All_list;
    setTimeout(() => {
      $('#All_list').DataTable();
    }, 1);


  })
  
  
}
Onopen(leadbank_gid:any,lead2campaign_gid:any,leadbankcontact_gid:any)
{
  debugger
  const secretKey = 'storyboarderp';
  const lspage1 = "/smr/SmrTrnCompleted";
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const param = (leadbank_gid);
  const param1 = (lead2campaign_gid);
  const param2 = (leadbankcontact_gid);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  const encryptedParam1 = AES.encrypt(param1,secretKey).toString();
  const encryptedParam2 = AES.encrypt(param2,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnSales360',encryptedParam,encryptedParam1,encryptedParam2,lspage]) 
}
  
  
  openModal5(){}

}
