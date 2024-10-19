import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { Location } from '@angular/common';

import { encode, decode } from 'js-base64';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Pipe } from '@angular/core';
import flatpickr from 'flatpickr';
interface GmailApiInboxSummaryItem {
  body: string;
  sent_date: string;
  s_no: string;
  leadbank_gid: string;
}
@Component({
  selector: 'app-crm-trn-shopifycontactus',
  templateUrl: './crm-trn-shopifycontactus.component.html',
  styleUrls: ['./crm-trn-shopifycontactus.component.scss']
})
export class CrmTrnShopifycontactusComponent {
  gmailapiinboxsummary_list: any;
  responsedata: any;
  decodebody: any;
  name: any;
  showOptionsDivId: any;
  decodedList: any[] = [];
  reactiveFormappointment!: FormGroup;
  AssignForm!: FormGroup;
  Getbussinessverticledropdown_list: any[] = [];
  executive_list: any[] = [];
  GetTeamdropdown_list: any[] = [];
  s_no:any;
  post_list:any;
  names:any;
  comment:any;
  name_list:any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location, private sanitizer: DomSanitizer) {

  }

  ngOnInit(): void {
    
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.Getshopifyenquiry();
    this.Getshopifyenquirysummary();
    this.GetTeamdropdown();

    var api = 'AppointmentManagement/Getbussinessverticledropdown';
      this.service.get(api).subscribe((result: any) => {
        this.Getbussinessverticledropdown_list = result.Getbussinessverticledropdown_list;
      });
      this.reactiveFormappointment = new FormGroup({
        appointment_timing: new FormControl(''),
        bussiness_verticle: new FormControl(null),
        lead_title: new FormControl(null),
        leadbank_gid: new FormControl(''),
        teamname_gid: new FormControl(null),
        employee_gid: new FormControl(null),
        appointment_gid: new FormControl(null),
      });
    const today = new Date();
    const minDate = today;
    const Options = {
      enableTime: true,
      dateFormat: 'Y-m-d H:i:S',
      minDate: minDate,
      defaultDate: today,
      minuteIncrement: 1,
      placeholder:"YYYY-MM-DD"
    };
    flatpickr('.date-picker', Options);
  }
  Getshopifyenquiry() {
    var url = 'AppointmentManagement/shopifyenquiry';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
    });
  }
  
  get appointment_timing() {
    return this.reactiveFormappointment.get('appointment_timing')!;
  }
  get bussiness_verticle() {
    return this.reactiveFormappointment.get('bussiness_verticle')!;
  }
  get lead_title() {
    return this.reactiveFormappointment.get('lead_title')!;
  }
  get teamname_gid() {
    return this.reactiveFormappointment.get('teamname_gid')!;
  }
  onclose() {
    this.reactiveFormappointment.reset();
  }
  GetTeamdropdown() {
    var api = 'Assignvisit/Getmarketingteamdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.GetTeamdropdown_list = result.marketingteamdropdown_list;
    });
  }
  marketingteam() {
    debugger
    let campaign_gid = this.reactiveFormappointment.get("teamname_gid")?.value;

    let params = {
      campaign_gid: campaign_gid
    }
    var url = 'Assignvisit/Getmarketingteamdropdownonchange'

    this.service.getparams(url, params).subscribe((result: any) => {

      this.executive_list = result.Getexecutedropdown;

    });
  }
  popmodal(parameter: string, parameter1: string) {
    this.names = parameter
    this.comment = parameter1;
  } 
  
  Getshopifyenquirysummary() {
    debugger
    var url1 = 'AppointmentManagement/shopifyenquirysummary'
    this.service.get(url1).subscribe((result: any) => {
      this.responsedata = result;
      this.gmailapiinboxsummary_list = this.responsedata.gmailapiinboxsummary_list;
      
      const extractedDataList: any[] = [];
      console.log('knkaksnk', this.gmailapiinboxsummary_list);
      this.decodedList = this.gmailapiinboxsummary_list.map((item: GmailApiInboxSummaryItem) => {
        const decodedString = decode(item.body);
        const safeHtml: SafeHtml = this.sanitizer.bypassSecurityTrustHtml(decodedString);
        const htmlString: string = safeHtml.toString();
        const cleanText: string = htmlString.replace(/<\/?[^>]+(>|$)/g, "").replace(/ style=".*?"/g, '');
        const extractedData = parseContentToList(cleanText,item.sent_date,item.s_no,item.leadbank_gid );

        //extractedDataList.push(item.sent_date)
        return extractedData;
      });
      //console.log('wendkjew', this.decodedList);
    });
  }
  toggleOptions(region_gid: any) {
    if (this.showOptionsDivId === region_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = region_gid;
    }
  }
  onpop(parameter :any,comment:any){
   this.s_no = parameter;
   this.comment = comment;
  //  this.comment = comment;
  this.reactiveFormappointment.get("lead_title")?.setValue(this.comment);

   this.post_list=this.decodedList.find(item=> item.s_no ==this.s_no) ; 
   if (this.post_list) {
    const detailsArray = [
      this.post_list.name,
      this.post_list.email,
      this.post_list.phoneNumber,
    ];
    this.name_list = detailsArray.filter(detail => detail).join('\n');
  }
   //console.log('jlkk', this.name_list);
  }
  onaddopportunity(){
    if(this.reactiveFormappointment.value.teamname_gid!=null && 
      this.reactiveFormappointment.value.appointment_timing != null){
        let params = {
          lead_title : this.reactiveFormappointment.value.lead_title,
          teamname_gid : this.reactiveFormappointment.value.teamname_gid,
          employee_gid : this.reactiveFormappointment.value.employee_gid,
          appointment_timing : this.reactiveFormappointment.value.appointment_timing,
          post_list : this.post_list,
        }
        var url='Mycalls/Postaddtooportunity'
        this.service.post(url,params).subscribe((result: any) => {
          if(result.status == false){
            this.ToastrService.warning(result.message);
          }
          else{
            this.ToastrService.success(result.message);
          }
          this.Getshopifyenquirysummary();
        });
    }
    else{
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!');
    }
  }
}
function parseContentToList(htmlText: any,sent_date:any,s_no:any,leadbank_gid:any) {
  // Define patterns to match each field
  const patterns: { [key: string]: RegExp } = {
    countryCode: /Country Code:\s*([\w\s]+)\s*Name:/s,
    name: /Name:\s*([\w\s]+)\s*Email:/s,
    email: /Email:\s*([\w.-]+@[a-zA-Z_-]+?\.[a-zA-Z]{2,6})\s*Phone Number:/s,
    phoneNumber: /Phone Number:\s*([\d\s]+)\s*Comment:/s,
    comment: /Comment:\s*([\w\s]+)/s,
  };
  const extractedData: { [key: string]: string } = {};
  Object.keys(patterns).forEach(key => {
    const match = htmlText.match(patterns[key]);
    if (match) {
      extractedData[key] = match[1].trim();
      extractedData['sent_date'] = sent_date;
      extractedData['s_no'] = s_no;
      extractedData['leadbank_gid'] = leadbank_gid;
    }
  });
  return extractedData;
}

