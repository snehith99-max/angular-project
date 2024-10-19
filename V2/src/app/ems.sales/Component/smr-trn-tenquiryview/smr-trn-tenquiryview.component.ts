import { Component, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-smr-trn-tenquiryview',
  templateUrl: './smr-trn-tenquiryview.component.html',
  styleUrls: ['./smr-trn-tenquiryview.component.scss']
})
export class SmrTrnTenquiryviewComponent {

  Viewenquirydetail_list: any;
  enquiryview_list: any;
  enquiry_gid:any;
  responsedata: any;

  constructor(private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router,private router:ActivatedRoute, public NgxSpinnerService:NgxSpinnerService) {
  }
  ngOnInit(): void {
    const enquiry_gid =this.router.snapshot.paramMap.get('enquiry_gid');
    this.enquiry_gid= enquiry_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.enquiry_gid,secretKey).toString(enc.Utf8);

    this.EnquiryView(deencryptedParam);
    this.EnquiryProductView(deencryptedParam);
  }
  EnquiryView(enquiry_gid: any) {
    var url='SmrTrnEnquiryView/GetEnquiryView'
    let param = {
      enquiry_gid : enquiry_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.enquiryview_list = result.GetEnquiryView;   
    });
  }

  EnquiryProductView(enquiry_gid: any) {
    debugger
    var url='SmrTrnEnquiryView/GetEnquiryProductView'
    let param = {
      enquiry_gid : enquiry_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.Viewenquirydetail_list = result.GetEnquiryViewProduct;   
    });
  }
  onback(){
    window.history.back();
  }
}
