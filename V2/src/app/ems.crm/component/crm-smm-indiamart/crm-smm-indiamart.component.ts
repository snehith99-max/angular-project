import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { Location } from '@angular/common';

interface IWhatsapp {
  customer_type: string;
  bussiness_verticle: string;
  appointment_timing: string;
  lead_title: string;

}


@Component({
  selector: 'app-crm-smm-indiamart',
  templateUrl: './crm-smm-indiamart.component.html',
  styleUrls: ['./crm-smm-indiamart.component.scss']
})
export class CrmSmmIndiamartComponent {
  @ViewChild('Inbox') tableRef!: ElementRef;
  searchText = '';
  leadbank!: IWhatsapp;
  unique_query_id: any;
  indiamartsummarylist: any[] = [];
  responsedata: any;
  chatWindow: string = "Default";
  indiamartview_list: any;
  sender_name: any;
  last_sync_at: any;
  contactsync_till: any;
  nextsync_at: any;
  unique_query_count: any;
  query_type: any;
  sender_email: any;
  sender_mobile: any;
  sender_city: any;
  sender_state: any;
  sender_pincode: any;
  sender_country_iso: any;
  sender_mobile_alt: any;
  query_message: any;
  query_mcat_name: any;
  call_duration: any;
  query_product_name: any;
  sender_address: any;
  receiver_mobile: any;
  sender_company: any;
  leadbank_gid: any;
  reactiveForm!: FormGroup;
  customertype_list: any[] = [];
  selectedIndex: any;
  Getbussinessverticledropdown_list: any[] = [];

  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.sender_name.toLowerCase().includes(searchString) || item.sender_mobile.toLowerCase().includes(searchString);
  }
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {
    this.leadbank = {} as IWhatsapp;

  }

  ngOnInit(): void {

    this.reactiveForm = new FormGroup({
      customer_type: new FormControl(this.leadbank.customer_type, [
        Validators.required,
      ]),
      bussiness_verticle: new FormControl(this.leadbank.bussiness_verticle, [
      ]),
      appointment_timing: new FormControl(this.leadbank.appointment_timing, [
      ]),
      lead_title: new FormControl(this.leadbank.lead_title, [
      ]),

    });
    this.Getbussinessverticledropdown();
    this.Getindiamartsummary();
    this.Getsyncdetails();
    //customer type dropdown
    var api3 = 'Whatsapp/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list2;
    });
  }
  Getbussinessverticledropdown() {
    var api = 'AppointmentManagement/Getbussinessverticledropdown';
    this.service.get(api).subscribe((result: any) => {
      this.Getbussinessverticledropdown_list = result.Getbussinessverticledropdown_list;
    });
}
  Getsyncdetails() {
    var url1 = 'IndiaMART/Getsyncdetails'
    this.service.get(url1).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      this.contactsync_till = this.responsedata.contactsync_till;
      this.last_sync_at = this.responsedata.last_sync_at;
      this.nextsync_at = this.responsedata.nextsync_at;
      this.unique_query_count = this.responsedata.unique_query_count;
    });
  }
  Getindiamartsummary() {
    this.NgxSpinnerService.show();
    var url1 = 'IndiaMART/Getindiamartsummary'
    this.service.get(url1).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      this.indiamartsummarylist = this.responsedata.indiamartsummary_list;

      setTimeout(() => {
        $('#indiamartsummarylist').DataTable();
      }, 1);
    });
  }
  showResponsiveOutput(unique_query_id: string) {
    this.unique_query_id = unique_query_id;
    this.chatWindow = "Chat"
    this.GetindiamartviewSummary(unique_query_id);
  }

  GetindiamartviewSummary(unique_query_id: any) {
    this.NgxSpinnerService.show();
    var url = 'IndiaMART/GetindiamartviewSummary'
    let param = {
      unique_query_id: unique_query_id
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.query_type = this.responsedata.query_type;
      this.sender_name = this.responsedata.sender_name;
      this.sender_email = this.responsedata.sender_email;
      this.sender_company = this.responsedata.sender_company;
      this.sender_mobile = this.responsedata.sender_mobile;
      this.sender_city = this.responsedata.sender_city;
      this.sender_state = this.responsedata.sender_state;
      this.sender_pincode = this.responsedata.sender_pincode;
      this.sender_country_iso = this.responsedata.sender_country_iso;
      this.sender_mobile_alt = this.responsedata.sender_mobile_alt;
      this.query_message = this.responsedata.query_message;
      this.query_mcat_name = this.responsedata.query_mcat_name;
      this.call_duration = this.responsedata.call_duration;
      this.query_product_name = this.responsedata.query_product_name;
      this.sender_address = this.responsedata.sender_address;
      this.receiver_mobile = this.responsedata.receiver_mobile;
      this.leadbank_gid = this.responsedata.leadbank_gid;
      this.NgxSpinnerService.hide();
      const id = this.indiamartsummarylist.find(item => item.unique_query_id == unique_query_id)
      if (id) {
        id.read_flag = this.responsedata.read_flag
      }
    });
  }

  summaryrefresh() {
    this.Getindiamartsummary();
    this.Getsyncdetails();
  }

  get customer_type() {
    return this.reactiveForm.get('customer_type')!;
  }

  addtoleadbank() {
    let params = {
      bussiness_verticle: this.reactiveForm.value.bussiness_verticle,
      appointment_timing: this.reactiveForm.value.appointment_timing,
      lead_title: this.reactiveForm.value.lead_title,
      unique_query_id: this.unique_query_id
    }

    this.NgxSpinnerService.show();
    var url = 'IndiaMART/PostAddtoLeadBank'
    this.service.post(url, params).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      this.GetindiamartviewSummary(this.unique_query_id);
    });
  }

  onclose() {
    this.reactiveForm.reset();
  }

  markasunread() {
    this.chatWindow = "Default"
    this.NgxSpinnerService.show();
    var params = {
      unique_query_id: this.unique_query_id
    }
    var url = 'IndiaMart/markAsUnread';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        const id = this.indiamartsummarylist.find(item => item.unique_query_id == this.unique_query_id)
        if (id) {
          id.read_flag = "N"
        }
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
    });
  }

  highlightItem(i:any){
    this.selectedIndex = i;
  }
}